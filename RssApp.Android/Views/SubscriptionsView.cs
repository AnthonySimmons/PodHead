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
    
    abstract class SubscriptionsView : StackLayout
    {
        protected ScrollView scrollView = new ScrollView();

        protected StackLayout stackLayout = new StackLayout();
        protected ItemsView itemsView = new ItemsView();
        
        
        public event ItemSelectedEventHandler ItemSelected;
        private Button LoadMoreButton = new Button();
        
        protected ProgressBar progressBar = new ProgressBar();

        protected List<Subscription> _subscriptions;

        protected Button RefreshButton = new Button();

        protected Dictionary<Subscription, List<View>> SubscriptionControls = new Dictionary<Subscription, List<View>>();

        protected virtual void Initialize()
        {            
            itemsView.PlayItem += ItemsView_PlayItem;
            itemsView.BackSelected += ItemsView_BackSelected;
                 
            LoadMoreButton.Clicked += LoadMoreButton_Clicked;
            LoadMoreButton.Text = "Load More";
            LoadMoreButton.IsVisible = false;
                        
            progressBar.IsVisible = false;
            progressBar.BackgroundColor = Color.Gray;
            
            scrollView.Content = stackLayout;
            
            Children.Insert(0, progressBar);
            Children.Add(scrollView);
        }
        

        public void LoadSubscriptionResults(List<Subscription> subscriptions)
        {
            //if(subscriptions.Count >= Feeds.Instance.MaxItems - 1)
            {
                LoadMoreButton.IsVisible = true;
            }
            LoadSubscriptions(subscriptions);
        }

        public void LoadSubscriptions(List<Subscription> subscriptions)
        {
            //Initialize ();
            scrollView.Content = stackLayout;
            stackLayout.Children.Clear ();
            SubscriptionControls.Clear();
            progressBar.IsVisible = false;
            
            _subscriptions = subscriptions;
            if(subscriptions.Count == 0)
            {
                var noResultsLabel = new Label();
                noResultsLabel.Text = "No Results";
                stackLayout.Children.Add(noResultsLabel);
            }

            foreach (var sub in subscriptions)
            {
                AddSubscription(sub);
            }
            stackLayout.Children.Add(LoadMoreButton);
        }

        protected void AddSubscription(Subscription sub)
        {
            if (!SubscriptionControls.ContainsKey(sub))
            {
                int boxHeight = 2;
                var topBoxView = new BoxView() { HeightRequest = boxHeight, BackgroundColor = Color.Black };
                var botBoxView = new BoxView() { HeightRequest = boxHeight, BackgroundColor = Color.Black };
                var titleLabel = new Label
                {
                    Text = sub.Title,
                    TextColor = Color.Black,
                    FontAttributes = FontAttributes.Bold,
                    FontSize = 24,
                    HorizontalTextAlignment = TextAlignment.Center,
                };
                var descLabel = new Label { Text = sub.Description, TextColor = Color.Black, };
                                
                var image = new Image()
                {
                    Source = sub.ImageUrl,
                    WidthRequest = 100,
                    HeightRequest = 100,
                };
                if (string.IsNullOrEmpty(sub.ImageUrl))
                {
                    image.Source = "@drawable/icon";
                }
                var viewButton = new Button { Text = "View", TextColor = Color.Black, };
                viewButton.BindingContext = sub;
                viewButton.Clicked += ViewButton_Clicked;
                var subscribeButton = new Button { Text = Feeds.Instance.GetSubscribeText(sub.Title), TextColor = Color.Black, };
                subscribeButton.BindingContext = sub;
                subscribeButton.Clicked += SubscribeButton_Clicked;

                var imageLayout = new StackLayout { Orientation = StackOrientation.Horizontal };
                var buttonLayout = new StackLayout();
                buttonLayout.Children.Add(viewButton);
                buttonLayout.Children.Add(subscribeButton);
                imageLayout.Children.Add(image);
                imageLayout.Children.Add(buttonLayout);

                var subControls = new List<View> { topBoxView, titleLabel, imageLayout, descLabel };
                SubscriptionControls.Add(sub, subControls);

                foreach (var subControl in subControls)
                {
                    stackLayout.Children.Add(subControl);
                }
            }
        }

        private void ViewButton_Clicked(object sender, EventArgs e)
        {
            var sub = ((View)sender).BindingContext as Subscription;
            if (sub != null)
            {
                RefreshButton.IsVisible = false;
                itemsView.IsVisible = true;
                //progressBar.IsVisible = true;
                scrollView.Content = itemsView;
                itemsView.LoadSubscription(sub);
            }
        }

        private void SubscribeButton_Clicked(object sender, EventArgs e)
        {
            var senderButton = (Button)sender;
            var subscription = (Subscription)senderButton.BindingContext;
            Feeds.Instance.ToggleSubscription(subscription);
            senderButton.Text = Feeds.Instance.GetSubscribeText(subscription.Title);
        }
        

        protected abstract void LoadMore();

        private void LoadMoreButton_Clicked(object sender, EventArgs e)
        {
            LoadMore();
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
            RefreshButton.IsVisible = true;
            scrollView.Content = stackLayout;
			LoadSubscriptions(_subscriptions);
        }

        private void ItemsView_PlayItem(Item item)
        {
            OnItemSelected(item);
        }
               

    }
}