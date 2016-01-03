using System;
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

    public delegate void PodcastSourceErrorEventHandler(string errorMessage);

    public class PodcastSource
    {
        

        public static Dictionary<string, int> PodcastGenreCodes = new Dictionary<string, int>
        {
            ["Arts"] = 1301,
            ["Business"] = 1321,
            ["Comedy"] = 1303,
            ["Education"] = 1304,
            ["Games & Hobbies"] = 1323,
            ["Government & Organizations"] = 1325,
            ["Health"] = 1307,
            ["Kids & Family"] = 1305,
            ["Music"] = 1310,
            ["News & Politics"] = 1311,
            ["Religion & Spirituality"] = 1314,
            ["Science & Medicine"] = 1315,
            ["Society & Culture"] = 1324,
            ["Sports & Recreation"] = 1316,
            ["Technology"] = 1318,
            ["TV & Film"] = 1309,
        };

        private static PodcastSource _instance;
        private static object _instanceLock = new object();

        public static PodcastSource Instance
        {
            get
            {
                if(_instance == null)
                {
                    lock(_instanceLock)
                    {
                        _instance = new PodcastSource();
                    }
                }
                return _instance;
            }
        }

        public List<Subscription> Podcasts { get; } = new List<Subscription>();

        //https://itunes.apple.com/lookup?id=260190086&entity=podcast
        private const string iTunesLookupUrlFormat = "https://itunes.apple.com/lookup?id={0}&entity={1}";

        public const string iTunesPodcastFormat = "https://itunes.apple.com/us/rss/toppodcasts/limit={0}/genre={1}/xml";

        private const string EntityPodcast = "podcast";

        public static int Limit { get; set; } = DefaultLimit;

        public static string Genre { get; set; } = "Comedy";

        public const int DefaultLimit = 10;

        public event PodcastSourceUpdateEventHandler PodcastSourceUpdated;

        public event PodcastSourceErrorEventHandler ErrorEncountered;

        private PodcastSource() { }


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

        private static Subscription GetSubscription(string json)
        {
            //Ex.
            //https://itunes.apple.com/lookup?id=278981407&entity=podcast
            var sub = new Subscription();

            string feedUrl = string.Empty;
            JToken rootToken = JObject.Parse(json);
            JToken resultsToken = rootToken["results"];
            JToken subToken = resultsToken.First;

            sub.RssLink = (string)subToken["feedUrl"];
            sub.Category = "Podcasts";
            sub.Title = (string)subToken["collectionName"];
            return sub;
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

            Parser.LoadAnyVersion(channel, Feeds.Instance.MaxItems);

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
                    var bw = new BackgroundWorker();
                    bw.DoWork += Bw_DoWork;
                    bw.RunWorkerCompleted += Bw_RunWorkerCompleted;
                    bw.RunWorkerAsync(podcast);

                    //double percent = (double)(++count) / (double)podcastsChart.Items.Count;
                    //PodcastSourceUpdated?.Invoke(percent);
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex);
                ErrorEncountered?.Invoke(ex.Message);
            }
        }

        private void Bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            double percent = (double)(Podcasts.Count+1) / (double)Limit;
            PodcastSourceUpdated?.Invoke(percent);
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
            var subscription = GetSubscription(podcastInfoJson);
            Parser.LoadAnyVersion(subscription, Feeds.Instance.MaxItems);

            if (Podcasts.FirstOrDefault(p => p.Title == subscription.Title) == null)
            {
                Podcasts.Add(subscription);
            }

        }

        public void GetPodcastsAsync()
        {
            var thread = new Thread(GetPodcasts);
            thread.Start();
        }

    }
}
