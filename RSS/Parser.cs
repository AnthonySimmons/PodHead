using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Net;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace RSS
{
    public class Parser
    {
        public List<Channel> Channels = new List<Channel>();
        public int maxItems = 25;

        public delegate void UpdateProgress(int val);
        public event UpdateProgress UpdateProgressBar;




        #region Save and Load


        public void Save(string fileName)
        {
            try
            {
                string dir = Path.GetDirectoryName(fileName);
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                XmlWriterSettings settings = new XmlWriterSettings();
                settings.IndentChars = "\t";
                settings.Indent = true;
                using (XmlWriter writer = XmlWriter.Create(fileName, settings))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("RSS");

                    foreach (Channel ch in Channels)
                    {
                        writer.WriteStartElement("Subscription");
                        writer.WriteElementString("Category_Name", ch.title);
                        if (ch.title == "")
                        {
                            writer.WriteElementString("Subscription_Name", "Default Name");
                        }
                        else
                        {
                            writer.WriteElementString("Subscription_Name", ch.title);
                        }
                        writer.WriteElementString("Subscription_URL", ch.RssLink);
                        writer.WriteElementString("Subscription_Update", ch.update.ToString());

                        writer.WriteEndElement();
                    }

                    writer.WriteEndElement();
                    writer.WriteEndDocument();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        //module loads RSS subscription and application configuration information from two seperate files(started, not done)
        public void Load(string fileName)
        {
            try
            {
                Channels.Clear();
                using (DataSet data = new DataSet())
                {

                    data.ReadXml(fileName);
                    foreach (DataRow dataRow in data.Tables["Subscription"].Rows)
                    {
                        Channel ch = new Channel();
                        ch.Category = Convert.ToString(dataRow["Category_Name"]);
                        ch.title = Convert.ToString(dataRow["Subscription_Name"]);
                        ch.RssLink = Convert.ToString(dataRow["Subscription_URL"]);
                        ch.update = Convert.ToInt32(dataRow["Subscription_Update"]);
                        Channels.Add(ch);
                    }

                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        #endregion Save and Load


        public void RemoveChannel(string name)
        {
            if (!String.IsNullOrEmpty(name))
            {
                foreach (Channel ch in Channels)
                {
                    if (ch.title == name)
                    {
                        Channels.Remove(ch);
                        break;
                    }
                }
            }
        }

        public void AddChannel(Channel ch)
        {
            if (!String.IsNullOrEmpty(ch.RssLink) && !Channels.Any(m => m.RssLink == ch.RssLink))
            {
                loadAnyVersion(ch);
                Channels.Add(ch);
            }
        }

        public void parseAllFeeds()
        {
            int count = 0;
            foreach (Channel ch in Channels)
            {
                loadAnyVersion(ch);
                InvokeUpdateProgress(100 * count / Channels.Count);
                count++;
            }
        }

        private void InvokeUpdateProgress(int val)
        {
            UpdateProgress copy = UpdateProgressBar;
            if (copy != null)
            {
                copy(val);
            }
        }

        public void setChannelFeed(string title, string feed)
        {
            foreach (Channel ch in Channels)
            {
                if (ch.title == title)
                {
                    ch.feed = feed;
                }
            }
        }

        public Item GetItem(string title)
        {
            Channel ch = Channels.FirstOrDefault(m => m.item.Any(p => p.titleI == title));
            Item it = null;
            if (ch != null)
            {
                it = ch.item.FirstOrDefault(m => m.titleI == title);
            }
            return it;
        }

        public int getVersion(string rssString)
        {
            int version = 0;
            bool rss_section = false;
            //string[] strArr = System.Text.RegularExpressions.Regex.Split(rssString, " ");
            for (int i = 0; i < rssString.Length - 7; i++)
            {
                if (rssString.Substring(i, 4) == "<rss")
                {
                    version = 2;
                    rss_section = true;
                }
                if (rssString.Substring(i, 4) == "<rdf")
                {
                    version = 1;
                }
            }

            return version;
        }
        private string processDescription(string description)
        {
            string newDescrip = "";
            bool tag = false;
            for (int i = 0; i < description.Length; i++)
            {
                if (description[i] == '<')
                {
                    tag = true;
                }
                else if (description[i] == '>')
                {
                    tag = false;
                }
                else if (!tag)
                {
                    newDescrip += description[i];
                }
            }
            return newDescrip;
        }

        public bool loadAnyVersion(Channel ch)
        {
            //loadXMLRSS1_0(rss_sub);
            string url = ch.RssLink;
            string rss;
            WebClient wc = new WebClient();
            try
            {
                Stream st = wc.OpenRead(url);

                using (StreamReader sr = new StreamReader(st))
                {
                    rss = sr.ReadToEnd();
                }

                //version=0 -> Atom 1.0
                //version=1 -> RSS 1.0
                //version=2 -> RSS 2.0
                int version = getVersion(rss);
                if (version == 0)
                {
                    loadXMLAtom(ch, rss);
                }
                else if (version == 1)
                {
                    loadXMLRSS1_0(ch, rss);
                }
                else
                {
                    loadXMLRSS2_0(ch, rss);
                }

                return true;
            }
            catch (WebException e)
            {
                MessageBox.Show(e.Message + ".\n" + ch.title);
            }
            return false;
        }

        public Channel findChannelName(string name)
        {
            foreach (Channel ch in Channels)
            { 
                if(name == ch.title)
                {
                    return ch;
                }
            }
            return null;
        }
        public bool loadXMLRSS2_0(Channel ch, string rss)
        {
            using (DataSet rssData = new DataSet())
            {
                System.IO.StringReader sr = new System.IO.StringReader(rss);
                DataSet ds2 = new DataSet();

                rssData.ReadXmlSchema("../../RSS-2_0-Schema.xsd");
                //rssData.InferXmlSchema(sr, null);
                rssData.EnforceConstraints = false;
                rssData.ReadXml(sr, XmlReadMode.Auto);
                string str = rssData.GetXmlSchema();
                if (rssData.Tables.Contains("channel"))
                {
                    foreach (DataRow dataRow in rssData.Tables["channel"].Rows)
                    {
                        //ch.title = dataRowContains("title", dataRow, rssData);//Convert.ToString(dataRow["title"]);
                        
                        //rss_sub.set_title(ch.title);

                        ch.SiteLink = dataRowContains("link", dataRow, rssData);
                        ch.description = dataRowContains("description", dataRow, rssData);
                        
                        ch.lastBuildDate = dataRowContains("lastBuildDate", dataRow, rssData);
                        ch.pubDate = dataRowContains("pubDate", dataRow, rssData);

                        ch.ttl = dataRowContains("ttl", dataRow, rssData);
                      
                        ch.title = dataRowContains("title", dataRow, rssData);
                      

                        foreach (DataRow im in rssData.Tables["image"].Rows)
                        {
                            ch.ImageUrl = dataRowContains("url", im, rssData);
                        }

                        int counter = 0;
                        
                        foreach (DataRow itemRow in rssData.Tables["item"].Rows)
                        {
                            Item inside = new Item();
                            inside.titleI = dataRowContains("title", itemRow, rssData);
                            string desc = dataRowContains("description", itemRow, rssData);
                            inside.descriptionI = processDescription(desc);
                            inside.linkI = dataRowContains("link", itemRow, rssData);
                            inside.guidI = dataRowContains("guid", itemRow, rssData);
                            inside.pubDateI = dataRowContains("pubDate", itemRow, rssData);
                            
                            ch.item.Add(inside);
                            counter++;
                            if (counter > maxItems) { break; }
                        }
                    }
                }
                return true;
            }
        }

        public bool loadXMLRSS1_0(Channel ch, string rss)
        {
            using (DataSet rssData = new DataSet())
            {
                System.IO.StringReader sr = new System.IO.StringReader(rss);
                DataSet ds2 = new DataSet();
             
                //rssData.ReadXmlSchema("../../RSS-1_0-Schema.xsd");
                rssData.ReadXml(sr, XmlReadMode.InferSchema);
                if (rssData.Tables.Contains("channel"))
                {
                    foreach (DataRow dataRow in rssData.Tables["channel"].Rows)
                    {
                        ch.title = dataRowContains("title", dataRow, rssData);
                                    
                        ch.description = dataRowContains("description", dataRow, rssData);
                        ch.SiteLink = dataRowContains("link", dataRow, rssData);
                        int counter = 0;
                        if (rssData.Tables.Contains("item"))
                        {
                            foreach (DataRow itemRow in rssData.Tables["item"].Rows)
                            {
                                Item inside = new Item();
                                inside.titleI = dataRowContains("title", itemRow, rssData);//Convert.ToString(itemRow["title"]);
                                inside.descriptionI = dataRowContains("description", itemRow, rssData);//Convert.ToString(itemRow["description"]);
                                inside.linkI = dataRowContains("link", itemRow, rssData);//Convert.ToString(itemRow["link"]);
                                inside.guidI = dataRowContains("guid", itemRow, rssData);//Convert.ToString(itemRow["guid"]);
                                //inside.guidI = Convert.ToString(rssData.Tables["guid"].Rows[counter].ItemArray[1]);                        
                                inside.pubDateI = dataRowContains("pubDate", itemRow, rssData);//Convert.ToString(itemRow["pubDate"]);
                                
                                ch.item.Add(inside);
                                counter++;
                                if (counter > maxItems) { break; }
                            }
                        }
                    }
                }
                return true;
            }
        }
        public string dataRowContains(string name, DataRow dataRow, DataSet ds)
        {
            if (dataRow.Table.Columns.Contains(name))
            {
                return dataRow[name].ToString();
            }
            else if (ds.Tables.Contains(name))
            {
                string str = ds.Tables[name].ToString();
                string s = "";
                return s;
            }
            else
            {
                return "";
            }
        }
        public bool loadXMLAtom(Channel ch, string rss)
        {      
            using (DataSet rssData = new DataSet())
            {
                System.IO.StringReader sr = new System.IO.StringReader(rss);
                DataSet ds2 = new DataSet();
                rssData.ReadXml(sr, XmlReadMode.Auto);
                string str = rssData.GetXmlSchema();

                if(rssData.Tables.Contains("feed"))
                {
                    foreach (DataRow dataRow in rssData.Tables["feed"].Rows)
                    {
                        int c = rssData.Tables.Count;

                        if (rssData.Tables.Contains("link"))
                        {
                            foreach (DataRow dr in rssData.Tables["link"].Rows)
                            {
                                ch.SiteLink = dataRowContains("href", dr, rssData);
                                break;
                            }
                        }
                        ch.title = dataRowContains("title", dataRow, rssData);
                        
                        ch.description = dataRowContains("subtitle", dataRow, rssData);//Convert.ToString(dataRow["subtitle"]);
                        ch.pubDate = dataRowContains("updated", dataRow, rssData);//Convert.ToString(dataRow["updated"]);
                        int counter = 0;
                        if (rssData.Tables.Contains("entry"))
                        {
                            foreach (DataRow itemRow in rssData.Tables["entry"].Rows)
                            {
                                Item inside = new Item();
                                inside.titleI = dataRowContains("title", itemRow, rssData);//Convert.ToString(itemRow["title"]);
                                inside.descriptionI = dataRowContains("summary", itemRow, rssData);//Convert.ToString(itemRow["summary"]);
                                inside.linkI = dataRowContains("id", itemRow, rssData);//Convert.ToString(rssData.Tables["id"]);
                                inside.pubDateI = dataRowContains("updated", itemRow, rssData);//Convert.ToString(itemRow["updated"]);
                                
                                if (rssData.Tables.Contains("link"))
                                {
                                    foreach (DataRow dr in rssData.Tables["link"].Rows)
                                    {
                                        if (dr["href"].ToString().Contains(".html"))
                                        {
                                            inside.linkI = dr["href"].ToString();
                                        }
                                    }
                                }
                                if (rssData.Tables.Contains("author"))
                                {
                                    foreach (DataRow authorRow in rssData.Tables["author"].Rows)
                                    {
                                        author auth = new author();
                                        auth.name = dataRowContains("name", authorRow, rssData);
                                        auth.email = dataRowContains("email", authorRow, rssData);
                                        inside.authors.Add(auth);
                                    }
                                }
                                ch.item.Add(inside);
                                counter++;
                                if (counter > maxItems) { break; }
                            }
                        }
                    }
                    
                }
                return true;
            }
        }
    }
}
