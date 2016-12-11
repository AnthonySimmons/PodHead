using RSS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace PodHead.Android.Views
{
    class SearchView : SubscriptionsView
    {
        private SearchBar searchEntry = new SearchBar();

        public SearchView()
        {
            Initialize();
        }
        
		protected override void Initialize()
        {
			base.Initialize ();

            PodcastSearch.Instance.SearchResultReceived += Instance_SearchResultReceived;
            
            searchEntry.SearchButtonPressed += SearchEntry_SearchButtonPressed;
            searchEntry.BackgroundColor = Color.Gray;
            searchEntry.TextColor = Color.White;
            
            Children.Insert(0, searchEntry);
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

        protected override void LoadMore()
        {
            PodcastSearch.Limit += 10;
            PerformSearch();
        }

    }
}