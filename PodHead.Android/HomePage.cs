using PodHead.Android.Views;
using System.Collections.Generic;
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

        private static readonly Stack<Page> _previousPages = new Stack<Page>();

        private static readonly object _pageLock = new object();

        public static bool HasPreviousPages
        {
            get
            {
                lock (_pageLock)
                {
                    //The "current" page will always be a "previous" page.
                    return _previousPages.Count > 1 || IsOnItemView;
                }
            }
        }

        public static bool IsOnItemView { get; set; }
                

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
            lock (_pageLock)
            {
                _previousPages.Clear();
            }
            downloadsView = new DownloadsView(_feeds, _parser);

            _errorLogger.ErrorFound += ErrorLogger_ErrorFound;

            _feeds.Load();
            _feeds.ParseAllFeedsAsync();
            PagesChanged += HomePage_PagesChanged;
            CurrentPageChanged += HomePage_CurrentPageChanged;
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

            
            Children.Add(subscriptionsPage);
            Children.Add(downloadsPage);
            Children.Add(mediaPlayerPage);
            Children.Add(topChartsPage);
            Children.Add(searchPage);

            this.CurrentPage = subscriptionsPage;            
        }

        private void HomePage_CurrentPageChanged(object sender, System.EventArgs e)
        {
            lock (_pageLock)
            {
                if (_previousPages.Count == 0 || !IsCurrentPageThePreviousPage())
                {
                    _previousPages.Push(CurrentPage);
                }
            }
        }

        private bool IsCurrentPageThePreviousPage()
        {
            lock (_pageLock)
            {
                if (_previousPages.Count == 0)
                {
                    return false;
                }

                ContentPage currentContentPage = CurrentPage as ContentPage;
                ContentPage previousContentPage = _previousPages.Peek() as ContentPage;
                if (currentContentPage != null && previousContentPage != null)
                {
                    return Equals(currentContentPage.Content.GetType(), previousContentPage.Content.GetType());
                }
                return false;
            }
        }

        protected override bool OnBackButtonPressed()
        {
            lock (_pageLock)
            {
                if (!NotifySubscriptionOnBackPressed())
                {
                    if (_previousPages.Count >= 2)
                    {
                        _previousPages.Pop(); //The First "previous" is the current page.
                        Page previousPage = _previousPages.Pop();
                        CurrentPage = previousPage;
                    }
                }
                return base.OnBackButtonPressed();
            }
        }

        private bool NotifySubscriptionOnBackPressed()
        {
            ContentPage currentContentPage = CurrentPage as ContentPage;
            if (currentContentPage != null)
            {
                SubscriptionsView currentSubscriptionView = currentContentPage.Content as SubscriptionsView;
                if (currentSubscriptionView != null)
                {
                    return currentSubscriptionView.OnBackButtonPressed();
                }
            }
            return false;
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
#if DEBUG
            DisplayAlert("Error", message, "Cancel");
#endif
        }

        private void SubscriptionsView_ItemSelected(Item item)
        {
            //IsPlaying should be set after this method is called.

            if (item.IsPlaying)
            {
                mediaPlayerView.Pause();
            }
            else
            {
                mediaPlayerView.LoadPlayer(item);
                mediaPlayerView.Play();
                if (item.PercentPlayed == 0)
                {
                    CurrentPage = mediaPlayerPage;
                }
            }
        }        
                
    }
}

