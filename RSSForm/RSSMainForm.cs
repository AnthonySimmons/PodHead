using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Xml;
using System.Security.Permissions;
using RSS;
using WMPLib;

namespace RSSForm
{
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    [System.Runtime.InteropServices.ComVisibleAttribute(true)]
    public partial class RSSMainForm : Form
    {
        int DownloadColumnWidth = 20;
        int DownloadColumnHeight = 20;

        int FileSizeColumnWidth = 75;
        int FileSizeColumnHeight = 25;

        List<string> latlon = new List<string>();
        string[] ttl1 = null;
        string[] desc1 = null;
        List<string> parse = null;
        string longitude;
        string latitude;
        char[] delimit = { ' ', '\'' };
        string desc;
        string ttl;
        string start = "'";
        int strst = 0;
        int stred = 0;
        int i = 0;
        int j = 0;
        bool webcheck = false;

        const string itunesPodcastUrl = "https://itunes.apple.com/us/genre/podcasts/id26?mt=2";

        public static Parser myparser = new Parser();
        
        public Form msg = new Form();
        ProgressBar msgProg = new ProgressBar();

        public RSSMainForm()
        {
            InitializeComponent();
            SetupTreeViewContextMenu();
            SetupGridViewContextMenu();
            loadMessageForm();

            myparser.UpdateProgressBar += UpdateProgressBar;
            myparser.Load(RSSConfig.ConfigFileName);
            myparser.parseAllFeeds();
            var podcastChannel = PodcastSource.GetPodcasts();
            myparser.Channels.Add(podcastChannel);
            //populate the listbox feed_list_display with the Saved channels
            
            dateTimePicker1.Format = DateTimePickerFormat.Short;
            dateTimePicker2.Format = DateTimePickerFormat.Short;
            treeViewRefresh();
            //treeView1.CheckBoxes = true;
            treeView1.BackColor = Color.CornflowerBlue;
            webBrowser1.Navigate(itunesPodcastUrl);
            webBrowser1.ObjectForScripting = this;
            Bitmap ico = new Bitmap(Properties.Resources.RSS_Icon);
            this.Icon = Icon.FromHandle(ico.GetHicon());
            
        }

        public void SetupTreeViewContextMenu()
        {
            treeView1.ContextMenuStrip = new ContextMenuStrip();
            treeView1.ContextMenuStrip.Items.Add("Add", Properties.Resources.AddIcon, TreeContextClick);
            treeView1.ContextMenuStrip.Items.Add("Remove", Properties.Resources.deleteIcon, TreeContextClick);
            treeView1.ContextMenuStrip.Items.Add("Load", Properties.Resources.LoadIcon, TreeContextClick);
            treeView1.ContextMenuStrip.Items.Add("Show", null, TreeContextClick);
            treeView1.ContextMenuStrip.Items.Add("Info", null, TreeContextClick);
        }

        public void SetupGridViewContextMenu()
        {
            ContextMenuStrip menu = new ContextMenuStrip();
            menu.Items.Add("Download", Properties.Resources.downloads_icon, GridContextClick);
            menu.Items.Add("Delete", Properties.Resources.deleteIcon, GridContextClick);
            menu.Items.Add("Play", Properties.Resources.PlayIcon, GridContextClick);
            
            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                column.ContextMenuStrip = menu;
            }
        }

        private void GridContextClick(object sender, EventArgs e)
        {
            ToolStripItem strip = (ToolStripItem)sender;
            switch(strip.Text)
            {
                case "Download":
                    DownloadFile();
                    break;
                case "Delete":
                    DeleteFile();
                    break;
                case "Play":
                    PlaySelected();
                    break;
            }
        }

        private void TreeContextClick(object sender, EventArgs e)
        {
            ToolStripItem strip = (ToolStripItem)sender;
            switch(strip.Text)
            {
                case "Add":
                    AddSubscription();
                    break;
                case "Remove":
                    myparser.RemoveChannel(GetTitleFromSelectedTreeNode());
                    myparser.Save(RSSConfig.ConfigFileName);
                    treeViewRefresh();
                    break;
                case "Load":
                    string title = treeView1.SelectedNode.Text;
                    loadDataGrid(title);
                    break;
                case "Show":
                    LoadChannelLink();
                    break;
                case "Info":
                    LoadChannelInfo(treeView1.SelectedNode.Text);
                    break;
            }
        }


        private void LoadChannelInfo(string channelTitle)
        {
            Channel ch = myparser.Channels.FirstOrDefault(c => c.title == channelTitle);
            if(ch != null)
            {
                var info = new SubscriptionInfo(ch);
                info.ShowDialog();
            }
        }

        private void LoadChannelLink()
        { 
            if(!String.IsNullOrEmpty(treeView1.SelectedNode.Text))
            {
                Channel ch = myparser.Channels.FirstOrDefault(m => m.title == treeView1.SelectedNode.Text);
                if (ch != null)
                { 
                    if(!String.IsNullOrEmpty(ch.SiteLink))
                    {
                        webBrowser1.Navigate(ch.SiteLink);
                    }
                    else if (!String.IsNullOrEmpty(ch.ImageUrl))
                    {
                        webBrowser1.Navigate(ch.ImageUrl);
                    }
                    else if (!String.IsNullOrEmpty(ch.RssLink))
                    {
                        webBrowser1.Navigate(ch.RssLink);
                    }
                }
            }
        }

        private string GetTitleFromSelectedTreeNode()
        {
            string title = String.Empty;

            if (treeView1.SelectedNode != null)
            {
                title = treeView1.SelectedNode.Text;
            }

            return title;
        }

        private string GetTitleFromSelectedDataGrid()
        {
            string title = String.Empty;
            if (dataGridView1.SelectedCells.Count > 0)
            {
                int row = dataGridView1.SelectedCells[0].RowIndex;
                if (dataGridView1.Rows[row].Cells["Title"].Value != null)
                {
                    title = dataGridView1.Rows[row].Cells["Title"].Value.ToString();
                }
            }
            return title;
        }

        public void UpdateProgressBar(int val)
        {
            msgProg.Value = val;
            msgProg.Refresh();
            msg.Refresh();
            msg.BringToFront();
            msg.Activate();
        }

        public void loadMessageForm()
        {
            msg = new Form();
            msg.ShowInTaskbar = false;
            msg.FormBorderStyle = FormBorderStyle.FixedDialog;
            Bitmap bmp = new Bitmap(Properties.Resources.RSS_Icon);
            msg.Icon = Icon.FromHandle(bmp.GetHicon());
            msg.Height = 150;
            msg.Width = 350;
            Label lbl = new Label();
            lbl.Name = "lbl";
            lbl.AutoSize = true;
            lbl.Visible = true;
            lbl.Enabled = true;
            lbl.ForeColor = System.Drawing.Color.ForestGreen;
            lbl.Text = "Loading Saved Subscriptions...";
            lbl.Location = new System.Drawing.Point(50, 20);

            msgProg = new ProgressBar();
            msgProg.Width = 250;
            msgProg.Location = new Point(40, 50);

            msg.Controls.Add(msgProg);
            msg.Controls.Add(lbl);
            msg.Location = new Point(700, 800);
            msg.Show();
            msg.Refresh();
        }


        //Menu strip File -> Exit: closes the program.
        private void exitToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            Close();
        }
       
    
        //Menu strip About: click opens the About form which contains information on authors, version, and last updated
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About about = new About();
            DialogResult dr = about.ShowDialog();
        }


        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            myparser.Save(RSSConfig.ConfigFileName);
        }


        public bool showUnread = false;
        private void showUnreadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            showUnread = !showUnread;
            loadDataGrid(treeView1.SelectedNode.Text);
            treeViewRefresh();

        }
        public void treeViewRefresh()
        {
            treeView1.Nodes.Clear();

            foreach (Channel ch in myparser.Channels)
            {
                TreeNode channelNode = new TreeNode(ch.title);


                foreach (Item it in ch.item)
                {
                    TreeNode itemNode = new TreeNode(it.titleI);
                    if (!channelNode.Nodes.ContainsKey(it.titleI))
                    {
                        channelNode.Nodes.Add(it.titleI);
                    }
                }

                if (ch.HasErrors)
                {
                    channelNode.BackColor = ch.HasErrors ? Color.Red : Color.Transparent;
                }

                if (!treeView1.Nodes.ContainsKey(channelNode.Name))
                {
                    treeView1.Nodes.Add(channelNode);
                }

            }
        }



        private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (treeView1.SelectedNode != null)
            {
                string str = treeView1.SelectedNode.Text;

                foreach (Channel ch in myparser.Channels)
                {
                    if (ch.title.Contains(str))
                    {
                        if (!string.IsNullOrEmpty(ch.SiteLink))
                        {
                            webBrowser1.Navigate(ch.SiteLink);
                        }
                        else if (!string.IsNullOrEmpty(ch.ImageUrl))
                        {
                            webBrowser1.Navigate(ch.ImageUrl);
                        }
                        loadDataGrid(str);
                        break;
                    }
                }
               
            }
        }

        private string findDescFromTitle(string title)
        {
            foreach (Channel ch in myparser.Channels)
            {
                foreach (Item it in ch.item)
                {
                    if (title == it.titleI)
                    {
                        //it.read = true;
                        return it.descriptionI;

                    }
                }
            }
            return "";
        }
        private string findLinkFromTitle(string title)
        {
            foreach (Channel ch in myparser.Channels)
            {
                foreach (Item it in ch.item)
                {
                    if (title == it.titleI)
                    {
                        if (webcheck == true)
                        {
                            it.read = true;
                        }

                        return it.linkI;

                    }
                }
            }
            return "";
        }
       
        string mLink = "";
              

        private bool DeleteFile()
        {
            bool success = false;
            if (dataGridView1.SelectedCells.Count > 0)
            {
                int row = dataGridView1.SelectedCells[0].RowIndex;
                if (dataGridView1.Rows[row].Cells["Title"].Value != null)
                {
                    string title = dataGridView1.Rows[row].Cells["Title"].Value.ToString();
                    Item it = myparser.GetItem(title);
                    success = it.DeleteFile();
                    UpdateDataGridRow(it);
                }                
            }
            return success;
        }
       

        private void DownloadFile()
        {
            if (dataGridView1.SelectedCells.Count > 0)
            {
                int row = dataGridView1.SelectedCells[0].RowIndex;
                if (dataGridView1.Rows[row].Cells["Title"].Value != null)
                {
                    string title = dataGridView1.Rows[row].Cells["Title"].Value.ToString();
                    Item it = myparser.GetItem(title);

                    it.DownloadProgress += DownloadProgressChangeCallBack;
                    it.DownloadComplete += DownloadCompleteCallBack;

                    it.DownloadFile();

                    it.DownloadProgress -= DownloadProgressChangeCallBack;
                    it.DownloadComplete -= DownloadCompleteCallBack;
                }
            }
        }


        public void DownloadCompleteCallBack(int row, int size)
        {
            dataGridView1.Rows[row].Cells["DownloadProgress"].Value = GetFileSizeBitmap(size);
            dataGridView1.Rows[row].Cells["Download"].Value = GetDeleteBitmap();
        }

        public void DownloadProgressChangeCallBack(string MbString, float percent, int row)
        {
            Bitmap bmp = new Bitmap(dataGridView1.Columns[0].Width, dataGridView1.Rows[0].Height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.FillRectangle(Brushes.Green, 0, 0, (int)(bmp.Width * ((float)percent / 100.0)), bmp.Height);
                
                g.DrawString(MbString, new Font("Times New Roman", 10), Brushes.Black, new PointF(bmp.Width / 4, bmp.Height / 4));
            }

            dataGridView1.Rows[row].Cells["DownloadProgress"].Value = bmp;
        
        }


        private void Form1_Resize(object sender, EventArgs e)
        {
            splitContainer1.Height = this.Height - 55;
            splitContainer1.Width = this.Width - treeView1.Width - 25;

            //webBrowser1.Width = this.Width - treeView1.Width - 50;
            //webBrowser1.Height = this.Height - 150;
            

            dataGridView1.Width = this.Width - treeView1.Width;
            dataGridView1.Height = this.Height - webBrowser1.Height;
            dataGridView1.Dock = DockStyle.Fill;

            treeView1.Height = this.Height;
            textBox1.Width = webBrowser1.Width;

            //dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            //dataGridView1.AutoResizeColumns();
            //dataGridView1.Columns[2].Width = 400;
            //  this.webBrowser1.Url = new Uri("http://www.google.com/earth/explore/products/plugin.html");//new System.Uri(System.Environment.CurrentDirectory + "\\" +
            //"HTMLPage1.htm", System.UriKind.Absolute);


        }

        private string GetDateTime(string date)
        {
            var formattedDateTime = string.Empty;
            DateTime dateTime;

            if (DateTime.TryParse(date, out dateTime))
            {
                //Use general date time pattern.
                formattedDateTime = dateTime.ToString("g");
            }

            return formattedDateTime;

            /*if (date != "" && date.Contains('-'))
            {
                string[] strArr = date.Split('-');
                int year = System.Convert.ToInt32(strArr[0]);
                int month = System.Convert.ToInt32(strArr[1]);
                strArr[2] = strArr[2].Substring(0, 2);
                int day = System.Convert.ToInt32(strArr[2]);

                return month.ToString() + "/" + day.ToString() + "/" + year.ToString();
            }
            if (date != null && date != "" && !date.Contains('-'))
            {

                int month = 1;
                int year = 2013;
                int i = date.IndexOf("20");
                if (i > 7) { year = Convert.ToInt32(date.Substring(i, 4)); }
                int day = Convert.ToInt32(date.Substring(4, 3));

                if (date.Contains("Jan")) { month = 1; }
                if (date.Contains("Feb")) { month = 2; }
                if (date.Contains("Mar")) { month = 3; }
                if (date.Contains("Apr")) { month = 4; }
                if (date.Contains("May")) { month = 5; }
                if (date.Contains("Jun")) { month = 6; }
                if (date.Contains("Jul")) { month = 7; }
                if (date.Contains("Aug")) { month = 8; }
                if (date.Contains("Sep")) { month = 9; }
                if (date.Contains("Oct")) { month = 10; }
                if (date.Contains("Nov")) { month = 11; }
                if (date.Contains("Dec")) { month = 12; }

                string dayStr = "";
                if (day < 10) { dayStr += "0"; }
                dayStr += day.ToString();

                return year.ToString() + "/" + month.ToString() + "/" + dayStr;
            }
            else { return ""; }*/
        }



        private void UpdateDataGridRow(Item it)
        {
            int count = 0;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells["Title"].Value.ToString() == it.titleI)
                {
                    break;
                }
                count++;
            }
            UpdateDataGridRow(it, count);
        }



        private void UpdateDataGridRow(Item it, int count)
        {
            it.rowNum = count;

            it.CalculateFilePath();

            dataGridView1.Rows[count].Cells["Date"].Value = GetDateTime(it.pubDateI);
            dataGridView1.Rows[count].Cells["Description"].Value = it.descriptionI.ToString();
            dataGridView1.Rows[count].Cells["Title"].Style.Font = new System.Drawing.Font(DefaultFont, FontStyle.Italic);
            dataGridView1.Rows[count].Cells["Title"].Style.ForeColor = Color.Blue;
            dataGridView1.Rows[count].Cells["Date"].Style.Font = new System.Drawing.Font(DefaultFont, FontStyle.Bold);


            if (it.CanBeDownloaded)
            {
                it.IsDownloaded = it.CheckIsDownloaded();

                if (it.IsDownloaded)
                {
                    it.MbSize = it.GetFileSizeMb();
                    dataGridView1.Rows[count].Cells["Download"].Value = GetDeleteBitmap();
                }
                else
                {
                    dataGridView1.Rows[count].Cells["Download"].Value = GetDownloadBitmap();
                }

                dataGridView1.Rows[count].Cells["DownloadProgress"].Value = GetFileSizeBitmap(it.MbSize);
            }

            if (it.read)
            {
                dataGridView1.Rows[count].Cells["Title"].Style.BackColor = Color.Red;
            }
        }

        public string selectedSub = "";
        private void loadDataGrid(string channelTitle)
        {
            dataGridView1.Rows.Clear();

            foreach (Channel ch in myparser.Channels)
            {
                int count = 0;
                if (!string.IsNullOrEmpty(channelTitle) && ch.title == channelTitle)
                {
                    foreach (Item it in ch.item)
                    {//4/22/2013

                        if ((!showUnread || (showUnread && !it.read)))
                        {
                            bool filter = false;
                            if (filterDateBefore != "" && it.pubDateI != "")
                            {
                                string dateTmp = GetDateTime(it.pubDateI);
                                string[] strArr = dateTmp.Split('/');
                                string[] strArr2 = filterDateBefore.Split('/');
                                int day1 = System.Convert.ToInt32(strArr[1].ToString());
                                int month1 = System.Convert.ToInt32(strArr[0]);
                                int year1 = System.Convert.ToInt32(strArr[2]);

                                int day2 = System.Convert.ToInt32(strArr2[1].ToString());
                                int month2 = System.Convert.ToInt32(strArr2[0]);
                                int year2 = System.Convert.ToInt32(strArr2[2]);


                                if (year1 < year2) { filter = true; }
                                if (year1 == year2 && month1 < month2) { filter = true; }
                                if (month1 == month2 && year1 == year2 && day1 < day2) { filter = true; }
                            }
                            if (filterDateAfter != "" && it.pubDateI != "")
                            {
                                string dateTmp = GetDateTime(it.pubDateI);
                                string[] strArr = dateTmp.Split('/');
                                string[] strArr2 = filterDateAfter.Split('/');
                                int day1 = System.Convert.ToInt32(strArr[1].ToString());
                                int month1 = System.Convert.ToInt32(strArr[0]);
                                int year1 = System.Convert.ToInt32(strArr[2]);

                                int day2 = System.Convert.ToInt32(strArr2[1].ToString());
                                int month2 = System.Convert.ToInt32(strArr2[0]);
                                int year2 = System.Convert.ToInt32(strArr2[2]);


                                if (year1 > year2) { filter = true; }
                                if (year1 == year2 && month1 > month2) { filter = true; }
                                if (month1 == month2 && year1 == year2 && day1 > day2) { filter = true; }
                            }

                            if (!filter)
                            {
                                if (it.read)
                                {
                                    dataGridView1.Rows.Add(null, null, "+ " + it.titleI);
                                }
                                else
                                {
                                    dataGridView1.Rows.Add(null, null, it.titleI);
                                }

                                UpdateDataGridRow(it, count);
                                count++;
                            }
                        }
                    }
                    break;
                }
                
            }
            dataGridView1.Sort(dataGridView1.Columns["Date"], ListSortDirection.Descending);
            dataGridView1.Rows.Add(null, GetBlankBitmap(), "Load 10 More");
            dataGridView1.AutoResizeColumns();
        }


        private Bitmap GetBlankBitmap()
        {
            Bitmap bmp = null;
            if (dataGridView1.Rows.Count > 0)
            {
                bmp = new Bitmap(DownloadColumnWidth, DownloadColumnHeight);
            }
            return bmp;
        }

        private Bitmap GetDownloadBitmap()
        {
            Bitmap bmp = new Bitmap(DownloadColumnWidth, DownloadColumnHeight);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.DrawImage(Properties.Resources.downloads_icon, 0, 0, bmp.Width, bmp.Height);
            }
            return bmp;
        }

        private Bitmap GetFileSizeBitmap(int size)
        {
            Bitmap bmp = new Bitmap(FileSizeColumnWidth, FileSizeColumnHeight);
            if (size > 0)
            {
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    string MbString = size.ToString() + "Mb";
                    g.FillRectangle(Brushes.Green, 0, 0, bmp.Width, bmp.Height);
                    g.DrawString(MbString, new Font("Times New Roman", 10), Brushes.Black, new PointF(bmp.Width / 8, bmp.Height / 8));
                }
            }
            return bmp;
        }

        private Bitmap GetDeleteBitmap()
        {
            Bitmap bmp = new Bitmap(DownloadColumnWidth, DownloadColumnHeight);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.DrawImage(Properties.Resources.deleteIcon, 0, 0, bmp.Width, bmp.Height);
            }
            return bmp;
        }
        

        private void webBrowser1_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            webBrowser1.ScrollBarsEnabled = true;
            webBrowser1.ScriptErrorsSuppressed = true;

            if (webBrowser1.Url != null)
            {
                textBox1.Text = webBrowser1.Url.ToString();
            }
            textBox1.ForeColor = Color.Blue;
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    //webBrowser1.Url = new Uri(textBox1.Text);
                    webBrowser1.Navigate(textBox1.Text);
                }
                catch
                {

                }
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.ShowDialog();
            myparser.Save(saveFileDialog1.FileName);

        }

        private void openConfigurationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();

            myparser.Load(openFileDialog1.FileName);
        }

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            printDialog1.ShowDialog();

            webBrowser1.Print();
        }

        private void setReadFromUrl(string url)
        {
            foreach (Channel ch in myparser.Channels)
            {
                foreach (Item it in ch.item)
                {
                    if (url == it.linkI)
                    {
                        if (webcheck == true)
                        {
                            it.read = true;
                        }
                        //return it.descriptionI;

                    }
                }
            }
        }


        string filterDateBefore = "";
        string filterDateAfter = "";
        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            filterDateAfter = dateTimePicker1.Text;
            loadDataGrid(selectedSub);
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            filterDateBefore = dateTimePicker2.Text;
            loadDataGrid(selectedSub);
        }

        private void PlaySelected()
        {
            int row = dataGridView1.SelectedCells[0].RowIndex;
            if (dataGridView1.Rows[row].Cells["Title"].Value != null)
            {
                string title = dataGridView1.Rows[row].Cells["Title"].Value.ToString();

                Item it = myparser.GetItem(title);

                if (it != null && (it.linkI.EndsWith(".mp3") || it.linkI.EndsWith(".mpeg") || it.linkI.EndsWith(".wma")))
                {
                    tabControl1.SelectTab(1);
                    if (it.IsDownloaded)
                    {
                        axWindowsMediaPlayer1.URL = it.FilePath;
                    }
                    else
                    {
                        axWindowsMediaPlayer1.URL = it.linkI;
                    }
                }
                else
                {
                    webBrowser1.Navigate(it.linkI);
                    tabControl1.SelectTab(0);
                }
            }
        }

        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewSelectedCellCollection selected = dataGridView1.SelectedCells;

            if (selected != null)
            {
                if (selected[0].Value != null && selected[0].Value.ToString() == "Load 10 More")
                {
                    int max = myparser.maxItems + 10;
                    myparser = new Parser();
                    myparser.maxItems = max;
                    loadMessageForm();
                    //myparser.maxItems = max;

                    if (selectedSub != "")
                    {
                        //string select = treeView1.SelectedNode.Text;
                        treeViewRefresh();
                        if (webcheck == true)
                        {
                            loadDataGrid(selectedSub);
                        }
                    }
                }
                else if (selected[0].Value != null)
                {
                    ttl = dataGridView1.Rows[selected[0].RowIndex].Cells["Title"].Value.ToString();

                    Item it = myparser.GetItem(ttl);
                    string str = it.linkI;
                 
                    if (str.EndsWith(".mp3") || str.EndsWith(".mpeg") || str.EndsWith(".wma"))
                    {
                        tabControl1.SelectTab(1);
                        if (it.IsDownloaded)
                        {
                            axWindowsMediaPlayer1.URL = it.FilePath;
                        }
                        else
                        {
                            axWindowsMediaPlayer1.URL = it.linkI;
                        }
                    }
                    else if (!String.IsNullOrEmpty(it.FilePath) && File.Exists(it.FilePath))
                    {
                        webBrowser1.Navigate(it.FilePath); 
                    }
                    else if (str != "")
                    {
                        webBrowser1.Navigate(str);
                    }
                    else
                    {
                        loadDataGrid(selectedSub);
                    }

                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 1)
            {
                string title = GetTitleFromSelectedDataGrid();
                Item it = myparser.GetItem(title);

                if (it.CanBeDownloaded)
                {
                    if (it.IsDownloaded)
                    {
                        it.DeleteFile();
                    }
                    else
                    {
                        it.DownloadComplete += DownloadCompleteCallBack;
                        it.DownloadProgress += DownloadProgressChangeCallBack;

                        it.DownloadFile();

                    }
                }
                UpdateDataGridRow(it);
            }
        }


        private void AddSubscription()
        {
            ADD_RSS_Subscription form = new ADD_RSS_Subscription();
            DialogResult result = form.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                Channel ch = new Channel();
                ch.RssLink = form.return_url;
                ch.update = form.return_update;
                myparser.AddChannel(ch);
                form.Dispose();
                treeViewRefresh();
            }
        }

        private void newSubcriptionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddSubscription();
        }

        private void treeView1_MouseDown(object sender, MouseEventArgs e)
        {
            TreeNode node = treeView1.GetNodeAt(new Point(e.X, e.Y));
            treeView1.SelectedNode = node;
        }

        private void dataGridView1_MouseDown(object sender, MouseEventArgs e)
        {
            
        }

    }    
}
