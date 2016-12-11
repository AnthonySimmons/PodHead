
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using PodHead;
using PodHead.Android.Views;

using Xamarin.Forms;

namespace PodHead.Android
{
    public class HomePage : TabbedPage
    {
        private StackLayout layout = new StackLayout();
        private Label title = new Label();

        private MediaPlayerView mediaPlayerView = new MediaPlayerView();
        private SubscriptionsView savedSubscriptionsView = new SavedSubscriptionsView();
        private SearchView searchView = new SearchView();
        private TopChartsView topChartsView = new TopChartsView();
        private DownloadsView downloadsView;

        
        private ContentPage mediaPlayerPage, subscriptionsPage, searchPage, topChartsPage, downloadsPage;

        private const string Subscriptions = "Subscriptions";
        private const string TopCharts = "Top Charts";
        private const string Search = "Search";
        private const string NowPlaying = "Now Playing";
        private const string Downloads = "Downloads";

        private readonly Feeds _feeds;
        private readonly Parser _parser;
        private readonly ErrorLogger _errorLogger;
        private readonly IConfig _config;

        public HomePage()
        {
            _config = Config.Instance;
            _errorLogger = ErrorLogger.Get(_config);
            _parser = Parser.Get(_config);
            _feeds = Feeds.Get(_parser, _config);

            Initialize();
        }

        
        private void Initialize()
        {
            downloadsView = new DownloadsView(_feeds, _parser);

            _errorLogger.ErrorFound += ErrorLogger_ErrorFound;

            _feeds.Load();
            _feeds.ParseAllFeedsAsync();
            this.PagesChanged += HomePage_PagesChanged;
            savedSubscriptionsView.ItemSelected += SubscriptionsView_ItemSelected;
            searchView.ItemSelected += SubscriptionsView_ItemSelected;
            topChartsView.ItemSelected += SubscriptionsView_ItemSelected;
            downloadsView.PlayItem += SubscriptionsView_ItemSelected;
            
            BackgroundColor = Color.White;
                        
            title.Text = "RSS";
            
            subscriptionsPage = new ContentPage { Content = savedSubscriptionsView, Title = Subscriptions };
            searchPage = new ContentPage { Content = searchView, Title = Search };
            topChartsPage = new ContentPage { Content = topChartsView, Title = TopCharts };
            mediaPlayerPage = new ContentPage { Content = mediaPlayerView, Title = NowPlaying };
            downloadsPage = new ContentPage { Content = downloadsView, Title = Downloads, Icon = "@drawable/icon" };

            LoadSubscriptions();

            Children.Add(subscriptionsPage);
            Children.Add(downloadsPage);
            Children.Add(mediaPlayerPage);
            Children.Add(topChartsPage);
            Children.Add(searchPage);

            this.CurrentPage = subscriptionsPage;            
        }

        private void HomePage_PagesChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if(SelectedItem == topChartsPage)
            {
                var topChartsView = topChartsPage.Content as TopChartsView;
                topChartsView.InitializeTopCharts();
            }
        }

        private void ErrorLogger_ErrorFound(string message)
        {
            DisplayAlert("Error", message, "Cancel");
        }

        private void SubscriptionsView_ItemSelected(Item item)
        {
            mediaPlayerView.LoadPlayer(item);
            mediaPlayerView.Play();

            CurrentPage = mediaPlayerPage;
        }

        private void LoadSubscriptions()
        {
            savedSubscriptionsView.LoadSubscriptions(_feeds.Subscriptions);
            _feeds.Save();
        }
                
    }
}

