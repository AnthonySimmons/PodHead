using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace RSS
{
    public enum PodcastGenre
    {
        Comedy = 1303,

    }

    public static class PodcastSource
    {
        public const string iTunesPodcastFormat = "https://itunes.apple.com/us/rss/toppodcasts/limit={0}/genre={1}/xml";

        public static int Limit { get; set; } = 10;

        public static PodcastGenre Genre { get; set; } = PodcastGenre.Comedy;

        private static string GetPodcastSourceUrl()
        {
            return string.Format(iTunesPodcastFormat, Limit, (int)Genre);
        }

        private static string GetPodcastSourceRss()
        {
            var rss = string.Empty;
            var url = GetPodcastSourceUrl();
            var webClient = new WebClient();

            Stream st = webClient.OpenRead(url);

            using (var sr = new StreamReader(st))
            {
                rss = sr.ReadToEnd();
            }

            return rss;
        }

        public static Channel GetPodcasts()
        {
            var url = GetPodcastSourceUrl();

            var parser = new Parser();
            var channel = new Channel()
            {
                RssLink = url,
                title = Genre.ToString(),
            };

            parser.loadAnyVersion(channel);

            return channel;
        }

    }
}
