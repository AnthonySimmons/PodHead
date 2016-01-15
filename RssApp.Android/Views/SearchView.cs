using RSS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace RssApp.Android.Views
{
    class SearchView : SubscriptionsView
    {

        private SearchBar searchEntry = new SearchBar();

        public SearchView() : base()
        {
            Initialize();
        }
        
        private void Initialize()
        {
            PodcastSearch.Instance.SearchResultReceived += Instance_SearchResultReceived;
            
            searchEntry.SearchButtonPressed += SearchEntry_SearchButtonPressed;
            searchEntry.BackgroundColor = Color.Gray;
            searchEntry.TextColor = Color.White;

            RefreshButton.IsVisible = false;

            stackLayout.Children.Insert(0, searchEntry);
        }

        private void Instance_SearchResultReceived(List<Subscription> subscriptions)
        {
            Device.BeginInvokeOnMainThread(() => LoadSearchResults(subscriptions));
        }

        private void SearchEntry_SearchButtonPressed(object sender, EventArgs e)
        {
            PodcastSearch.Instance.SearchAsync(searchEntry.Text);
        }


        private void LoadSearchResults(List<Subscription> subscriptions)
        {
            LoadSubscriptionResults(subscriptions);
        }
        
        private void PerformSearch()
        {
            PodcastSearch.Instance.Results.Clear();
            PodcastSearch.Instance.SearchAsync(searchEntry.Text);
        }

    }
}