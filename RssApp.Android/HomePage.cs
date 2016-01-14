
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
        private SubscriptionsView subscriptionsView = new SubscriptionsView();
        private SearchView searchView = new SearchView();
        private TopChartsView topChartsView = new TopChartsView();

        private ItemsView itemsView = new ItemsView();              
        
        private ContentPage mediaPlayerPage, subscriptionsPage, searchPage, topChartsPage;

        private View PreviousView;
        
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
            
            subscriptionsView.SubscriptionSelected += SubscriptionsView_SubscriptionSelected;
            searchView.SubscriptionSelected += SubscriptionsView_SubscriptionSelected;
            topChartsView.SubscriptionSelected += SubscriptionsView_SubscriptionSelected;

            itemsView.PlayItem += ItemsView_PlayItem;
            itemsView.BackSelected += ItemsView_BackSelected;

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

        private void ItemsView_BackSelected(object sender, EventArgs e)
        {
            var page = (ContentPage)CurrentPage;
            page.Content = PreviousView;
        }

        private void ItemsView_PlayItem(Item item)
        {
            mediaPlayerView.LoadPlayer(item);
            mediaPlayerView.Play();

            CurrentPage = mediaPlayerPage;
        }               

        private void SubscriptionsView_SubscriptionSelected(Subscription subscription)
        {
            itemsView.IsVisible = true;
            var page = (ContentPage)CurrentPage;
            PreviousView = page.Content;
            page.Content = itemsView;
            itemsView.LoadSubscription(subscription);
        }
        
        private void LoadSubscriptions()
        {
            subscriptionsView.LoadSubscriptions(Feeds.Instance.Subscriptions);
            Feeds.Instance.Save(RSSConfig.ConfigFileName);
        }
                
    }
}

