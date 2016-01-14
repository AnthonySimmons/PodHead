using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Xamarin.Forms;
using RSS;


using Button = Xamarin.Forms.Button;

namespace RssApp.Android.Views
{
    public delegate void SubscriptionSelectedEventHandler(Subscription subscription);
    
    class SubscriptionsView : StackLayout
    {
        protected TableView tableView = new TableView();

        public event SubscriptionSelectedEventHandler SubscriptionSelected;
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
            tableView.Root = new TableRoot();
            
            Feeds.Instance.FeedUpdated += Instance_FeedUpdated;

            LoadMoreButton.Clicked += LoadMoreButton_Clicked;
            LoadMoreButton.Text = "Load More";
            LoadMoreButton.IsVisible = false;

            RefreshButton.Text = "Refresh";
            RefreshButton.Clicked += RefreshButton_Clicked;
            
            progressBar.IsVisible = false;
            progressBar.BackgroundColor = Color.Gray;
            
            Children.Add(RefreshButton);
            Children.Add(tableView);
            Children.Add(progressBar);
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
            tableView.Root.Clear();
            _subscriptions = subscriptions;
            if(subscriptions.Count == 0)
            {
                var cell = new TextCell();
                cell.Text = "No Results";
                tableView.Root.Add(new TableSection() { cell });
            }

            foreach (var sub in subscriptions)
            {
                var titleCell = new TextCell { Text = sub.Title, TextColor = Color.Black, };
                var descCell = new TextCell { Text = sub.Description, TextColor = Color.Black, };
                
                titleCell.Tapped += SubscriptionChartTapped;
                titleCell.BindingContext = sub;
                descCell.Tapped += SubscriptionChartTapped;
                                
                var tableSection = new TableSection()
                {
                    titleCell,
                    descCell,
                };

                tableView.Root.Add(tableSection);
            }
                        
            var loadMoreSection = new TableSection()
            {
                new ViewCell { View = LoadMoreButton }
            };
            tableView.Root.Add(loadMoreSection);
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