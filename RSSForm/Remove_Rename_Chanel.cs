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
    public partial class Remove_Rename_Chanel : Form
    {
        public string return_oldname { get; set; }
        public string return_newname { get; set; }
        public string return_to_remove { get; set; }
        public Remove_Rename_Chanel(List<string>the_channels)
        {
            InitializeComponent();
            Channels.DataSource = the_channels;           
        }

        //event changes the option textbox to the selected item in the channels list.
        private void Channels_SelectedIndexChanged(object sender, EventArgs e)
        {
            option.Text = Channels.SelectedItem.ToString();  
        }

        //rename button click
        private void rename_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            return_oldname = Channels.SelectedItem.ToString();
            return_newname = option.Text;
            this.Close();
        }

        //remove button click
        private void remove_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Abort;
            return_to_remove = option.Text;
            this.Close();
        }

        //cancel button click
        private void cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
