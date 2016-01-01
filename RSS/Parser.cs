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


    public static class Parser
    {
        
        private static FeedType GetFeedType(string rssString)
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


        private static string processDescription(string description)
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

        public static bool LoadAnyVersion(Subscription ch, int maxItems)
        {
            ch.item.Clear();
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
                        LoadXMLAtom(ch, rss, maxItems);
                        break;
                    case FeedType.Rss1:
                        LoadXMLRSS1_0(ch, rss, maxItems);
                        break;
                    case FeedType.Rss2:
                        LoadXMLRSS2_0(ch, rss, maxItems);
                        break;
                }
                return true;
            }
            catch (WebException e)
            {
                ch.HasErrors = true;
                MessageBox.Show(e.Message + ".\n" + ch.title);
            }
            return false;
        }

       
        public static bool LoadXMLRSS2_0(Subscription ch, string rss, int maxItems)
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

        public static bool LoadXMLRSS1_0(Subscription ch, string rss, int maxItems)
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
        public static string dataRowContains(string name, DataRow dataRow, DataSet ds)
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

        public static bool LoadXMLAtom(Subscription ch, string rss, int maxItems)
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
