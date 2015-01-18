using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RSS
{
    public partial class RSS_Summary : Form
    {
        //constructor loads the summary forum.
        public RSS_Summary(string feed_title, Item feed)
        {
            InitializeComponent();
            title.Text = feed_title;
            Date.Text = feed.pubDateI;
            link.Text = feed.linkI;
            description.Text = feed.descriptionI;
        }

        //click the link and it hids the summary form and opens the web browser
        private void link_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Web_Browser web = new Web_Browser(link.Text);
            this.Hide();
            DialogResult dr = web.ShowDialog();
            this.Close();
        }        
    }
}
