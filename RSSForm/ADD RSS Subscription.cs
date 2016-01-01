using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using RSS;

namespace RSSForm
{
    public partial class ADD_RSS_Subscription : Form
    {
        public Subscription NewSubscription { get; private set; }


        public ADD_RSS_Subscription()
        {
            InitializeComponent();
            var icon_bmp = new Bitmap(Properties.Resources.AddIcon);
            Icon = Icon.FromHandle(icon_bmp.GetHicon());
        }
        
        //ADDS the URL and Subcrition name to the list of RSS feeds.(not done)
        private void ADD_button_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            NewSubscription = new Subscription();
            NewSubscription.RssLink = url_entry.Text;
            NewSubscription.Category = textBoxCategory.Text;
            
            NewSubscription.update = (int)numericUpDown1.Value;
            
            if (numericUpDown1.Value == 0)
            {
                NewSubscription.update = 24;
            }

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
