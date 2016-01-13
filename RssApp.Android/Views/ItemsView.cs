using System;
using System.Collections.Generic;

using Android.Widget;

using Xamarin.Forms;
using RSS;
using Button = Xamarin.Forms.Button;

namespace RssApp.Android.Views
{
    public delegate void PlayItemEventHandler(Item it);

    class ItemsView : TableView
    {
        public event PlayItemEventHandler PlayItem;

        private static int RowSize = 175;

        private List<Xamarin.Forms.ProgressBar> progressBars = new List<Xamarin.Forms.ProgressBar>();
        private Button LoadMoreButton = new Button();
        private Button RefreshButton = new Button();
        private Subscription _subscription;

        public ItemsView()
        {
            Initialize();
        }

        private void Initialize()
        {
            LoadMoreButton.Clicked += LoadMoreButton_Clicked;
            LoadMoreButton.Text = "Load More";
            LoadMoreButton.IsVisible = false;

            Parser.SubscriptionParsedComplete += Parser_SubscriptionParsedComplete;
        }

        private void Parser_SubscriptionParsedComplete(Subscription subscription)
        {
            Device.BeginInvokeOnMainThread(() =>
            LoadSubscription(subscription));
        }

        private void LoadMoreButton_Clicked(object sender, EventArgs e)
        {
            _subscription.MaxItems += 10;
            Parser.LoadSubscriptionAsync(_subscription, _subscription.MaxItems);
        }

        public void LoadSubscription(Subscription subscription)
        {
            _subscription = subscription;
            if(!subscription.IsLoaded)
            {
                Parser.LoadSubscription(subscription, Feeds.Instance.MaxItems);
            }
            //if(subscription.Items.Count == subscription.MaxItems)
            {
                LoadMoreButton.IsVisible = true;
            }

            //this.HasUnevenRows = true;
            this.RowHeight = RowSize;
            Root = new TableRoot();
            LoadSubscriptionTitle(subscription);
            LoadSubscriptionItems(subscription);
        }

        private void LoadSubscriptionTitle(Subscription subscription)
        {
            var subscriptionTitle = new TextCell();
            subscriptionTitle.TextColor = Color.Black;
            subscriptionTitle.Text = subscription.Title;
            
            
            var image = new Image();
            image.HeightRequest = RowSize;
            image.WidthRequest = RowSize;
            image.Source = subscription.ImageUrl;

            var description = new TextCell();
            description.TextColor = Color.Black;
            description.Text = subscription.Description;

            var subscribeCell = new TextCell();
            subscribeCell.BindingContext = subscription;
            subscribeCell.TextColor = Color.Black;
            if (!Feeds.Instance.ContainsSubscription(subscription.Title))
            {
                subscribeCell.Text = "Subscribe";
            }
            else
            {
                subscribeCell.Text = "Unsubscribe";
            }

            subscribeCell.Tapped += SubscribeButton_Tapped;

            RefreshButton.Text = "Refresh";
            RefreshButton.Clicked += RefreshButton_Clicked;

            var layout = new StackLayout();
            layout.Children.Add(image);
            layout.Children.Add(RefreshButton);

            layout.HeightRequest = RowSize;
            var viewLayout = new ViewCell()
            {
                View = layout,
                Height = RowSize,
            };
            

            var titleSection = new TableSection()
            {
                viewLayout,
                subscriptionTitle,
                description,
                subscribeCell,
            };

            Root.Add(titleSection);
        }

        private void RefreshButton_Clicked(object sender, EventArgs e)
        {
            Parser.LoadSubscriptionAsync(_subscription, _subscription.MaxItems);
        }

        private void SubscribeButton_Tapped(object sender, EventArgs e)
        {
            var subscription = ((Cell)sender).BindingContext as Subscription;
            if (subscription != null)
            {
                Feeds.Instance.AddChannel(subscription);
            }
        }

        private void LoadSubscriptionItems(Subscription subscription)
        {

            foreach (var item in subscription.Items)
            {
                var title = new Label() { Text = item.Title, TextColor = Color.Black };
                var description = new Label() { Text = item.Description, TextColor = Color.Black };
                var playButton = new Button() { Text = "Play", TextColor = Color.Black, };
                var downloadButton = new Button() { Text = "Download", TextColor = Color.Black, };
                var progressBar = new Xamarin.Forms.ProgressBar() { IsVisible = false, };
                if(item.CheckIsDownloaded())
                {
                    downloadButton.Text = "Remove";
                }
                downloadButton.BindingContext = item;

                playButton.BindingContext = item;
                playButton.Clicked += PlayButton_Clicked;
                downloadButton.Clicked += DownloadButton_Clicked;

                int height = (int)(title.Height + description.Height + playButton.Height + downloadButton.Height + progressBar.Height + 45);
                var layout = new StackLayout();
                layout.HeightRequest = height;
                layout.Children.Add(title);
                layout.Children.Add(description);
                layout.Children.Add(playButton);
                layout.Children.Add(downloadButton);
                layout.Children.Add(progressBar);

                progressBars.Add(progressBar);

                var viewCell = new ViewCell()
                {
                    View = layout,
                    Height = height,
                };
                
                var tableSection = new TableSection()
                {
                    viewCell,
                };

                
                Root.Add(tableSection);
            }

            var loadMoreSection = new TableSection()
            {
                new ViewCell { View = LoadMoreButton, Height = 50, }
            };
            Root.Add(loadMoreSection);
        }
        

        private void DownloadButton_Clicked(object sender, EventArgs e)
        {
            var item = ((Xamarin.Forms.Button)sender).BindingContext as Item;
            if(item != null)
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

        private void Item_DownloadComplete(int row, int size)
        {
            
        }

        private void Item_DownloadProgress(string MbString, float percent, int row)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                /*var viewCell = Root[row][0] as ViewCell;
                if (viewCell != null)
                {
                    var layout = viewCell.View as StackLayout;
                    if (layout != null)
                    {
                        var progressBar = layout.Children[3] as Xamarin.Forms.ProgressBar;
                        if (progressBar != null)
                        {
                            progressBar.IsVisible = percent >= 1.0 ? false : true;
                            progressBar.Progress = percent;
                        }
                    }
                }*/
                //var progressBar = progressBars[row - 1];
                //progressBar.IsVisible = percent >= 1.0 ? false : true;
                //progressBar.Progress = percent;


            }
            );
        }

        private void PlayButton_Clicked(object sender, EventArgs e)
        {
            var item = ((Xamarin.Forms.View)sender).BindingContext as Item;
            if (item != null)
            {
                OnPlayItem(item);
            }
        }

        private void OnPlayItem(Item it)
        {
            var copy = PlayItem;
            if(copy != null)
            {
                copy.Invoke(it);
            }
        }
    }
}