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
        public string Feed { get; set; }

        public string Version { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string RssLink { get; set; }

        public string LastBuildDate { get; set; }

        public string PubDate { get; set; }

        public string Ttl { get; set; }

        public int Update { get; set; }
        
        public List<Item> Items { get; set; }

        public string SiteLink { get; set; }

        public string ImageUrl { get; set; }

        public string Category { get; set; }

        public bool HasErrors { get; set; }

        public bool IsLoaded { get; set; }

        public int MaxItems { get; set; }

        public Subscription()
        {
            Feed = string.Empty;
            Version = string.Empty;
            Title = string.Empty;
            Description = string.Empty;
            RssLink = string.Empty;
            LastBuildDate = string.Empty;
            PubDate = string.Empty;
            Ttl = string.Empty;

            Items = new List<Item>();
            SiteLink = string.Empty;
            ImageUrl = string.Empty;
            Category = string.Empty;
            MaxItems = Feeds.Instance.MaxItems;
        }
    }
}
