using System;
using System.Collections.Generic;

using Android.Widget;

using Xamarin.Forms;
using RSS;
using ProgressBar = Xamarin.Forms.ProgressBar;
using Button = Xamarin.Forms.Button;

namespace RssApp.Android.Views
{
    public delegate void PlayItemEventHandler(Item it);

    class ItemsView : Xamarin.Forms.StackLayout
    {
        public event PlayItemEventHandler PlayItem;
        public event EventHandler BackSelected;

        private StackLayout stackLayout = new StackLayout();

        protected Xamarin.Forms.ScrollView scrollView = new Xamarin.Forms.ScrollView();

        private static int ImageSize = 175;

        //private TableView tableView = new TableView();
        private Dictionary<Item, Dictionary<string, View>> ItemControls = new Dictionary<Item, Dictionary<string, View>>();

        private List<Xamarin.Forms.ProgressBar> progressBars = new List<Xamarin.Forms.ProgressBar>();
        private Button LoadMoreButton = new Button();
        private Button RefreshButton = new Button();
        private Button SubscribeButton = new Button();
        private Subscription _subscription;
        private Button BackButton = new Button();

        public ItemsView()
        {
            Initialize();
        }

        private void Initialize()
        {
            Parser.SubscriptionParsedComplete += Parser_SubscriptionParsedComplete;

            LoadMoreButton.Clicked += LoadMoreButton_Clicked;
            LoadMoreButton.Text = "Load More";
            LoadMoreButton.IsVisible = false;
            
            BackButton.Text = "Back";
            BackButton.TextColor = Color.Black;
            BackButton.Clicked += BackButton_Clicked;

            SubscribeButton.Text = "Subscribe";
            SubscribeButton.TextColor = Color.Black;
            SubscribeButton.Clicked += SubscribeButton_Clicked;

            RefreshButton.Text = "Refresh";
            RefreshButton.Clicked += RefreshButton_Clicked;

            scrollView.Content = stackLayout;
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
            if (!subscription.IsLoaded)
            {
                Parser.LoadSubscription(subscription, Feeds.Instance.MaxItems);
            }
            //if(subscription.Items.Count == subscription.MaxItems)
            {
                LoadMoreButton.IsVisible = true;
            }
            stackLayout.Children.Clear();
            Children.Clear();
                        

            var topLayout = new StackLayout { Orientation = StackOrientation.Horizontal };
            topLayout.Children.Add(BackButton);
            topLayout.Children.Add(RefreshButton);
            topLayout.Children.Add(SubscribeButton);

            Children.Insert(0, topLayout);

            LoadSubscriptionTitle(subscription);
            LoadSubscriptionItems(subscription);
            scrollView.Content = stackLayout;
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
            
            scrollView.Content = stackLayout;

            Children.Add(scrollView);            
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
            SubscribeButton.TextColor = Color.Black;
            SubscribeButton.BindingContext = subscription;
            SubscribeButton.Text = Feeds.Instance.GetSubscribeText(subscription.Title);
        }

        private void LoadSubscriptionItems(Subscription subscription)
        {
            SetSubscriptionText(subscription);

            foreach (var item in subscription.Items)
            {
                var title = new Label()
                {
                    Text = item.Title,
                    TextColor = Color.Black,
                    FontSize = 18,
                    HorizontalTextAlignment = TextAlignment.Center
                };
                var description = new Label() { Text = item.Description, TextColor = Color.Black };
                var playButton = new Button() { Text = "Play", TextColor = Color.Black, };
                var downloadButton = new Button() { Text = "Download", TextColor = Color.Black, };
                var progressBar = new Xamarin.Forms.ProgressBar() { IsVisible = false, };
                if (item.CheckIsDownloaded())
                {
                    downloadButton.Text = "Remove";
                }
                downloadButton.BindingContext = item;

                playButton.BindingContext = item;
                playButton.Clicked += PlayButton_Clicked;
                downloadButton.Clicked += DownloadButton_Clicked;

                int height = (int)(title.Height + description.Height + playButton.Height + downloadButton.Height + progressBar.Height + 45);
                                
                var hLayout = new StackLayout();
                hLayout.Orientation = StackOrientation.Horizontal;
                hLayout.Children.Add(playButton);
                hLayout.Children.Add(downloadButton);

                int boxHeight = 2;
                var topBoxView = new BoxView() { HeightRequest = boxHeight, BackgroundColor = Color.Black };
                var botBoxView = new BoxView() { HeightRequest = boxHeight, BackgroundColor = Color.Black };

                stackLayout.Children.Add(topBoxView);
                stackLayout.Children.Add(title);
                stackLayout.Children.Add(description);
                stackLayout.Children.Add(hLayout);
                stackLayout.Children.Add(progressBar);

                ItemControls.Add(item, new Dictionary<string, View>()
                {
                    { "ProgressBar", progressBar},
                    { "DownloadButton", downloadButton},
                }
                );
                
            }

            stackLayout.Children.Add(LoadMoreButton);
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
            var item = ((Button)sender).BindingContext as Item;
            if (item != null)
            {
                if (!item.CheckIsDownloaded())
                {
                    item.DownloadProgress += Item_DownloadProgress;
                    item.DownloadComplete += Item_DownloadComplete;
                    item.CalculateFilePath();
                    item.DownloadFile();
                }
                else
                {
                    item.DeleteFile();
                }
            }
        }

        private void Item_DownloadComplete(Item item)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                var downloadButton = (Button)ItemControls[item]["DownloadButton"];
                downloadButton.Text = "Remove";
                
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