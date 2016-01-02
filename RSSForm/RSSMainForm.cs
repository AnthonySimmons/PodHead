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

        char[] delimit = { ' ', '\'' };

        string ttl;
        bool webcheck = false;

        const string itunesPodcastUrl = "https://itunes.apple.com/us/genre/podcasts/id26?mt=2";


        public Form msg = new Form();
        ProgressBar msgProg = new ProgressBar();

        public RSSMainForm()
        {
            InitializeComponent();
            //SetupTreeViewContextMenu();
            SetupGridViewContextMenu();
            loadMessageForm();

            treeView1.ImageList = new ImageList();
            treeView1.ImageList.Images.Add(nameof(Properties.Resources.RSS_Icon), Properties.Resources.RSS_Icon);
            treeView1.ImageList.Images.Add(nameof(Properties.Resources.bookmark_ribbon), Properties.Resources.bookmark_ribbon);

            comboBoxSource.SelectedIndex = 0;

            Feeds.Instance.UpdateProgressBar += UpdateProgressBar;
            Feeds.Instance.Load(RSSConfig.ConfigFileName);
            Feeds.Instance.ParseAllFeeds();
            
            //populate the listbox feed_list_display with the Saved channels

            dateTimePicker1.Format = DateTimePickerFormat.Short;
            dateTimePicker2.Format = DateTimePickerFormat.Short;
            LoadSubscriptions();
            //treeView1.CheckBoxes = true;
            treeView1.BackColor = Color.CornflowerBlue;
            webBrowser1.Navigate(itunesPodcastUrl);
            webBrowser1.ObjectForScripting = this;
            Bitmap ico = new Bitmap(Properties.Resources.RSS_Icon);
            this.Icon = Icon.FromHandle(ico.GetHicon());

        }


        private ContextMenuStrip GetCategoryContextMenuStrip()
        {
            var contextMenuStrip = new ContextMenuStrip();
            contextMenuStrip.Items.Add("Add", Properties.Resources.AddIcon, TreeContextClick);
            contextMenuStrip.Items.Add("Refresh", null, TreeContextClick);
            return contextMenuStrip;
        }


        private ContextMenuStrip GetSubscriptionContextMenuStrip()
        {
            var contextMenuStrip = new ContextMenuStrip();
            contextMenuStrip.Items.Add("Remove", Properties.Resources.deleteIcon, TreeContextClick);
            contextMenuStrip.Items.Add("Load", Properties.Resources.LoadIcon, TreeContextClick);
            contextMenuStrip.Items.Add("Info", Properties.Resources.info, TreeContextClick);
            contextMenuStrip.Items.Add("Refresh", Properties.Resources.reload, TreeContextClick);
            return contextMenuStrip;
        }

        private ContextMenuStrip GetTopChartContextMenuStrip()
        {
            var contextMenuStrip = new ContextMenuStrip();
            contextMenuStrip.Items.Add("Info", Properties.Resources.info, TreeContextClick);
            contextMenuStrip.Items.Add("Subscribe", Properties.Resources.bookmark_ribbon, TreeContextClick);
            return contextMenuStrip;
        }


        private void SetupGridViewContextMenu()
        {
            ContextMenuStrip menu = new ContextMenuStrip();
            menu.Items.Add("Download", Properties.Resources.downloads_icon, GridContextClick);
            menu.Items.Add("Delete", Properties.Resources.deleteIcon, GridContextClick);
            menu.Items.Add("Play", Properties.Resources.PlayIcon, GridContextClick);
            menu.Items.Add("Info", Properties.Resources.info, GridContextClick);

            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                column.ContextMenuStrip = menu;
            }
        }

        private void GridContextClick(object sender, EventArgs e)
        {
            ToolStripItem strip = (ToolStripItem)sender;
            switch (strip.Text)
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
                case "Info":
                    LoadItemInfo();
                    break;
            }
        }

        private void TreeContextClick(object sender, EventArgs e)
        {
            ToolStripItem strip = (ToolStripItem)sender;
            switch (strip.Text)
            {
                case "Add":
                    AddSubscription();
                    break;
                case "Remove":
                    Feeds.Instance.RemoveChannel(GetTitleFromSelectedTreeNode());
                    Feeds.Instance.Save(RSSConfig.ConfigFileName);
                    LoadSubscriptions();
                    break;
                case "Load":
                    string title = treeView1.SelectedNode.Text;
                    LoadDataGrid(title);
                    LoadChannelLink();
                    break;
                case "Info":
                    LoadSubscriptionInfo(treeView1.SelectedNode.Text);
                    break;
                case "Refresh":
                    RefreshChannel(treeView1.SelectedNode.Text);
                    break;
                case "Subscribe":
                    AddChartSubscription();
                    break;
            }
        }

        private void AddChartSubscription()
        {
            Subscription sub;
            string subTitle = treeView1.SelectedNode.Text;
            sub = PodcastSource.Instance.Podcasts.FirstOrDefault(p => p.Title == subTitle);
            if (sub != null)
            {
                Feeds.Instance.AddChannel(sub);
            }
        }

        private void RefreshChannel(string channelTitle)
        {
            Subscription ch = Feeds.Instance.Subscriptions.FirstOrDefault(c => c.Title == channelTitle);
            if (ch != null)
            {
                Parser.LoadAnyVersion(ch, Feeds.Instance.MaxItems);
                LoadSubscriptions();
            }
        }

        private void LoadSubscriptionInfo(string subscriptionTitle)
        {
            Subscription sub = GetSubscription(subscriptionTitle);
            if (sub != null)
            {
                var info = new SubscriptionInfo(sub);
                info.ShowDialog();
            }
        }

        private void LoadItemInfo()
        {
            int row = dataGridView1.SelectedCells[0].RowIndex;
            if (dataGridView1.Rows[row].Cells["Title"].Value != null)
            {
                string title = dataGridView1.Rows[row].Cells["Title"].Value.ToString();
                string subscriptionTitle = selectedSub;
                
                Subscription sub = GetSubscription(subscriptionTitle);
                Item it = sub.Items.FirstOrDefault(i => i.Title == title);

                if (sub != null && it != null)
                {
                    var info = new SubscriptionInfo(sub, it);
                    info.ShowDialog();
                }
            }
            
        }


        private void LoadChannelLink()
        {
            if (!String.IsNullOrEmpty(treeView1.SelectedNode.Text))
            {
                Subscription ch = Feeds.Instance.Subscriptions.FirstOrDefault(m => m.Title == treeView1.SelectedNode.Text);
                if (ch != null)
                {
                    if (!String.IsNullOrEmpty(ch.SiteLink))
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
            lbl.Font = new Font(FontFamily.GenericSerif, 12, FontStyle.Bold);
            lbl.Enabled = true;
            lbl.ForeColor = System.Drawing.Color.Black;
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
            Feeds.Instance.Save(RSSConfig.ConfigFileName);
        }


        public bool showUnread = false;
        private void showUnreadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            showUnread = !showUnread;
            LoadDataGrid(treeView1?.SelectedNode?.Text);
            LoadSubscriptions();

        }
        public void LoadSubscriptions()
        {
            treeView1.BeginUpdate();
            treeView1.SuspendLayout();
            treeView1.Nodes.Clear();

            foreach (string category in Feeds.Instance.Categories)
            {
                TreeNode categoryNode = new TreeNode();
                categoryNode.NodeFont = new Font(FontFamily.GenericSansSerif, 14, FontStyle.Bold);
                categoryNode.Text = category;
                categoryNode.ContextMenuStrip = GetCategoryContextMenuStrip();

                foreach (Subscription sub in Feeds.Instance.ChannelsByCategory(category))
                {
                    TreeNode subNode = new TreeNode();
                    subNode.NodeFont = new Font(FontFamily.GenericSansSerif, 12, FontStyle.Italic);
                    subNode.Text = sub.Title;
                    subNode.ContextMenuStrip = GetSubscriptionContextMenuStrip();
                    if (sub.HasErrors)
                    {
                        subNode.BackColor = sub.HasErrors ? Color.Red : Color.Transparent;
                    }

                    if (!categoryNode.Nodes.ContainsKey(subNode.Name))
                    {
                        categoryNode.Nodes.Add(subNode);
                    }
                }

                if (!treeView1.Nodes.ContainsKey(categoryNode.Name))
                {
                    treeView1.Nodes.Add(categoryNode);
                }
            }
            treeView1.EndUpdate();
            treeView1.ResumeLayout();
            treeView1.Refresh();
        }


        private void LoadTopCharts()
        {
            treeView1.BeginUpdate();
            treeView1.SuspendLayout();
            treeView1.Nodes.Clear();
            foreach(Subscription sub in PodcastSource.Instance.Podcasts)
            {
                TreeNode subNode = new TreeNode();
                subNode.NodeFont = new Font(FontFamily.GenericSansSerif, 12, FontStyle.Italic);
                subNode.Text = sub.Title;
                subNode.ContextMenuStrip = GetTopChartContextMenuStrip();

                if(Feeds.Instance.ContainsSubscription(sub.Title))
                {
                    subNode.SelectedImageKey = nameof(Properties.Resources.bookmark_ribbon);
                }

                if(!treeView1.Nodes.ContainsKey(subNode.Name))
                {
                    treeView1.Nodes.Add(subNode);
                }
            }
            treeView1.EndUpdate();
            treeView1.ResumeLayout();
            treeView1.Refresh();
        }


        private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (treeView1.SelectedNode != null)
            {
                string str = treeView1.SelectedNode.Text;
                var sub = GetSubscription(str);
                if (sub != null)
                {
                    selectedSub = sub.Title;
                    if (!string.IsNullOrEmpty(sub.SiteLink))
                    {
                        webBrowser1.Navigate(sub.SiteLink);
                    }
                    else if (!string.IsNullOrEmpty(sub.ImageUrl))
                    {
                        webBrowser1.Navigate(sub.ImageUrl);
                    }
                    LoadDataGrid(str);
                }
            }

        }

        private string findDescFromTitle(string title)
        {
            foreach (Subscription ch in Feeds.Instance.Subscriptions)
            {
                foreach (Item it in ch.Items)
                {
                    if (title == it.Title)
                    {
                        //it.read = true;
                        return it.Description;

                    }
                }
            }
            return "";
        }
        private string findLinkFromTitle(string title)
        {
            foreach (Subscription ch in Feeds.Instance.Subscriptions)
            {
                foreach (Item it in ch.Items)
                {
                    if (title == it.Title)
                    {
                        if (webcheck == true)
                        {
                            it.Read = true;
                        }

                        return it.Link;

                    }
                }
            }
            return "";
        }


        private bool DeleteFile()
        {
            bool success = false;
            if (dataGridView1.SelectedCells.Count > 0)
            {
                int row = dataGridView1.SelectedCells[0].RowIndex;
                if (dataGridView1.Rows[row].Cells["Title"].Value != null)
                {
                    string title = dataGridView1.Rows[row].Cells["Title"].Value.ToString();
                    Item it = GetItem(selectedSub, title);
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
                    Item it = GetItem(selectedSub, title);

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
                        
            dataGridView1.Width = this.Width - treeView1.Width;
            dataGridView1.Height = this.Height - webBrowser1.Height;
            dataGridView1.Dock = DockStyle.Fill;

            treeView1.Height = this.Height;
            textBox1.Width = webBrowser1.Width;
            
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
        }



        private void UpdateDataGridRow(Item it)
        {
            int count = 0;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells["Title"].Value.ToString() == it.Title)
                {
                    break;
                }
                count++;
            }
            UpdateDataGridRow(it, count);
        }



        private void UpdateDataGridRow(Item it, int count)
        {
            it.RowNum = count;

            it.CalculateFilePath();

            dataGridView1.Rows[count].Cells["Date"].Value = GetDateTime(it.PubDate);
            dataGridView1.Rows[count].Cells["Description"].Value = it.Description.ToString();
            dataGridView1.Rows[count].Cells["Title"].Style.Font = new System.Drawing.Font(DefaultFont, FontStyle.Italic);
            dataGridView1.Rows[count].Cells["Title"].Style.ForeColor = Color.Blue;
            dataGridView1.Rows[count].Cells["Date"].Style.Font = new System.Drawing.Font(DefaultFont, FontStyle.Bold);


            //if (it.CanBeDownloaded)
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

            if (it.Read)
            {
                dataGridView1.Rows[count].Cells["Title"].Style.BackColor = Color.Red;
            }
        }

        private Subscription GetSubscription(string subscriptionTitle)
        {
            var sub = Feeds.Instance.Subscriptions.FirstOrDefault(s => s.Title == subscriptionTitle);
            if(sub == null)
            {
                sub = PodcastSource.Instance.Podcasts.FirstOrDefault(s => s.Title == subscriptionTitle);
            }
            return sub;
        }

        private Item GetItem(string subscriptionTitle, string itemTitle)
        {
            Item it = null;
            var sub = GetSubscription(subscriptionTitle);
            if(sub != null)
            {
                it = sub.Items.FirstOrDefault(i => i.Title == itemTitle);
            }

            return it;
        }

        public string selectedSub = "";
        private void LoadDataGrid(string channelTitle)
        {
            dataGridView1.SuspendLayout();
            dataGridView1.Rows.Clear();

            Subscription sub = GetSubscription(channelTitle);
            if (sub != null)
            {
                int count = 0;

                foreach (Item it in sub.Items)
                {
                    if ((!showUnread || (showUnread && !it.Read)))
                    {
                        bool filter = false;
                        if (filterDateBefore != "" && it.PubDate != "")
                        {
                            string dateTmp = GetDateTime(it.PubDate);
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
                        if (filterDateAfter != "" && it.PubDate != "")
                        {
                            string dateTmp = GetDateTime(it.PubDate);
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
                            if (it.Read)
                            {
                                dataGridView1.Rows.Add(null, null, "+ " + it.Title);
                            }
                            else
                            {
                                dataGridView1.Rows.Add(null, null, it.Title);
                            }

                            UpdateDataGridRow(it, count);
                            count++;
                        }
                    }
                }


            }
            dataGridView1.ResumeLayout();
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
            Feeds.Instance.Save(saveFileDialog1.FileName);

        }

        private void openConfigurationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();

            Feeds.Instance.Load(openFileDialog1.FileName);
        }


        private void setReadFromUrl(string url)
        {
            foreach (Subscription ch in Feeds.Instance.Subscriptions)
            {
                foreach (Item it in ch.Items)
                {
                    if (url == it.Link)
                    {
                        if (webcheck == true)
                        {
                            it.Read = true;
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
            LoadDataGrid(selectedSub);
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            filterDateBefore = dateTimePicker2.Text;
            LoadDataGrid(selectedSub);
        }

        private void PlaySelected()
        {
            int row = dataGridView1.SelectedCells[0].RowIndex;
            if (dataGridView1.Rows[row].Cells["Title"].Value != null)
            {
                string title = dataGridView1.Rows[row].Cells["Title"].Value.ToString();

                Item it = GetItem(selectedSub, title);

                if (it != null && (it.Link.EndsWith(".mp3") || it.Link.EndsWith(".mpeg") || it.Link.EndsWith(".wma")))
                {
                    tabControl1.SelectTab(1);
                    if (it.IsDownloaded)
                    {
                        axWindowsMediaPlayer1.URL = it.FilePath;
                    }
                    else
                    {
                        axWindowsMediaPlayer1.URL = it.Link;
                    }
                }
                else
                {
                    webBrowser1.Navigate(it.Link);
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
                    Feeds.Instance.MaxItems += 10;

                    loadMessageForm();
                    //myparser.maxItems = max;

                    if (selectedSub != "")
                    {
                        //string select = treeView1.SelectedNode.Text;
                        LoadSubscriptions();
                        if (webcheck == true)
                        {
                            LoadDataGrid(selectedSub);
                        }
                    }
                }
                else if (selected[0].Value != null)
                {
                    ttl = dataGridView1.Rows[selected[0].RowIndex].Cells["Title"].Value.ToString();

                    Item it = GetItem(selectedSub, ttl);
                    string str = it.Link;

                    if (str.EndsWith(".mp3") || str.EndsWith(".mpeg") || str.EndsWith(".wma"))
                    {
                        tabControl1.SelectTab(1);
                        if (it.IsDownloaded)
                        {
                            axWindowsMediaPlayer1.URL = it.FilePath;
                        }
                        else
                        {
                            axWindowsMediaPlayer1.URL = it.Link;
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
                        LoadDataGrid(selectedSub);
                    }

                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 1)
            {
                string title = GetTitleFromSelectedDataGrid();
                Item it = GetItem(selectedSub, title);
                
                if (it != null && it.CanBeDownloaded)
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

            if (result == DialogResult.OK)
            {
                Feeds.Instance.AddChannel(form.NewSubscription);
                form.Dispose();
                LoadSubscriptions();
            }
        }

        private void UpdateDisplay(string selectedDisplay)
        {
            switch(selectedDisplay)
            {
                case "Subscriptions":
                    comboBoxGenre.Visible = false;
                    LoadSubscriptions();
                    break;
                case "Top Charts":
                    comboBoxGenre.Visible = true;
                    LoadTopCharts();
                    break;
                case "Downloads":
                    break;
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

        private void comboBoxDisplay_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateDisplay(comboBoxSource?.SelectedItem?.ToString());
        }
        
    }
}
