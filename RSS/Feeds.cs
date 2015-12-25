using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace RSS
{
    public class Feeds
    {
        private static Feeds _instance;
        public static Feeds Instance
        {
            get
            {
                if(_instance == null)
                {
                    _instance = new Feeds();
                }
                return _instance;
            }
        }

        public int MaxItems = 25;

        public List<Channel> Channels = new List<Channel>();

        public List<string> Categories => Channels.GroupBy(ch => ch.Category).Select(g => g.First().Category).ToList();
       
        public List<Channel> ChannelsByCategory(string category)
        {
            return Channels.Where(ch => ch.Category == category).ToList();
        }


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
                Parser.LoadAnyVersion(ch, MaxItems);
                Channels.Add(ch);
            }
        }

        public void parseAllFeeds()
        {
            int count = 0;
            foreach (Channel ch in Channels)
            {
                Parser.LoadAnyVersion(ch, MaxItems);
                InvokeUpdateProgress(100 * count / Channels.Count);
                count++;
            }
        }



        public delegate void UpdateProgress(int val);
        public event UpdateProgress UpdateProgressBar;



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
            catch (Exception ex)
            {
                ErrorLogger.Log(ex);
                MessageBox.Show(ex.Message);
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

        public Channel findChannelName(string name)
        {
            foreach (Channel ch in Channels)
            {
                if (name == ch.title)
                {
                    return ch;
                }
            }
            return null;
        }
    }
}
