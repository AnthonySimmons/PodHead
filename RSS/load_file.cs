using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace RSS
{
    public class load_file
    {
        [XmlAttribute("Channel")]
        public string channel;
        [XmlElement("Channel_name")]
        public string channel_name;
        [XmlElement("Subscription_name")]
        public string Subscription_name;
        [XmlElement("Subscription_URL")]
        public string Subscription_URL;
        [XmlElement("Subscription_Update")]
        public int Subscription_Update;        
    }
}
