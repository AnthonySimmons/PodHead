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
        
        private Feeds() { }

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


        public void RemoveChannel(string name)
        {
            if (!String.IsNullOrEmpty(name))
            {
                foreach (Subscription ch in Subscriptions)
                {
                    if (ch.Title == name)
                    {
                        Subscriptions.Remove(ch);
                        break;
                    }
                }
            }
        }

        public void AddChannel(Subscription ch)
        {
            if (!String.IsNullOrEmpty(ch.RssLink) && !Subscriptions.Any(m => m.RssLink == ch.RssLink))
            {
                Parser.LoadSubscriptionAsync(ch, MaxItems);
                Subscriptions.Add(ch);
            }
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
                Parser.LoadSubscriptionAsync(sub, MaxItems);
            }
        }
        int subsParsed = 0;

        private void Bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            OnFeedUpdated((double)++subsParsed / (double)Subscriptions.Count);
        }

		private void OnFeedUpdated(double percentUpdate)
		{
			var copy = FeedUpdated;
			if (copy != null) 
			{
				copy (percentUpdate);
			}
		}

        private void Bw_DoWork(object sender, DoWorkEventArgs e)
        {
            var sub = e.Argument as Subscription;
            if (sub != null)
            {
                Parser.LoadSubscriptionAsync(sub, MaxItems);
            }
        }
        

        public delegate void FeedUpdatedEventHandler(double updatePercentage);
        public event FeedUpdatedEventHandler FeedUpdated;

        
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
