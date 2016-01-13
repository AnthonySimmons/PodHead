using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using Xamarin.Forms;
using RSS;

using Button = Xamarin.Forms.Button;

namespace RssApp.Android.Views
{
    public delegate void SubscriptionSelectedEventHandler(Subscription subscription);
    
    class SubscriptionsView : TableView
    {
        public event SubscriptionSelectedEventHandler SubscriptionSelected;
        private Button LoadMoreButton = new Button();
        private Button RefreshButton = new Button();
        List<Subscription> _subscriptions;

        public SubscriptionsView()
        {
            Root = new TableRoot();
            Initialize();
        }

        private void Initialize()
        {
            Feeds.Instance.FeedUpdated += Instance_FeedUpdated;

            LoadMoreButton.Clicked += LoadMoreButton_Clicked;
            LoadMoreButton.Text = "Load More";
            LoadMoreButton.IsVisible = false;

            RefreshButton.Text = "Refresh";
            RefreshButton.Clicked += RefreshButton_Clicked;
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
            Root.Clear();
            _subscriptions = subscriptions;
            if(subscriptions.Count == 0)
            {
                var cell = new TextCell();
                cell.Text = "No Results";
                Root.Add(new TableSection() { cell });
            }

            foreach (var sub in subscriptions)
            {
                var titleCell = new TextCell { Text = sub.Title, TextColor = Color.Black, };
                var descCell = new TextCell { Text = sub.Description, TextColor = Color.Black, };
                //var imageCell =
                // new ImageCell { ImageSource = sub.ImageUrl, Text = sub.ImageUrl, TextColor = Color.Black, };

                titleCell.Tapped += SubscriptionChartTapped;
                titleCell.BindingContext = sub;
                descCell.Tapped += SubscriptionChartTapped;
                //imageCell.Tapped += SubscriptionChartTapped;

                var subscribeButton = new Xamarin.Forms.Button();
                
                var stackLayout = new StackLayout();
                //stackLayout.Children.Add(stackLayout);
                //stackLayout.

                var viewCell = new ViewCell()
                {
                    View = stackLayout,
                };
                
                var tableSection = new TableSection()
                {
                    //viewCell,
                    titleCell,
                    //imageCell,       

                };
                
                Root.Add(tableSection);
            }
                        
            var loadMoreSection = new TableSection()
            {
                new ViewCell { View = LoadMoreButton }
            };
            Root.Add(loadMoreSection);
        }

        private void LoadMoreButton_Clicked(object sender, EventArgs e)
        {
            Feeds.Instance.MaxItems += 10;
            Feeds.Instance.ParseAllFeedsAsync();
        }

        private void SubscriptionChartTapped(object sender, EventArgs e)
        {
            var sub = ((Cell)sender).BindingContext as Subscription;
            if (sub != null)
            {
                OnSubscriptionSelected(sub);
            }
        }

        private void OnSubscriptionSelected(Subscription subscription)
        {
            var copy = SubscriptionSelected;
            if(copy != null)
            {
                copy.Invoke(subscription);
            }
        }
    }
}