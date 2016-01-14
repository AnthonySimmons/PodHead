using System;
using System.Collections.Generic;

using Android.Widget;

using Xamarin.Forms;
using RSS;
using Button = Xamarin.Forms.Button;

namespace RssApp.Android.Views
{
    public delegate void PlayItemEventHandler(Item it);

    class ItemsView : StackLayout
    {
        public event PlayItemEventHandler PlayItem;
        public event EventHandler BackSelected;

        private static int RowSize = 175;

        private TableView tableView = new TableView();

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
            tableView.RowHeight = RowSize;

            tableView.Root = new TableRoot();
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

            BackButton.Text = "Back";
            BackButton.TextColor = Color.Black;
            BackButton.Clicked += BackButton_Clicked;

            SubscribeButton.Text = "Subscribe";
            SubscribeButton.TextColor = Color.Black;
            SubscribeButton.Clicked += SubscribeButton_Clicked;

            var topLayout = new StackLayout { Orientation = StackOrientation.Horizontal };
            topLayout.Children.Add(BackButton);
            topLayout.Children.Add(RefreshButton);
            topLayout.Children.Add(SubscribeButton);

            RefreshButton.Text = "Refresh";
            RefreshButton.Clicked += RefreshButton_Clicked;

            var layout = new StackLayout();
            layout.Children.Add(image);
            layout.Children.Add(topLayout);

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
            };
            
            
            tableView.Root.Add(titleSection);
        }

        private void BackButton_Clicked(object sender, EventArgs e)
        {
            OnBackSelected();
        }

        private void RefreshButton_Clicked(object sender, EventArgs e)
        {
            Parser.LoadSubscriptionAsync(_subscription, _subscription.MaxItems);
        }

        private void SubscribeButton_Clicked(object sender, EventArgs e)
        {
            var subscription = ((View)sender).BindingContext as Subscription;
            if (subscription != null)
            {
                Feeds.Instance.AddChannel(subscription);
            }
        }

        private void LoadSubscriptionItems(Subscription subscription)
        {
            SubscribeButton.TextColor = Color.Black;
            SubscribeButton.BindingContext = subscription;
            SubscribeButton.Text = "Subscribe";
            if(Feeds.Instance.ContainsSubscription(subscription.Title))
            {
                SubscribeButton.Text = "Unsubscribe";
            }

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

                var hLayout = new StackLayout();
                hLayout.Orientation = StackOrientation.Horizontal;
                hLayout.Children.Add(playButton);
                hLayout.Children.Add(downloadButton);

                layout.Children.Add(hLayout);
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


                tableView.Root.Add(tableSection);
                //Children.Add(layout);
                
            }
            Children.Add(tableView);
            Children.Add(LoadMoreButton);
        }
        
        private void OnBackSelected()
        {
            var copy = BackSelected;
            if(copy != null)
            {
                copy.Invoke(null, null);
            }
        }

        private void DownloadButton_Clicked(object sender, EventArgs e)
        {
            var item = ((Button)sender).BindingContext as Item;
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
            var item = ((View)sender).BindingContext as Item;
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