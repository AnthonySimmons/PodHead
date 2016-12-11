using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PodHead;

namespace PodHeadForms
{
    public partial class NewSubscriptionForm : Form
    {
        public Subscription NewSubscription { get; private set; }


        public NewSubscriptionForm()
        {
            InitializeComponent();
            var icon_bmp = new Bitmap(Properties.Resources.Subscribe);
            Icon = Icon.FromHandle(icon_bmp.GetHicon());
        }
        
        //ADDS the URL and Subcrition name to the list of RSS feeds.(not done)
        private void ADD_button_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            NewSubscription = new Subscription();
            NewSubscription.RssLink = url_entry.Text;
           
            this.Close();
        }

        //Cancel ADDing the RSS feed to the list of RS feeds and closes the forum.
        private void cancel_button_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }        
    }
}
