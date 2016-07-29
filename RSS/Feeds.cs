using System;
using System.ComponentModel;
using System.Threading;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace RSS
{
    public delegate void FeedUpdatedEventHandler(double updatePercentage);

    public delegate void SubscriptionModifiedEventHandler(Subscription subscription);

    public class Feeds
    {
        private static object _instanceLock = new object();

        public event EventHandler AllFeedsParsed;

        private static Feeds _instance;
        public static Feeds Instance
        {
            get
            {
                lock(_instanceLock)
                {
                    if (_instance == null)
                    {
                        _instance = new Feeds();
                    }
                }
                return _instance;
            }
        }
        
        private Feeds()
        {
            Parser.SubscriptionParsedComplete += Parser_SubscriptionParsedComplete;
        }
        

        private void Parser_SubscriptionParsedComplete(Subscription subscription)
        {
            if (ContainsSubscription(subscription.Title))
            {
                OnFeedUpdated(subsParsed / Subscriptions.Count);
                if (++subsParsed >= Subscriptions.Count - 1)
                {
                    OnAllFeedsParsed();
                }
            }
        }

        public int MaxItems = 25;

        public List<Subscription> Subscriptions = new List<Subscription>();

		public List<string> Categories 
		{
			get 
			{
				return Subscriptions.GroupBy (ch => ch.Category).Select (g => g.First ().Category).ToList ();
			}
		}
       
        public List<Subscription> ChannelsByCategory(string category)
        {
            return Subscriptions.Where(ch => ch.Category == category).ToList();
        }

        public List<Item> DownloadedItems
        {
            get
            {
                var downloads = new List<Item>();
                
                foreach(var sub in Subscriptions)
                {
                    downloads.AddRange(sub.Items.Where(it => it.IsDownloaded));
                }
                                
                return downloads.ToList();
            }
        }


        public void RemoveChannel(string name)
        {
            if (!String.IsNullOrEmpty(name))
            {
                foreach (Subscription sub in Subscriptions)
                {
                    if (sub.Title == name)
                    {
                        Subscriptions.Remove(sub);
                        Save();
                        OnSubscriptionRemoved(sub);
                        break;
                    }
                }
            }
        }

        public void AddChannel(Subscription sub)
        {
            if (!String.IsNullOrEmpty(sub.RssLink) && !ContainsSubscription(sub.Title))
            {
                Subscriptions.Add(sub);
                OnSubscriptionAdded(sub);
                Save();
            }
        }

        public IEnumerable<Item> GetDownloads()
        {
            var downloads = new List<Item>();
            foreach(var sub in Subscriptions)
            {
                downloads.AddRange(sub.GetDownloads());
            }
            return downloads;
        }

        public void ParseAllFeedsAsync()
        {
            var thread = new Thread(ParseAllFeeds);
            thread.Start();
        }

        public void ParseAllFeeds()
        {
            subsParsed = 0;
            foreach (Subscription sub in Subscriptions)
            {
                Parser.LoadSubscription(sub, MaxItems);
            }

        }
        int subsParsed = 0;
        
        private void OnSubscriptionRemoved(Subscription subscription)
        {
            var copy = SubscriptionRemoved;
            if(copy != null)
            {
                copy.Invoke(subscription);
            }
        }

        private void OnSubscriptionAdded(Subscription subscription)
        {
            var copy = SubscriptionAdded;
            if(copy != null)
            {
                copy.Invoke(subscription);
            }
        }

        private void OnFeedUpdated(double percentUpdate)
        {
            var copy = FeedUpdated;
            if(copy != null)
            {
                copy.Invoke(percentUpdate);
            }
        }
        
        private void OnAllFeedsParsed()
        {
            var copy = AllFeedsParsed;
            if(copy != null)
            {
                copy.Invoke(this, null);
            }
        }

        private void Bw_DoWork(object sender, DoWorkEventArgs e)
        {
            var sub = e.Argument as Subscription;
            if (sub != null)
            {
                Parser.LoadSubscriptionAsync(sub);
            }
        }
        

        
        public event FeedUpdatedEventHandler FeedUpdated;

        public event SubscriptionModifiedEventHandler SubscriptionAdded;

        public event SubscriptionModifiedEventHandler SubscriptionRemoved;

        public void setChannelFeed(string title, string feed)
        {
            foreach (Subscription ch in Subscriptions)
            {
                if (ch.Title == title)
                {
                    ch.Feed = feed;
                }
            }
        }

        public void ToggleSubscription(Subscription subscription)
        {
            if (ContainsSubscription(subscription.Title))
            {
                RemoveChannel(subscription.Title);
            }
            else
            {
                AddChannel(subscription);
            }
        }

        public Subscription GetSubscriptionFromItem(string itemTitle)
        {
            return Subscriptions.FirstOrDefault(sub => sub.Items.Any(it => it.Title == itemTitle));
        }

        public Item GetItem(string title)
        {
            Subscription ch = Subscriptions.FirstOrDefault(m => m.Items.Any(p => p.Title == title));
            Item it = null;
            if (ch != null)
            {
                it = ch.Items.FirstOrDefault(m => m.Title == title);
            }
            return it;
        }


        public bool ContainsSubscription(string subscriptionTitle)
        {
            return Subscriptions.FirstOrDefault(s => s.Title == subscriptionTitle) != null;
        }

        public string GetSubscribeText(string subscriptionTitle)
        {
            var text = "Subscribe";
            if (ContainsSubscription(subscriptionTitle))
            {
                text = "Unsubscribe";
            }
            return text;
        }

        #region Save and Load


        public void Save()
        {
            Save(RSSConfig.ConfigFileName);
        }

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

                    foreach (Subscription ch in Subscriptions)
                    {
                        writer.WriteStartElement("Subscription");
                        writer.WriteElementString("Category_Name", ch.Category);
                        if (ch.Title == "")
                        {
                            writer.WriteElementString("Subscription_Name", "Default Name");
                        }
                        else
                        {
                            writer.WriteElementString("Subscription_Name", ch.Title);
                        }
                        writer.WriteElementString("Subscription_URL", ch.RssLink);
                        writer.WriteElementString("Subscription_Update", ch.Update.ToString());

                        writer.WriteEndElement();
                    }

                    writer.WriteEndElement();
                    writer.WriteEndDocument();
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex);
            }
        }

        //module loads RSS subscription and application configuration information from two seperate files(started, not done)
        public void Load(string fileName)
        {
            try
            {
                if (File.Exists(fileName))
                {
                    var contents = File.ReadAllText(fileName);
                    Subscriptions.Clear();
                    using (DataSet data = new DataSet())
                    {
                        data.ReadXml(fileName);
                        foreach (DataRow dataRow in data.Tables["Subscription"].Rows)
                        {
                            Subscription ch = new Subscription();
                            ch.Category = Convert.ToString(dataRow["Category_Name"]);
                            ch.Title = Convert.ToString(dataRow["Subscription_Name"]);
                            ch.RssLink = Convert.ToString(dataRow["Subscription_URL"]);
                            ch.Update = Convert.ToInt32(dataRow["Subscription_Update"]);
                            Subscriptions.Add(ch);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                ErrorLogger.Log(e);
            }
        }

        #endregion Save and Load

        public Subscription findChannelName(string name)
        {
            foreach (Subscription ch in Subscriptions)
            {
                if (name == ch.Title)
                {
                    return ch;
                }
            }
            return null;
        }

    }
}
