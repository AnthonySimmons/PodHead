using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using PodHead;

namespace PodHead.Android.Views
{
    class SavedSubscriptionsView : SubscriptionsView
    {
        double progressStep;


        public SavedSubscriptionsView()
        {
            Initialize();
        }

		protected override void Initialize()
        {
            Feeds.Instance.AllFeedsParsed += Instance_AllFeedsParsed;
            Feeds.Instance.FeedUpdated += Instance_FeedUpdated;
            Feeds.Instance.SubscriptionAdded += Instance_SubscriptionAdded;
            Feeds.Instance.SubscriptionRemoved += Instance_SubscriptionRemoved;

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
                    Parser.SubscriptionParsedComplete -= Parser_SubscriptionParsedComplete;
                    LoadSubscriptions(Feeds.Instance.Subscriptions);
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
            Parser.SubscriptionParsedComplete += Parser_SubscriptionParsedComplete;
            progressBar.IsVisible = true;
            progressBar.Progress = 0;
            progressStep = 1.0 / Feeds.Instance.Subscriptions.Count;
            Feeds.Instance.ParseAllFeedsAsync();
        }

    }
}