using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using RSS;

namespace RssApp.Android.Views
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

            RefreshButton.Text = "Refresh";
            RefreshButton.Clicked += RefreshButton_Clicked;

            Children.Insert(0, RefreshButton);
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
            progressBar.IsVisible = true;
            progressBar.Progress = 0;
            progressStep = 1.0 / Feeds.Instance.Subscriptions.Count;
            Feeds.Instance.ParseAllFeedsAsync();
        }

    }
}