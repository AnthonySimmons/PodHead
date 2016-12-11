using System;
using System.Collections.Generic;

using Android.Widget;

using Xamarin.Forms;
using PodHead;
using ProgressBar = Xamarin.Forms.ProgressBar;
using Button = Xamarin.Forms.Button;

namespace PodHead.Android.Views
{
    public delegate void PlayItemEventHandler(Item it);

    class ItemsView : StackLayout
    {
        public event PlayItemEventHandler PlayItem;
        public event EventHandler BackSelected;

        protected StackLayout stackLayout = new StackLayout();

        protected Xamarin.Forms.ScrollView scrollView = new Xamarin.Forms.ScrollView();

        private static int ImageSize = 175;

        //private TableView tableView = new TableView();
        protected Dictionary<Item, Dictionary<string, View>> ItemControls = new Dictionary<Item, Dictionary<string, View>>();

        private List<ProgressBar> progressBars = new List<ProgressBar>();
        private Button LoadMoreButton = new Button();
        private Image RefreshImage = new Image();
        private Image SubscribeImage = new Image();
        private Subscription _subscription;
        private Image BackImage = new Image();

        int buttonSize = 50;

        public ItemsView()
        {
            Initialize();
        }

        protected virtual void Initialize()
        {
            Parser.SubscriptionParsedComplete += Parser_SubscriptionParsedComplete;

            LoadMoreButton.Clicked += LoadMoreButton_Clicked;
            LoadMoreButton.Text = "Load More";
            LoadMoreButton.IsVisible = false;
            
            BackImage.Source = "Back.png";
            BackImage.WidthRequest = BackImage.HeightRequest = buttonSize;
            BackImage.GestureRecognizers.Add(new TapGestureRecognizer(sender => { BackButton_Clicked(sender, null); }));


            SubscribeImage.Source = "Subscribe.png";
            SubscribeImage.WidthRequest = SubscribeImage.HeightRequest = buttonSize;
            SubscribeImage.GestureRecognizers.Add(new TapGestureRecognizer(sender => { SubscribeButton_Clicked(sender, null); }));
            
            RefreshImage.Source = "Refresh.png";
            RefreshImage.WidthRequest = RefreshImage.HeightRequest = buttonSize;
            RefreshImage.GestureRecognizers.Add(new TapGestureRecognizer(sender => { RefreshButton_Clicked(sender, null); }));
        }

        private void Parser_SubscriptionParsedComplete(Subscription subscription)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                if (_subscription != null && subscription.Title == _subscription.Title)
                {
                    LoadSubscription(subscription);                    
                }
            }
            );
        }

        private void LoadMoreButton_Clicked(object sender, EventArgs e)
        {
            _subscription.MaxItems += 10;
            Parser.LoadSubscriptionAsync(_subscription);
        }

        public void LoadSubscription(Subscription subscription)
        {
            _subscription = subscription;
            if (!subscription.IsLoaded || !subscription.ItemsLoaded)
            {
                subscription.MaxItems = Math.Max(Subscription.DefaultMaxItems, subscription.MaxItems);
                Parser.LoadSubscription(subscription, subscription.MaxItems);
            }
            //if(subscription.Items.Count == subscription.MaxItems)
            {
                LoadMoreButton.IsVisible = true;
            }
            stackLayout.Children.Clear();
            Children.Clear();
            ItemControls.Clear();

            var topLayout = new StackLayout { Orientation = StackOrientation.Horizontal };
            topLayout.Children.Add(BackImage);
            topLayout.Children.Add(RefreshImage);
            topLayout.Children.Add(SubscribeImage);

            Children.Insert(0, topLayout);

            LoadSubscriptionTitle(subscription);
            LoadSubscriptionItems(subscription);

            scrollView.Content = stackLayout;
            Children.Add(scrollView);

        }

        private void LoadSubscriptionTitle(Subscription subscription)
        {
            var subscriptionTitle = new Label();
            subscriptionTitle.HorizontalTextAlignment = TextAlignment.Center;
            subscriptionTitle.FontSize = 20;
            subscriptionTitle.FontAttributes = FontAttributes.Bold;
            subscriptionTitle.TextColor = Color.Black;
            subscriptionTitle.Text = subscription.Title;

            var image = new Image();
            image.HeightRequest = ImageSize;
            image.WidthRequest = ImageSize;
            image.Source = subscription.ImageUrl;

            var description = new Label();
            description.TextColor = Color.Black;
            description.Text = subscription.Description;
            
            stackLayout.Children.Add(subscriptionTitle);
            stackLayout.Children.Add(image);
            stackLayout.Children.Add(description);
            
        }


        protected void LoadSubscriptionItems(Subscription subscription)
        {
            SetSubscriptionText(subscription);
            LoadItems(subscription.Items);
            stackLayout.Children.Add(LoadMoreButton);
        }

        protected void LoadItems(IEnumerable<Item> items)
        {
            int count = 0;
            foreach(var item in items)
            {
                InsertItem(item, count);
                count++;                
            }
        }

        protected void InsertItem(Item item, int index)
        {
            if (!ItemControls.ContainsKey(item))
            {
                var title = new Label()
                {
                    Text = item.Title,
                    TextColor = Color.Black,
                    FontSize = 18,
                    HorizontalTextAlignment = TextAlignment.Center
                };
                var description = new Label() { Text = item.Description, TextColor = Color.Black };
                
                var playImage = new Image() { Source = "Play.png", HeightRequest = buttonSize, WidthRequest = buttonSize, };
                var downloadImage = new Image() { Source = "Download.png", HeightRequest = buttonSize, WidthRequest = buttonSize };
                downloadImage.BindingContext = item;
                playImage.BindingContext = item;
                playImage.GestureRecognizers.Add(new TapGestureRecognizer(sender => { PlayButton_Clicked(sender, null); }));
                downloadImage.GestureRecognizers.Add(new TapGestureRecognizer(sender => { DownloadButton_Clicked(sender, null); }));

                var progressBar = new ProgressBar() { IsVisible = false, };

                if (item.IsDownloaded)
                {
                    downloadImage.Source = "Remove.png";
                }
                
                var hLayout = new StackLayout();
                hLayout.Orientation = StackOrientation.Horizontal;
                hLayout.Children.Add(playImage);
                hLayout.Children.Add(downloadImage);
            
                int boxHeight = 2;
                var topBoxView = new BoxView() { HeightRequest = boxHeight, BackgroundColor = Color.Black };
                var botBoxView = new BoxView() { HeightRequest = boxHeight, BackgroundColor = Color.Black };

                var itemLayout = new StackLayout();
                itemLayout.Children.Add(topBoxView);
                itemLayout.Children.Add(title);
                itemLayout.Children.Add(description);
                //itemLayout.Children.Add(tableView);
                itemLayout.Children.Add(hLayout);
                itemLayout.Children.Add(progressBar);

                stackLayout.Children.Add(itemLayout);//.Insert(index, itemLayout);

                ItemControls.Add(item, new Dictionary<string, View>()
                {
                    { "ItemLayout", itemLayout },
                    { "ProgressBar", progressBar},
                    { "DownloadButton", downloadImage},
                }
                );
            }
        }



        private void BackButton_Clicked(object sender, EventArgs e)
        {
            OnBackSelected();
        }

        private void RefreshButton_Clicked(object sender, EventArgs e)
        {
            Parser.LoadSubscriptionAsync(_subscription);
        }

        private void SubscribeButton_Clicked(object sender, EventArgs e)
        {
            var subscription = ((View)sender).BindingContext as Subscription;
            if (subscription != null)
            {
                Feeds.Instance.ToggleSubscription(subscription);
                SetSubscriptionText(subscription);
            }
        }

        private void SetSubscriptionText(Subscription subscription)
        {
            SubscribeImage.BindingContext = subscription;
            var subscribeText = Feeds.Instance.GetSubscribeText(subscription.Title);
            if(subscribeText == "Subscribe")
            {
                SubscribeImage.Source = "Subscribe.png";
            }
            else if(subscribeText == "Unsubscribe")
            {
                SubscribeImage.Source = "Unsubscribe.png";
            }
        }

        private void OnBackSelected()
        {
            var copy = BackSelected;
            if (copy != null)
            {
                copy.Invoke(null, null);
            }
        }

        private void DownloadButton_Clicked(object sender, EventArgs e)
        {
            var item = ((View)sender).BindingContext as Item;
            if (item != null)
            {
                if (!item.IsDownloaded)
                {
                    item.DownloadProgress += Item_DownloadProgress;
                    Item.AnyDownloadComplete += Item_DownloadComplete;
                    item.DownloadFile();
                }
                else
                {
                    Item.AnyDownloadRemoved += Item_AnyDownloadRemoved;
                    item.DeleteFile();
                }
            }
        }

        private void Item_AnyDownloadRemoved(Item item)
        {
            ItemDownloadRemoved(item);
        }

        protected void ItemDownloadRemoved(Item item)
        {
            Device.BeginInvokeOnMainThread(() => 
            {
                if (ItemControls.ContainsKey(item))
                {
                    var itemControls = ItemControls[item];
                    if (itemControls.ContainsKey("DownloadButton"))
                    {
                        var downloadButton = (Image)itemControls["DownloadButton"];
                        //downloadButton.Text = "Download";
                        downloadButton.Source = "Download.png";
                    }
                }
            }
            );
        }

        private void Item_DownloadComplete(Item item)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                var downloadButton = (Image)ItemControls[item]["DownloadButton"];
                //downloadButton.Text = "Remove";
                downloadButton.Source = "Remove.png";
                
                var progressBar = (ProgressBar)ItemControls[item]["ProgressBar"];
                progressBar.IsVisible = false;
            }
            );
        }

        private void Item_DownloadProgress(Item item, double percent)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                var progressBar = (ProgressBar)ItemControls[item]["ProgressBar"];
                progressBar.IsVisible = true;
                progressBar.Progress = percent / 100;
            }
            );
        }

        private void PlayButton_Clicked(object sender, EventArgs e)
        {
            var item = ((View)sender).BindingContext as Item;
            if (item != null)
            {
                OnPlayItem(item);
            }
        }

        private void OnPlayItem(Item it)
        {
            var copy = PlayItem;
            if (copy != null)
            {
                copy.Invoke(it);
            }
        }
    }
}