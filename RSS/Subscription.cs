using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace RSS
{
    public class Subscription
    {
        public string Feed { get; set; } = string.Empty;

        public string Version { get; set; } = string.Empty;

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string RssLink { get; set; } = string.Empty;

        public string LastBuildDate { get; set; } = string.Empty;

        public string PubDate { get; set; } = string.Empty;

        public string Ttl { get; set; } = string.Empty;

        public int Update { get; set; } = 0;
        
        public List<Item> Items { get; set; } = new List<Item>();

        public string SiteLink { get; set; } = string.Empty;

        public string ImageUrl { get; set; } = string.Empty;

        public string Category { get; set; } = string.Empty;

        public bool HasErrors { get; set; } = false;
    }
}
