using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using RSS;
using System.Diagnostics;

namespace RSSForm
{
    public partial class About : Form
    {
        public About()
        {
            InitializeComponent();
        }

        private void ok_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        
        private void linkLabelProjectHomeUrl_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(linkLabelProjectHomeUrl.Text);
        }
    }
}
