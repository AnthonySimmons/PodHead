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
        public string return_url { get; set; }
        public string return_name { get; set; }
        public string return_channel { get; set; }
        public int return_update { get; set; }
        public ADD_RSS_Subscription()
        {
            InitializeComponent();
        }
        
        //ADDS the URL and Subcrition name to the list of RSS feeds.(not done)
        private void ADD_button_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            return_url = url_entry.Text;
            return_channel = "General";
            if (numericUpDown1.Value == 0)
            {
                return_update = 24;
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
