using PodHead;
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

        private readonly PodcastSearch _podcastSearch;
        private readonly IConfig _config;

        public SearchView() 
            : base()
        {
            _config = Config.Instance;
            _podcastSearch = PodcastSearch.Get(_config, _parser);
            Initialize();
        }
        
		protected override void Initialize()
        {
			base.Initialize ();
            
            _podcastSearch.SearchResultReceived += Instance_SearchResultReceived;
            
            searchEntry.SearchButtonPressed += SearchEntry_SearchButtonPressed;
            searchEntry.BackgroundColor = Color.Gray;
            searchEntry.TextColor = Color.White;

            AddControls();
        }

        protected override void AddControls()
        {
            stackLayout.Children.Insert(0, searchEntry);
        }


        private void Instance_SearchResultReceived(List<Subscription> subscriptions)
        {
            Device.BeginInvokeOnMainThread(() => LoadSearchResults(subscriptions));
        }

        private void SearchEntry_SearchButtonPressed(object sender, EventArgs e)
        {
            _podcastSearch.SearchAsync(searchEntry.Text);
        }


        private void LoadSearchResults(List<Subscription> subscriptions)
        {
            LoadSubscriptionResults(subscriptions);
        }
        
        private void PerformSearch()
        {
            _podcastSearch.Results.Clear();
            _podcastSearch.SearchAsync(searchEntry.Text);
        }

        protected override void LoadMore()
        {
            PodcastSearch.Limit += 10;
            PerformSearch();
        }

    }
}