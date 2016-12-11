using System;
using Xamarin.Forms;

namespace PodHead.Android.Views
{
    class SavedSubscriptionsView : SubscriptionsView
    {
        double progressStep;
        
        public SavedSubscriptionsView()
            : base()
        {
            Initialize();
        }

		protected override void Initialize()
        {
            _feeds.AllFeedsParsed += Instance_AllFeedsParsed;
            _feeds.FeedUpdated += Instance_FeedUpdated;
            _feeds.SubscriptionAdded += Instance_SubscriptionAdded;
            _feeds.SubscriptionRemoved += Instance_SubscriptionRemoved;

            //Parser.SubscriptionParsedComplete += Parser_SubscriptionParsedComplete;
			base.Initialize();

            RefreshImage.Source = "Refresh.png";
            RefreshImage.WidthRequest = RefreshImage.HeightRequest = imageSize;
            RefreshImage.GestureRecognizers.Add(new TapGestureRecognizer(sender => { RefreshButton_Clicked(sender, null); }));

            Children.Insert(0, RefreshImage);
        }

        private void Instance_SubscriptionRemoved(Subscription subscription)
        {
            RemoveSubscriptionControl(subscription);
        }

        private void RemoveSubscriptionControl(Subscription subscription)
        {
            if (SubscriptionControls.ContainsKey(subscription))
            {
                foreach(var view in SubscriptionControls[subscription])
                {
                    stackLayout.Children.Remove(view);
                }
                SubscriptionControls.Remove(subscription); 
            }
        }

        private void Instance_SubscriptionAdded(Subscription subscription)
        {
            AddSubscription(subscription);
        }

        protected override void LoadMore()
        {

        }

        private void Instance_FeedUpdated(double updatePercentage)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                progressBar.IsVisible = true;
                progressBar.Progress = updatePercentage;
            }
            );
        }

        private void Instance_AllFeedsParsed(object sender, EventArgs e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                if (scrollView.Content != itemsView)
                {
                    progressBar.IsVisible = false;
                    _parser.SubscriptionParsedComplete -= Parser_SubscriptionParsedComplete;
                    LoadSubscriptions(_feeds.Subscriptions);
                }
            }
            );
        }


        private void Parser_SubscriptionParsedComplete(Subscription subscription)
        {
            Device.BeginInvokeOnMainThread(() =>
               {
                   if (progressBar.IsVisible)
                   {
                       progressBar.Progress += progressStep;
                   }
               }
            );
        }


        private void RefreshButton_Clicked(object sender, EventArgs e)
        {
            _parser.SubscriptionParsedComplete += Parser_SubscriptionParsedComplete;
            progressBar.IsVisible = true;
            progressBar.Progress = 0;
            progressStep = 1.0 / _feeds.Subscriptions.Count;
            _feeds.ParseAllFeedsAsync();
        }

    }
}