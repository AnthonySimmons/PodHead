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
    public partial class ERROR : Form
    {
        public ERROR(string the_error)
        {
            InitializeComponent();
            label1.Text = the_error;
        }

        private void close_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
