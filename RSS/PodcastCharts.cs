﻿using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Text;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Converters;
using System.Text.RegularExpressions;

namespace RSS
{
    public delegate void PodcastSourceUpdateEventHandler(double updatePercentage);

    public delegate void ErrorEventHandler(string errorMessage);

    public class PodcastCharts
    {
        

        public static Dictionary<string, int> PodcastGenreCodes = new Dictionary<string, int> {
			{ "Arts", 1301 },
			{ "Business", 1321 },
			{ "Comedy", 1303 },
			{ "Education", 1304 },
			{ "Games & Hobbies", 1323 },
			{ "Government & Organizations", 1325 },
			{ "Health", 1307 },
			{ "Kids & Family", 1305 },
			{ "Music", 1310 },
			{ "News & Politics", 1311 },
			{ "Religion & Spirituality", 1314 },
			{ "Science & Medicine", 1315 },
			{ "Society & Culture", 1324 },
			{ "Sports & Recreation", 1316 },
			{ "Technology", 1318 },
			{ "TV & Film", 1309 },
		};

        private static PodcastCharts _instance;
        private static object _instanceLock = new object();

        public static PodcastCharts Instance
        {
            get
            {
                lock (_instanceLock)
                {
                    if (_instance == null)
                    {
                        _instance = new PodcastCharts();
                    }
                }
                return _instance;
            }
        }

		public List<Subscription> Podcasts; 

        //https://itunes.apple.com/lookup?id=260190086&entity=podcast
        private const string iTunesLookupUrlFormat = "https://itunes.apple.com/lookup?id={0}&entity={1}";

        public const string iTunesPodcastFormat = "https://itunes.apple.com/us/rss/toppodcasts/limit={0}/genre={1}/xml";

        private const string EntityPodcast = "podcast";

        public static int Limit { get; set; }

        public static string Genre { get; set; }

        public const int DefaultLimit = 10;

        public event PodcastSourceUpdateEventHandler PodcastSourceUpdated;

        public event ErrorEventHandler ErrorEncountered;

        private PodcastCharts() 
		{
			Limit = DefaultLimit;
			Genre = "Comedy";
			Podcasts = new List<Subscription>();
		}


        private static string GetPodcastInfoJson(string podcastId)
        {
            string json = string.Empty;
            string url = string.Format(iTunesLookupUrlFormat, podcastId, EntityPodcast);

            var webClient = new WebClient();
            Stream st = webClient.OpenRead(url);

            using (var sr = new StreamReader(st))
            {
                json = sr.ReadToEnd();
            }

            return json;
        }

        public static List<Subscription> DeserializeSubscriptions(string json)
        {
            //Ex.
            //https://itunes.apple.com/lookup?id=278981407&entity=podcast
            var subscriptions = new List<Subscription>();
                        
            string feedUrl = string.Empty;
            JToken rootToken = JObject.Parse(json);
            JToken resultsToken = rootToken["results"];

            foreach (var subToken in resultsToken)
            {
                var sub = new Subscription();
                sub.RssLink = (string)subToken["feedUrl"];
                sub.Category = "Podcasts";
                sub.Title = (string)subToken["collectionName"];
                subscriptions.Add(sub);
            }

            return subscriptions;
        }

        private static string GetPodcastId(string itunesPodcastUrl)
        {
            //Ex.
            //https://itunes.apple.com/us/podcast/monday-morning-podcast/id480486345?mt=2&ign-mpt=uo=2
            // /id(\d)+
            string id = string.Empty;

            var match = Regex.Match(itunesPodcastUrl, @"/id(?<ID>(\d)+)");
            id = match.Groups["ID"].Value;

            return id;
        }

        private static string GetiTunesSourceUrl()
        {
            return string.Format(iTunesPodcastFormat, Limit, PodcastGenreCodes[Genre]);
        }

        private static string GetiTunesSourceRss()
        {
            var rss = string.Empty;
            var url = GetiTunesSourceUrl();
            var webClient = new WebClient();

            Stream st = webClient.OpenRead(url);

            using (var sr = new StreamReader(st))
            {
                rss = sr.ReadToEnd();
            }

            return rss;
        }

        private static Subscription GetiTunesPodcasts()
        {
            var url = GetiTunesSourceUrl();
                        
            var channel = new Subscription()
            {
                RssLink = url,
                Title = Genre.ToString(),
                Category = "iTunes"
            };

            Parser.LoadSubscription(channel, Feeds.Instance.MaxItems);

            return channel;
        }

        private void GetPodcasts()
        {
            try
            {
                var podcastsChart = GetiTunesPodcasts();
                
                int count = 0;
                foreach (var podcast in podcastsChart.Items)
                {                    
                    GetPodcastFromItem(podcast);

                    double percent = (double)(++count) / (double)podcastsChart.Items.Count;
                    OnPodcastSourceUpdated(percent);
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex);
                OnErrorEncountered(ex.Message);
            }
        }

        private void Bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            double percent = (double)(Podcasts.Count+1) / (double)Limit;
            OnPodcastSourceUpdated(percent);
        }

		private void OnErrorEncountered(string message)
		{
			var copy = ErrorEncountered;
			if (copy != null) 
			{
				copy.Invoke(message);
			}
		}

		private void OnPodcastSourceUpdated(double percentUpdated)
		{
			var copy = PodcastSourceUpdated;
			if (copy != null) 
			{
				copy.Invoke(percentUpdated);
			}
		}

        private void Bw_DoWork(object sender, DoWorkEventArgs e)
        {
            var item = e.Argument as Item;
            if(item != null)
            {
                GetPodcastFromItem(item);
            }
        }

        private void GetPodcastFromItem(Item podcastItem)
        {
            var podcastId = GetPodcastId(podcastItem.Link);
            var podcastInfoJson = GetPodcastInfoJson(podcastId);
            var subscriptions = DeserializeSubscriptions(podcastInfoJson);
            var sub = subscriptions.First();
            
            if (Podcasts.FirstOrDefault(p => p.Title == sub.Title) == null)
            {
                Podcasts.Add(sub);
            }

        }

        public void GetPodcastsAsync()
        {
            var thread = new Thread(GetPodcasts);
            thread.Start();
        }

    }
}