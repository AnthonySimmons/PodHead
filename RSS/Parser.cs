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
    public enum FeedType
    {
        Atom = 0,
        Rss1 = 1,
        Rss2 = 2,
    }


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

        private FeedType GetFeedType(string rssString)
        {
            FeedType type = FeedType.Atom;

            var doc = new XmlDocument();
            doc.LoadXml(rssString);
            var rssTags = doc.GetElementsByTagName("rss");
            if (rssTags != null && rssTags.Count > 0)
            {
                string rssVersion = rssTags[0].Attributes["version"].Value;
                if (rssVersion == "1.0")
                {
                    type = FeedType.Rss1;
                }
                if(rssVersion == "2.0")
                {
                    type = FeedType.Rss2;
                }
            }
            
            return type;
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
                
                FeedType feedType = GetFeedType(rss);
                switch(feedType)
                {
                    case FeedType.Atom:
                        loadXMLAtom(ch, rss);
                        break;
                    case FeedType.Rss1:
                        loadXMLRSS1_0(ch, rss);
                        break;
                    case FeedType.Rss2:
                        loadXMLRSS2_0(ch, rss);
                        break;
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
            bool success = true;
            try
            {
                var xmlDocument = new XmlDocument();
                var bytes = Encoding.UTF8.GetBytes(rss);
                using (var stream = new MemoryStream(bytes))
                {
                    xmlDocument.Load(stream);
                }
                var channels = xmlDocument.GetElementsByTagName("channel");
                foreach (XmlElement channel in channels)
                {
                    int counter = 0;
                    ch.title = channel["title"]?.InnerText;
                    ch.SiteLink = channel["link"]?.InnerText;
                    ch.description = channel["description"]?.InnerText;
                    ch.pubDate = channel["pubDate"]?.InnerText;
                    ch.ttl = channel["ttl"]?.InnerText;
                    ch.lastBuildDate = channel["lastBuildDate"]?.InnerText;

                    var imageNodes = channel.GetElementsByTagName("image");
                    if(imageNodes.Count > 0)
                    {
                        var imageNode = imageNodes[0];
                        ch.ImageUrl = imageNode["url"].InnerText;
                    }

                    var items = channel.GetElementsByTagName("item");
                    foreach(XmlNode item in items)
                    {
                        var it = new Item();
                        it.titleI = item["title"]?.InnerText;
                        it.linkI = item["link"]?.InnerText;
                        it.descriptionI = processDescription(item["description"]?.InnerText);
                        it.guidI = item["guid"]?.InnerText;
                        it.pubDateI = item["pubDate"]?.InnerText;

                        ch.item.Add(it);
                        counter++;
                        if(counter > maxItems) { break; }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex);
                success = false;
            }
            ch.HasErrors = !success;
            return success;
        }

        public bool loadXMLRSS1_0(Channel ch, string rss)
        {
            bool success = true;
            try
            {
                var xmlDocument = new XmlDocument();
                var bytes = Encoding.UTF8.GetBytes(rss);
                using (var stream = new MemoryStream(bytes))
                {
                    xmlDocument.Load(stream);
                }
                var channels = xmlDocument.GetElementsByTagName("channel");
                foreach (XmlElement channel in channels)
                {
                    int count = 0;
                    ch.title = channel["title"]?.InnerText;
                    ch.description = channel["description"]?.InnerText;
                    ch.SiteLink = channel["link"]?.InnerText;

                    var items = channel.GetElementsByTagName("item");
                        
                    foreach(XmlNode item in items)
                    {
                        var it = new Item();
                        it.titleI = item["title"]?.InnerText;
                        it.descriptionI = item["description"]?.InnerText;
                        it.linkI = item["link"]?.InnerText;
                        it.guidI = item["guid"]?.InnerText;
                        it.pubDateI = item["pubDate"]?.InnerText;

                        ch.item.Add(it);
                        count++;
                        if(count > maxItems) { break; }
                    }

                }
            }
            catch(Exception ex)
            {
                ErrorLogger.Log(ex);
                success = false;                
            }
            ch.HasErrors = !success;
            return success;
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
            bool success = true;
            try
            {
                var doc = new XmlDocument();
                doc.LoadXml(rss);
                var feedNode = doc["feed"];
                ch.title = feedNode["title"]?.InnerText;
                
                ch.ImageUrl = feedNode["icon"]?.InnerText;

                var entries = feedNode.GetElementsByTagName("entry");
                foreach (XmlNode entry in entries)
                {
                    int count = 0;
                    var item = new Item();
                    item.titleI = entry["title"].InnerText;
                    item.descriptionI = entry["summary"].InnerText;
                    item.pubDateI = entry["updated"].InnerText;

                    item.linkI = entry["link"]?.Attributes["href"]?.Value;

                    if (entry["author"] != null)
                    {
                        foreach (XmlNode authorNode in entry["author"])
                        {
                            var auth = new author();
                            auth.name = authorNode["name"].InnerText;
                            auth.email = authorNode["email"].InnerText;
                            item.authors.Add(auth);
                        }
                    }

                    ch.item.Add(item);
                    count++;
                    if(count > maxItems) { break; }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex);
                success = false;
            }
            ch.HasErrors = !success;
            return success;
        }
    }
}
