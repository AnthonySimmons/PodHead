using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace RSS
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            RSSMainForm form = new RSSMainForm();
            form.Show();
            
            form.treeViewRefresh();
            form.msg.Close();
            Application.Run(form);
        }
    }    
}

