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
    public partial class Web_Browser : Form
    {
        public Web_Browser(string url_to_open)
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            webBrowser.Navigate(url_to_open);
            
        }

        //Menu Strip File -> Exit: closes window
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
