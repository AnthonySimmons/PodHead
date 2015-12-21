using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace RSS
{
    public class Channel
    {
        public string feed;
        
        public string version;
        
        public string title;
        
        public string description;
        
        public string RssLink;
        
        public string lastBuildDate;
        
        public string pubDate;
        
        public string ttl;
        
        public int update;
        
        public List<Item> item = new List<Item>();
        
        public string SiteLink;

        public string ImageUrl;

        public string Category;

        public bool HasErrors;
    }
}
