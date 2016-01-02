using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using RSS;

namespace RSSForm
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                RSSMainForm form = new RSSMainForm();
                form.Show();

                form.LoadSubscriptions();
                form.msg.Close();
                Application.Run(form);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex);
                MessageBox.Show(ex.Message + ex.InnerException?.Message + ex.StackTrace);
            }
        }
    }    
}

