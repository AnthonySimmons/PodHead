using System;
using System.Collections.Generic;

using Android.Widget;

using Xamarin.Forms;
using PodHead;
using ProgressBar = Xamarin.Forms.ProgressBar;
using Button = Xamarin.Forms.Button;
using System.Collections.Concurrent;

namespace PodHead.Android.Views
{
    public delegate void PlayItemEventHandler(Item it);

    class ItemsView : StackLayout
    {
        protected const string ItemLayout = "ItemLayout";
        protected const string DownloadButton = "DownloadButton";
        protected const string ProgressBar = "ProgressBar";
        protected const string PercentPlayedLabel = "PercentPlayedLabel";
        protected const string PlayButton = "PlayButton";

        public event PlayItemEventHandler PlayItem;
        public event EventHandler BackSelected;

        protected StackLayout stackLayout = new StackLayout();

        protected Xamarin.Forms.ScrollView scrollView = new Xamarin.Forms.ScrollView();

        private static int ImageSize = 175;

        //private TableView tableView = new TableView();
        protected ConcurrentDictionary<Item, Dictionary<string, View>> ItemControls = new ConcurrentDictionary<Item, Dictionary<string, View>>();

        private List<ProgressBar> progressBars = new List<ProgressBar>();
        private Button LoadMoreButton = new Button();
        private Image RefreshImage = new Image();
        private Image SubscribeImage = new Image();
        private Subscription _subscription;
        private Image BackImage = new Image();

        int buttonSize = 50;

        private readonly Parser _parser;
        private readonly Feeds _feeds;

        public ItemsView(Feeds feeds, Parser parser)
        {
            _feeds = feeds;
            _parser = parser;
            Initialize();
        }

        protected virtual void Initialize()
        {
            _parser.SubscriptionParsedComplete += Parser_SubscriptionParsedComplete;

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
            _parser.LoadSubscriptionAsync(_subscription);
        }

        public void LoadSubscription(Subscription subscription)
        {
            _subscription = subscription;
            if (!subscription.IsLoaded || !subscription.ItemsLoaded)
            {
                subscription.MaxItems = Math.Max(Subscription.DefaultMaxItems, subscription.MaxItems);
                _parser.LoadSubscription(subscription, subscription.MaxItems);
            }
            //if(subscription.Items.Count == subscription.MaxItems)
            {
                LoadMoreButton.IsVisible = true;
            }
            UnbindItemEvents();

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

        private void UnbindItemEvents()
        {
            foreach(Item item in ItemControls.Keys)
            {
                item.DownloadProgress -= Item_DownloadProgress;
                item.PercentPlayedChanged -= Item_PercentPlayedChanged;
            }
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
            UnbindItemEvents(subscription.Items);
            SetSubscriptionText(subscription);
            LoadItems(subscription.Items);
            stackLayout.Children.Add(LoadMoreButton);
        }

        protected virtual void UnbindItemEvents(IEnumerable<Item> items)
        {
            foreach(Item item in items)
            {
                item.IsPlayingChanged -= Item_IsPlayingChanged;
                item.DownloadProgress -= Item_DownloadProgress;
            }
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
                item.IsPlayingChanged += Item_IsPlayingChanged;

                var description = new Label() { Text = item.Description, TextColor = Color.Black };
                
                var playImage = new Image() { Source = "Play.png", HeightRequest = buttonSize, WidthRequest = buttonSize, };
                var downloadImageControl = new Image() { Source = "Download.png", HeightRequest = buttonSize, WidthRequest = buttonSize };
                downloadImageControl.BindingContext = item;
                playImage.BindingContext = item;
                playImage.GestureRecognizers.Add(new TapGestureRecognizer(sender => { PlayButton_Clicked(sender, null); }));
                downloadImageControl.GestureRecognizers.Add(new TapGestureRecognizer(sender => { DownloadButton_Clicked(sender, null); }));

                var progressBarControl = new ProgressBar() { IsVisible = false, };

                if (item.IsDownloaded)
                {
                    downloadImageControl.Source = "Remove.png";
                }

                var percentPlayedLabel = new Label
                {
                    FontSize = 16,
                    TextColor = Color.Blue,
                    HorizontalTextAlignment = TextAlignment.Center,
                    Text = ((int)item.PercentPlayed).ToString() + "%"
                };
                
                var hLayout = new StackLayout();
                hLayout.Orientation = StackOrientation.Horizontal;
                hLayout.Children.Add(playImage);
                hLayout.Children.Add(downloadImageControl);
                hLayout.Children.Add(percentPlayedLabel);
            
                int boxHeight = 2;
                var topBoxView = new BoxView() { HeightRequest = boxHeight, BackgroundColor = Color.Black };
                var botBoxView = new BoxView() { HeightRequest = boxHeight, BackgroundColor = Color.Black };

                var itemLayoutControl = new StackLayout();
                itemLayoutControl.Children.Add(topBoxView);
                itemLayoutControl.Children.Add(title);
                itemLayoutControl.Children.Add(description);
                //itemLayout.Children.Add(tableView);
                itemLayoutControl.Children.Add(hLayout);
                itemLayoutControl.Children.Add(progressBarControl);

                stackLayout.Children.Add(itemLayoutControl);//.Insert(index, itemLayout);

                ItemControls.TryAdd(item, new Dictionary<string, View>()
                {
                    { ItemLayout, itemLayoutControl },
                    { ProgressBar, progressBarControl},
                    { DownloadButton, downloadImageControl},
                    { PercentPlayedLabel, percentPlayedLabel },
                    { PlayButton, playImage },
                }
                );

                item.PercentPlayedChanged += Item_PercentPlayedChanged;

            }
        }

        private void Item_IsPlayingChanged(Item item)
        {
            UpdateIsPlaying(item);
        }

        protected void Item_PercentPlayedChanged(Item item)
        {
            UpdatePercentPlayed(item);
        }

        protected virtual void UpdateIsPlaying(Item item)
        {
            if (ItemControls.ContainsKey(item) && ItemControls[item].ContainsKey(PlayButton))
            {
                Image playImage = ItemControls[item][PlayButton] as Image;
                playImage.Source = item.IsPlaying ? "Pause.png" : "Play.png";
            }
        }

        protected virtual void UpdatePercentPlayed(Item item)
        {
            if(ItemControls.ContainsKey(item) && ItemControls[item].ContainsKey(PercentPlayedLabel))
            {
                Label percentPlayedLabel = ItemControls[item][PercentPlayedLabel] as Label;
                Device.BeginInvokeOnMainThread(() =>
                {
                    percentPlayedLabel.Text = ((int)item.PercentPlayed).ToString() + "%";
                });
            }
        }

        private void BackButton_Clicked(object sender, EventArgs e)
        {
            OnBackSelected();
        }

        private void RefreshButton_Clicked(object sender, EventArgs e)
        {
            _parser.LoadSubscriptionAsync(_subscription);
        }

        private void SubscribeButton_Clicked(object sender, EventArgs e)
        {
            var subscription = ((View)sender).BindingContext as Subscription;
            if (subscription != null)
            {
                _feeds.ToggleSubscription(subscription);
                SetSubscriptionText(subscription);
            }
        }

        private void SetSubscriptionText(Subscription subscription)
        {
            SubscribeImage.BindingContext = subscription;
            var subscribeText = _feeds.GetSubscribeText(subscription.Title);
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
                    ItemControls[item][DownloadButton].IsVisible = false;
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
                    if (itemControls.ContainsKey(DownloadButton))
                    {
                        var downloadButtonControl = (Image)itemControls[DownloadButton];
                        //downloadButton.Text = "Download";
                        downloadButtonControl.Source = "Download.png";
                    }
                }
            }
            );
        }

        private void Item_DownloadComplete(Item item)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                if (ItemControls.ContainsKey(item) &&
                    ItemControls[item].ContainsKey(DownloadButton) &&
                    ItemControls[item].ContainsKey(ProgressBar))
                {
                    var downloadButtonControl = (Image)ItemControls[item][DownloadButton];
                    //downloadButton.Text = "Remove";
                    downloadButtonControl.Source = "Remove.png";

                    var progressBarControl = (ProgressBar)ItemControls[item][ProgressBar];
                    progressBarControl.IsVisible = false;
                    ItemControls[item][DownloadButton].IsVisible = true;
                }
            }
            );
        }

        private void Item_DownloadProgress(Item item, double percent)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                if (ItemControls.ContainsKey(item) && ItemControls[item].ContainsKey(ProgressBar))
                {
                    var progressBarControl = (ProgressBar)ItemControls[item][ProgressBar];
                    progressBarControl.IsVisible = true;
                    progressBarControl.Progress = percent / 100;
                }
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

        protected virtual void OnPlayItem(Item it)
        {
            var copy = PlayItem;
            if (copy != null)
            {
                copy.Invoke(it);
            }
        }
    }
}