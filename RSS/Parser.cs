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
            ch.Items.Clear();
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
                MessageBox.Show(e.Message + ".\n" + ch.Title);
            }
            return false;
        }



        private static string GetXmlElementValue(XmlNode parentNode, string elementName)
        {
            string value = string.Empty;

            if (parentNode[elementName] != null)
            {
                value = parentNode[elementName].InnerText;
            }

            return value;
        }

        private static string GetXmlAttribute(XmlNode xmlNode, string attributeName)
        {
            string attribute = string.Empty;
            if (xmlNode != null)
            {
                string value = xmlNode.Attributes[attributeName]?.Value;
                if (!string.IsNullOrEmpty(value))
                {
                    attribute = value;
                }
            }

            return attribute;
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
                    ch.Title = GetXmlElementValue(channel, "title");
                    ch.SiteLink = GetXmlElementValue(channel, "link");
                    ch.Description = GetXmlElementValue(channel, "description");
                    ch.PubDate = GetXmlElementValue(channel, "pubDate");
                    ch.Ttl = GetXmlElementValue(channel, "ttl");
                    ch.LastBuildDate = GetXmlElementValue(channel, "lastBuildDate");

                    var imageNodes = channel.GetElementsByTagName("image");
                    if(imageNodes.Count > 0)
                    {
                        var imageNode = imageNodes[0];
                        ch.ImageUrl = GetXmlElementValue(imageNode, "url");
                    }

                    var items = channel.GetElementsByTagName("item");
                    foreach(XmlNode item in items)
                    {
                        var it = new Item();
                        it.Title = GetXmlElementValue(item, "title");
                        it.Link = GetXmlElementValue(item, "link");

                        if (item["enclosure"] != null)
                        {
                            it.Link = GetXmlAttribute(item["enclosure"], "url");
                        }

                        it.Description = processDescription(GetXmlElementValue(item, "description"));
                        it.Guid = GetXmlElementValue(item, "guid");
                        it.PubDate = GetXmlElementValue(item, "pubDate");

                        ch.Items.Add(it);
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
                    ch.Title = GetXmlElementValue(channel, "title");
                    ch.Description = GetXmlElementValue(channel, "description");
                    ch.SiteLink = GetXmlElementValue(channel, "link");

                    var items = channel.GetElementsByTagName("item");
                        
                    foreach(XmlNode item in items)
                    {
                        var it = new Item();
                        it.Title = GetXmlElementValue(item, "title");
                        it.Description = GetXmlElementValue(item, "description");
                        it.Link = GetXmlElementValue(item, "link");
                        it.Guid = GetXmlElementValue(item, "guid");
                        it.PubDate = GetXmlElementValue(item, "pubDate");

                        ch.Items.Add(it);
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
        


        public static bool LoadXMLAtom(Subscription ch, string rss, int maxItems)
        {
            bool success = true;
            try
            {
                var doc = new XmlDocument();
                doc.LoadXml(rss);
                var feedNode = doc["feed"];
                ch.Title = GetXmlElementValue(feedNode, "title");
                ch.ImageUrl = GetXmlElementValue(feedNode, "icon");

                var entries = feedNode.GetElementsByTagName("entry");
                foreach (XmlNode entry in entries)
                {
                    int count = 0;
                    var item = new Item();
                    item.Title = GetXmlElementValue(entry, "title");
                    item.Description = GetXmlElementValue(entry, "summary");
                    item.PubDate = GetXmlElementValue(entry, "updated");
                    
                    item.Link = GetXmlAttribute(entry["link"], "href");
                    if (entry["author"] != null)
                    {
                        foreach (XmlNode authorNode in entry["author"])
                        {
                            var auth = new author();
                            auth.name = GetXmlElementValue(authorNode, "name");
                            auth.email = GetXmlElementValue(authorNode, "email");
                            item.Authors.Add(auth);
                        }
                    }

                    ch.Items.Add(item);
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
