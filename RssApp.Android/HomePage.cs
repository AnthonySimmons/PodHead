﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using RSS;
using RssApp.Android.Views;

using Xamarin.Forms;

namespace RssApp.Android
{
    public class HomePage : TabbedPage
    {
        private StackLayout layout = new StackLayout();
        private Label title = new Label();       
        
        private MediaPlayerView mediaPlayerView = new MediaPlayerView();
        private SubscriptionsView subscriptionsView = new SavedSubscriptionsView();
        private SearchView searchView = new SearchView();
        private TopChartsView topChartsView = new TopChartsView();

        
        private ContentPage mediaPlayerPage, subscriptionsPage, searchPage, topChartsPage;

        private const string Subscriptions = "Subscriptions";
        private const string TopCharts = "Top Charts";
        private const string Search = "Search";
        private const string NowPlaying = "Now Playing";

        public HomePage()
        {
            Initialize();
        }

        
        private void Initialize()
        {
            Feeds.Instance.Load(RSSConfig.ConfigFileName);
            Feeds.Instance.ParseAllFeeds();
            subscriptionsView.ItemSelected += SubscriptionsView_ItemSelected;
            searchView.ItemSelected += SubscriptionsView_ItemSelected;
            topChartsView.ItemSelected += SubscriptionsView_ItemSelected;
            
            BackgroundColor = Color.Silver;
                        
            title.Text = "RSS";
            
            subscriptionsPage = new ContentPage { Content = subscriptionsView, Title = Subscriptions };
            searchPage = new ContentPage { Content = searchView, Title = Search };
            topChartsPage = new ContentPage { Content = topChartsView, Title = TopCharts };
            mediaPlayerPage = new ContentPage { Content = mediaPlayerView, Title = NowPlaying };

            LoadSubscriptions();

            Children.Add(subscriptionsPage);
            Children.Add(searchPage);
            Children.Add(topChartsPage);
            Children.Add(mediaPlayerPage);

            this.CurrentPage = subscriptionsPage;            
        }

        private void SubscriptionsView_ItemSelected(Item item)
        {
            mediaPlayerView.LoadPlayer(item);
            mediaPlayerView.Play();

            CurrentPage = mediaPlayerPage;
        }

        private void LoadSubscriptions()
        {
            subscriptionsView.LoadSubscriptions(Feeds.Instance.Subscriptions);
            Feeds.Instance.Save(RSSConfig.ConfigFileName);
        }
                
    }
}

