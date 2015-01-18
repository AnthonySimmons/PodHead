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
    public partial class ADD_New_Channel : Form
    {
        public ADD_New_Channel()
        {
            InitializeComponent();
        }
        public string return_channel { get; set; }

        //button click to return the channel name to the previous form
        private void add_Click(object sender, EventArgs e)
        {
            return_channel = chanel_name.Text;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        //button click cancels the adding of a channel name
        private void cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        
    }
}
