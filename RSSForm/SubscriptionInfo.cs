using System;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RSS
{
    public partial class SubscriptionInfo : Form
    {
        private Channel _channel;
        public SubscriptionInfo(Channel ch)
        {
            InitializeComponent();
            _channel = ch;
            InitializeChannel();
        }

        private void InitializeChannel()
        {
            if (_channel != null)
            {
                textBoxTitle.Text = _channel.title;
                linkLabelFeed.Text = _channel.RssLink;
                linkLabelSite.Text = _channel.SiteLink;
                textBoxDate.Text = _channel.pubDate;
                textBoxDescription.Text = _channel.description;
                pictureBox1.Load(_channel.ImageUrl);
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
