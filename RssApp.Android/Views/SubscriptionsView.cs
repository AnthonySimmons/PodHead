using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Xamarin.Forms;
using RSS;


using Button = Xamarin.Forms.Button;

namespace RssApp.Android.Views
{
    public delegate void ItemSelectedEventHandler(Item item);
    
    class SubscriptionsView : ScrollView
    {
        protected StackLayout stackLayout = new StackLayout();
        private ItemsView itemsView = new ItemsView();
        

        public event ItemSelectedEventHandler ItemSelected;
        private Button LoadMoreButton = new Button();
        protected Button RefreshButton = new Button();
        
        protected ProgressBar progressBar = new ProgressBar();

        List<Subscription> _subscriptions;
        
        public SubscriptionsView()
        {
            Initialize();
        }

        private void Initialize()
        {
            itemsView.PlayItem += ItemsView_PlayItem;
            itemsView.BackSelected += ItemsView_BackSelected;

            Feeds.Instance.FeedUpdated += Instance_FeedUpdated;

            LoadMoreButton.Clicked += LoadMoreButton_Clicked;
            LoadMoreButton.Text = "Load More";
            LoadMoreButton.IsVisible = false;

            RefreshButton.Text = "Refresh";
            RefreshButton.Clicked += RefreshButton_Clicked;
            
            progressBar.IsVisible = false;
            progressBar.BackgroundColor = Color.Gray;
            
            stackLayout.Children.Add(RefreshButton);
            stackLayout.Children.Add(progressBar);

            Content = stackLayout;
        }

        private void Instance_FeedUpdated(double updatePercentage)
        {
            LoadSubscriptions(Feeds.Instance.Subscriptions);
        }

        private void RefreshButton_Clicked(object sender, EventArgs e)
        {
            Feeds.Instance.ParseAllFeedsAsync();
        }

        public void LoadSubscriptionResults(List<Subscription> subscriptions)
        {
            if(subscriptions.Count == Feeds.Instance.MaxItems)
            {
                LoadMoreButton.IsVisible = true;
            }
            LoadSubscriptions(subscriptions);
        }

        public void LoadSubscriptions(List<Subscription> subscriptions)
        {
            stackLayout.Children.Clear();
            _subscriptions = subscriptions;
            if(subscriptions.Count == 0)
            {
                var noResultsLabel = new Label();
                noResultsLabel.Text = "No Results";
                stackLayout.Children.Add(noResultsLabel);
            }

            foreach (var sub in subscriptions)
            {
                //Parser.LoadSubscription(sub, sub.MaxItems);
                int boxHeight = 2;
                var topBoxView = new BoxView() { HeightRequest = boxHeight, BackgroundColor = Color.Black };
                var botBoxView = new BoxView() { HeightRequest = boxHeight, BackgroundColor = Color.Black };
                var titleButton = new Button
                {
                    Text = sub.Title,
                    TextColor = Color.Black,
                };
                var descLabel = new Label { Text = sub.Description, TextColor = Color.Black, };
                
                titleButton.Clicked += SubscriptionChartTapped;
                titleButton.BindingContext = sub;

                var image = new Image()
                {
                    Source = sub.ImageUrl,
                    WidthRequest = 25,
                    HeightRequest = 25,
                };

                stackLayout.Children.Add(topBoxView);
                stackLayout.Children.Add(titleButton);
                stackLayout.Children.Add(image);
                stackLayout.Children.Add(descLabel);
                //stackLayout.Children.Add(botBoxView);
            }
                        
            Content = stackLayout;
        }

        private void LoadMoreButton_Clicked(object sender, EventArgs e)
        {
            Feeds.Instance.MaxItems += 10;
            Feeds.Instance.ParseAllFeedsAsync();
        }

        private void SubscriptionChartTapped(object sender, EventArgs e)
        {
            var sub = ((Button)sender).BindingContext as Subscription;
            if (sub != null)
            {
                itemsView.IsVisible = true;
                progressBar.IsVisible = true;
                Content = itemsView;
                itemsView.LoadSubscription(sub);
            }
        }
        
        private void OnItemSelected(Item item)
        {
            var copy = ItemSelected;
            if(copy != null)
            {
                copy.Invoke(item);
            }
        }

        private void ItemsView_BackSelected(object sender, EventArgs e)
        {
            Content = stackLayout;
        }

        private void ItemsView_PlayItem(Item item)
        {
            OnItemSelected(item);
        }

        

    }
}