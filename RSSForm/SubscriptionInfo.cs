using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using RSS;

namespace RSSForm
{
    public partial class SubscriptionInfo : Form
    {
        private Subscription _subscription;

        private Item _item;

        private SubscriptionInfo()
        {
            InitializeComponent();

            Bitmap ico = new Bitmap(Properties.Resources.Icon);
            Icon = Icon.FromHandle(ico.GetHicon());
        }

        public SubscriptionInfo(Subscription sub) 
            : this()
        {            
            _subscription = sub;
            LoadSubscription();
        }

        public SubscriptionInfo(Subscription sub, Item it)
            : this()
        {
            _subscription = sub;
            _item = it;
            LoadItem();
        }

        private void LoadSubscription()
        {
            if (_subscription != null)
            {
                if(!_subscription.IsLoaded)
                {
                    Parser.LoadSubscription(_subscription, Feeds.Instance.MaxItems);
                }

                textBoxTitle.Text = _subscription.Title;
                linkLabelFeed.Text = _subscription.RssLink;
                linkLabelSite.Text = _subscription.SiteLink;
                textBoxDate.Text = _subscription.PubDate;
                textBoxDescription.Text = _subscription.Description;
                if (!string.IsNullOrEmpty(_subscription.ImageUrl))
                {
                    pictureBox1.Load(_subscription.ImageUrl);
                }
            }
        }

        private void LoadItem()
        {
            if(_item != null)
            {
                textBoxTitle.Text = _item.Title;
                linkLabelSite.Text = _item.Link;
                linkLabelFeed.Text = _subscription.RssLink;
                textBoxDescription.Text = _item.Description;
                textBoxDate.Text = _item.PubDate;

                if (!string.IsNullOrEmpty(_subscription.ImageUrl))
                {
                    pictureBox1.Load(_subscription.ImageUrl);
                }
            }
        }

        private void ShowLink(string url)
        {
            var processInfo = new ProcessStartInfo(url);
            Process.Start(processInfo);
        }

        private void linkLabelFeed_Click(object sender, EventArgs e)
        {
            ShowLink(linkLabelFeed.Text);
        }

        private void linkLabelSite_Click(object sender, EventArgs e)
        {
            ShowLink(linkLabelSite.Text);
        }
    }
}
