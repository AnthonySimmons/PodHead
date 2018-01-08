using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Xamarin.Forms;
using PodHead;


using Button = Xamarin.Forms.Button;

namespace PodHead.Android.Views
{
    public delegate void ItemSelectedEventHandler(Item item);
    
    abstract class SubscriptionsView : StackLayout
    {
        protected ScrollView scrollView = new ScrollView();

        protected StackLayout stackLayout = new StackLayout();
        protected ItemsView itemsView;
        private Label _noResultsLabel;
        protected readonly Parser _parser;
        protected readonly Feeds _feeds;
        
        public event ItemSelectedEventHandler ItemSelected;
        private Button LoadMoreButton = new Button();
        
        protected ProgressBar progressBar = new ProgressBar();

        protected IList<Subscription> _subscriptions;

        protected Image RefreshImage = new Image();

        protected Dictionary<Subscription, List<View>> SubscriptionControls = new Dictionary<Subscription, List<View>>();

        protected int imageSize = 50;

		protected List<Image> ImageList = new List<Image> ();

        public SubscriptionsView()
        {
            _parser = Parser.Get(Config.Instance);
            _feeds = Feeds.Get(_parser, Config.Instance);
        }

        protected virtual void Initialize()
        {
            itemsView = new ItemsView(_feeds, _parser);
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
        

        public void LoadSubscriptionResults(IList<Subscription> subscriptions)
        {
            //if(subscriptions.Count >= Feeds.Instance.MaxItems - 1)
            {
                LoadMoreButton.IsVisible = true;
            }
            LoadSubscriptions(subscriptions);
        }

        protected abstract void AddControls();

        public void LoadSubscriptions(IList<Subscription> subscriptions)
        {
            //Initialize ();
            stackLayout.Children.Clear ();
            SubscriptionControls.Clear();
			ClearImages ();
			GC.Collect ();
            progressBar.IsVisible = false;
            
            _subscriptions = subscriptions;
            if(subscriptions.Count == 0)
            {
                _noResultsLabel = new Label();
                _noResultsLabel.Text = "No Results";
                stackLayout.Children.Add(_noResultsLabel);
            }

            foreach (var sub in subscriptions)
            {
                AddSubscription(sub);
            }
            scrollView.Content = stackLayout;
            stackLayout.Children.Add(LoadMoreButton);
            AddControls();
        }

		protected void ClearImages()
		{
            for (int i = 0; i < ImageList.Count; i++)
			{
                var image = ImageList[i];
                image.GestureRecognizers.Clear();
				image.Source = null;
                image = null;
			}
			ImageList.Clear ();
		}

        private void RemoveNoResultsLabel()
        {
            if (_noResultsLabel != null)
            {
                stackLayout.Children.Remove(_noResultsLabel);
            }
        }

        protected void AddSubscription(Subscription sub)
        {
            if (!SubscriptionControls.ContainsKey(sub))
            {
                RemoveNoResultsLabel();
                int boxHeight = 2;
                var topBoxView = new BoxView() { HeightRequest = boxHeight, BackgroundColor = Color.Black };
                
                var titleLabel = new Label
                {
                    Text = sub.Title,
                    TextColor = Color.Black,
                    FontAttributes = FontAttributes.Bold,
                    FontSize = 24,
                    HorizontalTextAlignment = TextAlignment.Center,
                };
                var descLabel = new Label { Text = sub.Description, TextColor = Color.Black, };

                var viewSubscriptionTap = new TapGestureRecognizer(ViewButton_Clicked);
                
                var image = new Image()
                {
                    Source = sub.ImageUrl,
                    WidthRequest = 100,
                    HeightRequest = 100,
                    BindingContext = sub,
                };
                image.GestureRecognizers.Add(viewSubscriptionTap);
                
                var nextImage = new Image { Source = "Next.png", };
                nextImage.BindingContext = sub;
                nextImage.HeightRequest = nextImage.WidthRequest = imageSize;
                nextImage.GestureRecognizers.Add(viewSubscriptionTap);

                var subscribeImage = new Image();
                SetSubscribedImage(sub, subscribeImage);
                subscribeImage.BindingContext = sub;
                subscribeImage.HeightRequest = subscribeImage.WidthRequest = imageSize;
                subscribeImage.GestureRecognizers.Add(new TapGestureRecognizer(SubscribeButton_Clicked));
                

                var imageLayout = new StackLayout { Orientation = StackOrientation.Horizontal };
                var buttonLayout = new StackLayout();
                buttonLayout.Children.Add(nextImage);
                buttonLayout.Children.Add(subscribeImage);
                imageLayout.Children.Add(image);
                imageLayout.Children.Add(buttonLayout);

				ImageList.Add (nextImage);
				ImageList.Add (subscribeImage);
				ImageList.Add (image);

                var subControls = new List<View> { topBoxView, titleLabel, imageLayout, descLabel };
                SubscriptionControls.Add(sub, subControls);

                foreach (var subControl in subControls)
                {
                    stackLayout.Children.Add(subControl);
                }
            }
        }

        private void ViewButton_Clicked(object sender)
        {
            var sub = ((View)sender).BindingContext as Subscription;
            if (sub != null)
            {
                RefreshImage.IsVisible = false;
                itemsView.IsVisible = true;
                //progressBar.IsVisible = true;
                scrollView.Content = itemsView;
                itemsView.LoadSubscription(sub);
            }
        }

        protected async void SubscribeButton_Clicked(object sender)
        {
            var senderImage = (Image)sender;
            var subscription = (Subscription)senderImage.BindingContext;

            if (!_feeds.ContainsSubscription(subscription.Title) ||
                await App.Current.MainPage.DisplayAlert("PodHead", string.Format("Unsubscribe from {0}?", subscription.Title), "Yes", "No"))
            {
                _feeds.ToggleSubscription(subscription);
                SetSubscribedImage(subscription, senderImage);
            }
        }
        

        private void SetSubscribedImage(Subscription subscription, Image image)
        {
            if (_feeds.ContainsSubscription(subscription.Title))
            {
                image.Source = "Unsubscribe.png";
            }
            else
            {
                image.Source = "Subscribe.png";
            }
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
            RefreshImage.IsVisible = true;
            scrollView.Content = stackLayout;
			LoadSubscriptions(_subscriptions);
        }

        private void ItemsView_PlayItem(Item item)
        {
            OnItemSelected(item);
        }
               

    }
}