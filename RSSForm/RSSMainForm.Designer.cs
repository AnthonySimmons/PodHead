namespace RSSForm
{
    partial class RSSMainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RSSMainForm));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newSubcriptionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.refreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageWebsite = new System.Windows.Forms.TabPage();
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.tabPageNowPlaying = new System.Windows.Forms.TabPage();
            this.axWindowsMediaPlayer1 = new AxWMPLib.AxWindowsMediaPlayer();
            this.tabPageSubscription = new System.Windows.Forms.TabPage();
            this.tableLayoutPanelSubInfo = new System.Windows.Forms.TableLayoutPanel();
            this.textBoxSubDescription = new System.Windows.Forms.TextBox();
            this.labelSubscriptionTitle = new System.Windows.Forms.Label();
            this.pictureBoxSubInfo = new System.Windows.Forms.PictureBox();
            this.linkLabelSite = new System.Windows.Forms.LinkLabel();
            this.linkLabelFeed = new System.Windows.Forms.LinkLabel();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.DownloadProgress = new System.Windows.Forms.DataGridViewImageColumn();
            this.Download = new System.Windows.Forms.DataGridViewImageColumn();
            this.Title = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Date = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Description = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.label1 = new System.Windows.Forms.Label();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.textBoxSearch = new System.Windows.Forms.TextBox();
            this.comboBoxSource = new System.Windows.Forms.ComboBox();
            this.comboBoxGenre = new System.Windows.Forms.ComboBox();
            this.buttonSearch = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.buttonAdd = new System.Windows.Forms.Button();
            this.buttonRefresh = new System.Windows.Forms.Button();
            this.progressBarSubscriptions = new System.Windows.Forms.ProgressBar();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolTipButtonAdd = new System.Windows.Forms.ToolTip(this.components);
            this.toolTipButtonRefresh = new System.Windows.Forms.ToolTip(this.components);
            this.subscriptionListControl1 = new RSSForm.SubscriptionListControl();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPageWebsite.SuspendLayout();
            this.tabPageNowPlaying.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axWindowsMediaPlayer1)).BeginInit();
            this.tabPageSubscription.SuspendLayout();
            this.tableLayoutPanelSubInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSubInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(745, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newSubcriptionToolStripMenuItem,
            this.refreshToolStripMenuItem,
            this.toolStripSeparator2,
            this.exitToolStripMenuItem2});
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.exitToolStripMenuItem.Text = "Options";
            // 
            // newSubcriptionToolStripMenuItem
            // 
            this.newSubcriptionToolStripMenuItem.Image = global::RSSForm.Properties.Resources.Subscribe;
            this.newSubcriptionToolStripMenuItem.Name = "newSubcriptionToolStripMenuItem";
            this.newSubcriptionToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.newSubcriptionToolStripMenuItem.Text = "Add Subcription";
            this.newSubcriptionToolStripMenuItem.Click += new System.EventHandler(this.newSubcriptionToolStripMenuItem_Click);
            // 
            // refreshToolStripMenuItem
            // 
            this.refreshToolStripMenuItem.Image = global::RSSForm.Properties.Resources.Refresh;
            this.refreshToolStripMenuItem.Name = "refreshToolStripMenuItem";
            this.refreshToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.refreshToolStripMenuItem.Text = "Refresh";
            this.refreshToolStripMenuItem.Click += new System.EventHandler(this.refreshToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(157, 6);
            // 
            // exitToolStripMenuItem2
            // 
            this.exitToolStripMenuItem2.Name = "exitToolStripMenuItem2";
            this.exitToolStripMenuItem2.Size = new System.Drawing.Size(160, 22);
            this.exitToolStripMenuItem2.Text = "Exit";
            this.exitToolStripMenuItem2.Click += new System.EventHandler(this.exitToolStripMenuItem2_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tabControl1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.dataGridView1);
            this.splitContainer1.Size = new System.Drawing.Size(483, 551);
            this.splitContainer1.SplitterDistance = 253;
            this.splitContainer1.TabIndex = 3;
            // 
            // tabControl1
            // 
            this.tabControl1.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.tabControl1.Controls.Add(this.tabPageWebsite);
            this.tabControl1.Controls.Add(this.tabPageNowPlaying);
            this.tabControl1.Controls.Add(this.tabPageSubscription);
            this.tabControl1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl1.HotTrack = true;
            this.tabControl1.ItemSize = new System.Drawing.Size(125, 25);
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(483, 253);
            this.tabControl1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabControl1.TabIndex = 3;
            // 
            // tabPageWebsite
            // 
            this.tabPageWebsite.BackColor = System.Drawing.SystemColors.Control;
            this.tabPageWebsite.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tabPageWebsite.Controls.Add(this.webBrowser1);
            this.tabPageWebsite.Location = new System.Drawing.Point(4, 29);
            this.tabPageWebsite.Name = "tabPageWebsite";
            this.tabPageWebsite.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageWebsite.Size = new System.Drawing.Size(475, 220);
            this.tabPageWebsite.TabIndex = 1;
            this.tabPageWebsite.Text = "Website";
            // 
            // webBrowser1
            // 
            this.webBrowser1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowser1.Location = new System.Drawing.Point(3, 3);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.ScriptErrorsSuppressed = true;
            this.webBrowser1.Size = new System.Drawing.Size(467, 212);
            this.webBrowser1.TabIndex = 0;
            // 
            // tabPageNowPlaying
            // 
            this.tabPageNowPlaying.BackColor = System.Drawing.SystemColors.Control;
            this.tabPageNowPlaying.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tabPageNowPlaying.Controls.Add(this.axWindowsMediaPlayer1);
            this.tabPageNowPlaying.Location = new System.Drawing.Point(4, 29);
            this.tabPageNowPlaying.Name = "tabPageNowPlaying";
            this.tabPageNowPlaying.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageNowPlaying.Size = new System.Drawing.Size(475, 220);
            this.tabPageNowPlaying.TabIndex = 0;
            this.tabPageNowPlaying.Text = "Now Playing";
            // 
            // axWindowsMediaPlayer1
            // 
            this.axWindowsMediaPlayer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.axWindowsMediaPlayer1.Enabled = true;
            this.axWindowsMediaPlayer1.Location = new System.Drawing.Point(3, 3);
            this.axWindowsMediaPlayer1.Name = "axWindowsMediaPlayer1";
            this.axWindowsMediaPlayer1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axWindowsMediaPlayer1.OcxState")));
            this.axWindowsMediaPlayer1.Size = new System.Drawing.Size(467, 212);
            this.axWindowsMediaPlayer1.TabIndex = 0;
            // 
            // tabPageSubscription
            // 
            this.tabPageSubscription.BackColor = System.Drawing.SystemColors.Control;
            this.tabPageSubscription.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tabPageSubscription.Controls.Add(this.tableLayoutPanelSubInfo);
            this.tabPageSubscription.Location = new System.Drawing.Point(4, 29);
            this.tabPageSubscription.Name = "tabPageSubscription";
            this.tabPageSubscription.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageSubscription.Size = new System.Drawing.Size(475, 220);
            this.tabPageSubscription.TabIndex = 2;
            this.tabPageSubscription.Text = "Subscription";
            // 
            // tableLayoutPanelSubInfo
            // 
            this.tableLayoutPanelSubInfo.ColumnCount = 2;
            this.tableLayoutPanelSubInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.tableLayoutPanelSubInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelSubInfo.Controls.Add(this.textBoxSubDescription, 0, 3);
            this.tableLayoutPanelSubInfo.Controls.Add(this.labelSubscriptionTitle, 0, 0);
            this.tableLayoutPanelSubInfo.Controls.Add(this.pictureBoxSubInfo, 0, 1);
            this.tableLayoutPanelSubInfo.Controls.Add(this.linkLabelSite, 1, 1);
            this.tableLayoutPanelSubInfo.Controls.Add(this.linkLabelFeed, 1, 2);
            this.tableLayoutPanelSubInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelSubInfo.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanelSubInfo.Name = "tableLayoutPanelSubInfo";
            this.tableLayoutPanelSubInfo.RowCount = 4;
            this.tableLayoutPanelSubInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanelSubInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10.20408F));
            this.tableLayoutPanelSubInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.22449F));
            this.tableLayoutPanelSubInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 77.77778F));
            this.tableLayoutPanelSubInfo.Size = new System.Drawing.Size(467, 212);
            this.tableLayoutPanelSubInfo.TabIndex = 3;
            // 
            // textBoxSubDescription
            // 
            this.textBoxSubDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxSubDescription.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxSubDescription.Location = new System.Drawing.Point(220, 68);
            this.textBoxSubDescription.Margin = new System.Windows.Forms.Padding(20, 3, 0, 3);
            this.textBoxSubDescription.Multiline = true;
            this.textBoxSubDescription.Name = "textBoxSubDescription";
            this.textBoxSubDescription.ReadOnly = true;
            this.textBoxSubDescription.Size = new System.Drawing.Size(249, 141);
            this.textBoxSubDescription.TabIndex = 3;
            // 
            // labelSubscriptionTitle
            // 
            this.labelSubscriptionTitle.AutoSize = true;
            this.tableLayoutPanelSubInfo.SetColumnSpan(this.labelSubscriptionTitle, 2);
            this.labelSubscriptionTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSubscriptionTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelSubscriptionTitle.Location = new System.Drawing.Point(3, 0);
            this.labelSubscriptionTitle.Name = "labelSubscriptionTitle";
            this.labelSubscriptionTitle.Size = new System.Drawing.Size(463, 25);
            this.labelSubscriptionTitle.TabIndex = 1;
            this.labelSubscriptionTitle.Text = "Title";
            this.labelSubscriptionTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelSubscriptionTitle.Visible = false;
            // 
            // pictureBoxSubInfo
            // 
            this.pictureBoxSubInfo.BackgroundImage = global::RSSForm.Properties.Resources.Icon;
            this.pictureBoxSubInfo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBoxSubInfo.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pictureBoxSubInfo.Location = new System.Drawing.Point(3, 28);
            this.pictureBoxSubInfo.MaximumSize = new System.Drawing.Size(200, 200);
            this.pictureBoxSubInfo.MinimumSize = new System.Drawing.Size(200, 200);
            this.pictureBoxSubInfo.Name = "pictureBoxSubInfo";
            this.tableLayoutPanelSubInfo.SetRowSpan(this.pictureBoxSubInfo, 3);
            this.pictureBoxSubInfo.Size = new System.Drawing.Size(200, 200);
            this.pictureBoxSubInfo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxSubInfo.TabIndex = 0;
            this.pictureBoxSubInfo.TabStop = false;
            // 
            // linkLabelSite
            // 
            this.linkLabelSite.AutoSize = true;
            this.linkLabelSite.Dock = System.Windows.Forms.DockStyle.Fill;
            this.linkLabelSite.Location = new System.Drawing.Point(203, 25);
            this.linkLabelSite.Name = "linkLabelSite";
            this.linkLabelSite.Size = new System.Drawing.Size(263, 19);
            this.linkLabelSite.TabIndex = 4;
            this.linkLabelSite.TabStop = true;
            this.linkLabelSite.Text = "Site";
            this.linkLabelSite.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.linkLabelSite.Visible = false;
            this.linkLabelSite.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelSite_LinkClicked);
            // 
            // linkLabelFeed
            // 
            this.linkLabelFeed.AutoSize = true;
            this.linkLabelFeed.Dock = System.Windows.Forms.DockStyle.Fill;
            this.linkLabelFeed.Location = new System.Drawing.Point(203, 44);
            this.linkLabelFeed.Name = "linkLabelFeed";
            this.linkLabelFeed.Size = new System.Drawing.Size(263, 21);
            this.linkLabelFeed.TabIndex = 5;
            this.linkLabelFeed.TabStop = true;
            this.linkLabelFeed.Text = "Feed";
            this.linkLabelFeed.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.linkLabelFeed.Visible = false;
            this.linkLabelFeed.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelFeed_LinkClicked);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.DownloadProgress,
            this.Download,
            this.Title,
            this.Date,
            this.Description});
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.Size = new System.Drawing.Size(483, 294);
            this.dataGridView1.TabIndex = 12;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            this.dataGridView1.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentDoubleClick);
            this.dataGridView1.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView1_CellMouseDown);
            this.dataGridView1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dataGridView1_MouseDown);
            // 
            // DownloadProgress
            // 
            this.DownloadProgress.FillWeight = 50F;
            this.DownloadProgress.HeaderText = "Size";
            this.DownloadProgress.Name = "DownloadProgress";
            this.DownloadProgress.ReadOnly = true;
            this.DownloadProgress.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.DownloadProgress.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.DownloadProgress.Width = 75;
            // 
            // Download
            // 
            this.Download.HeaderText = "";
            this.Download.Name = "Download";
            this.Download.ReadOnly = true;
            this.Download.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Download.Width = 25;
            // 
            // Title
            // 
            this.Title.HeaderText = "Title";
            this.Title.Name = "Title";
            this.Title.ReadOnly = true;
            this.Title.Width = 200;
            // 
            // Date
            // 
            this.Date.HeaderText = "Date";
            this.Date.Name = "Date";
            this.Date.ReadOnly = true;
            this.Date.Width = 200;
            // 
            // Description
            // 
            this.Description.HeaderText = "Description";
            this.Description.Name = "Description";
            this.Description.ReadOnly = true;
            this.Description.Width = 500;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(436, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 13);
            this.label1.TabIndex = 4;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer2.Location = new System.Drawing.Point(3, 30);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.tableLayoutPanel1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.splitContainer1);
            this.splitContainer2.Size = new System.Drawing.Size(730, 551);
            this.splitContainer2.SplitterDistance = 243;
            this.splitContainer2.TabIndex = 10;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(243, 551);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.Controls.Add(this.textBoxSearch, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.comboBoxSource, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.comboBoxGenre, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.buttonSearch, 1, 2);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 3;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.Size = new System.Drawing.Size(237, 105);
            this.tableLayoutPanel2.TabIndex = 1;
            // 
            // textBoxSearch
            // 
            this.textBoxSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxSearch.Location = new System.Drawing.Point(3, 81);
            this.textBoxSearch.Name = "textBoxSearch";
            this.textBoxSearch.Size = new System.Drawing.Size(154, 23);
            this.textBoxSearch.TabIndex = 5;
            this.textBoxSearch.Text = "Search...";
            this.textBoxSearch.Visible = false;
            this.textBoxSearch.Enter += new System.EventHandler(this.textBoxSearch_Enter);
            this.textBoxSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxSearch_KeyDown);
            this.textBoxSearch.Leave += new System.EventHandler(this.textBoxSearch_Leave);
            // 
            // comboBoxSource
            // 
            this.tableLayoutPanel2.SetColumnSpan(this.comboBoxSource, 2);
            this.comboBoxSource.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxSource.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSource.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.comboBoxSource.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBoxSource.FormattingEnabled = true;
            this.comboBoxSource.Items.AddRange(new object[] {
            "Subscriptions",
            "Downloads",
            "Top Charts",
            "Search"});
            this.comboBoxSource.Location = new System.Drawing.Point(3, 3);
            this.comboBoxSource.Name = "comboBoxSource";
            this.comboBoxSource.Size = new System.Drawing.Size(231, 33);
            this.comboBoxSource.TabIndex = 1;
            this.comboBoxSource.SelectedIndexChanged += new System.EventHandler(this.comboBoxDisplay_SelectedIndexChanged);
            // 
            // comboBoxGenre
            // 
            this.comboBoxGenre.BackColor = System.Drawing.SystemColors.Window;
            this.tableLayoutPanel2.SetColumnSpan(this.comboBoxGenre, 2);
            this.comboBoxGenre.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxGenre.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxGenre.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.comboBoxGenre.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBoxGenre.ForeColor = System.Drawing.SystemColors.ControlText;
            this.comboBoxGenre.FormattingEnabled = true;
            this.comboBoxGenre.Location = new System.Drawing.Point(3, 42);
            this.comboBoxGenre.Name = "comboBoxGenre";
            this.comboBoxGenre.Size = new System.Drawing.Size(231, 33);
            this.comboBoxGenre.TabIndex = 2;
            this.comboBoxGenre.Visible = false;
            this.comboBoxGenre.SelectedIndexChanged += new System.EventHandler(this.comboBoxGenre_SelectedIndexChanged);
            // 
            // buttonSearch
            // 
            this.buttonSearch.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonSearch.Location = new System.Drawing.Point(163, 81);
            this.buttonSearch.Name = "buttonSearch";
            this.buttonSearch.Size = new System.Drawing.Size(71, 23);
            this.buttonSearch.TabIndex = 7;
            this.buttonSearch.Text = "Search";
            this.buttonSearch.UseVisualStyleBackColor = true;
            this.buttonSearch.Click += new System.EventHandler(this.buttonSearch_Click);
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.subscriptionListControl1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 184);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(237, 380);
            this.panel1.TabIndex = 2;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 3;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.Controls.Add(this.buttonAdd, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.buttonRefresh, 1, 1);
            this.tableLayoutPanel3.Controls.Add(this.progressBarSubscriptions, 0, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 114);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(237, 64);
            this.tableLayoutPanel3.TabIndex = 3;
            // 
            // buttonAdd
            // 
            this.buttonAdd.BackgroundImage = global::RSSForm.Properties.Resources.Subscribe;
            this.buttonAdd.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.buttonAdd.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonAdd.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonAdd.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonAdd.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonAdd.Location = new System.Drawing.Point(3, 23);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.buttonAdd.Size = new System.Drawing.Size(35, 44);
            this.buttonAdd.TabIndex = 0;
            this.buttonAdd.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonAdd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.buttonAdd.UseVisualStyleBackColor = true;
            this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click_1);
            // 
            // buttonRefresh
            // 
            this.buttonRefresh.BackgroundImage = global::RSSForm.Properties.Resources.Refresh;
            this.buttonRefresh.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.buttonRefresh.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonRefresh.Location = new System.Drawing.Point(53, 23);
            this.buttonRefresh.Name = "buttonRefresh";
            this.buttonRefresh.Size = new System.Drawing.Size(44, 44);
            this.buttonRefresh.TabIndex = 1;
            this.buttonRefresh.UseVisualStyleBackColor = true;
            this.buttonRefresh.Click += new System.EventHandler(this.buttonRefresh_Click);
            // 
            // progressBarSubscriptions
            // 
            this.tableLayoutPanel3.SetColumnSpan(this.progressBarSubscriptions, 3);
            this.progressBarSubscriptions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.progressBarSubscriptions.Location = new System.Drawing.Point(3, 3);
            this.progressBarSubscriptions.Name = "progressBarSubscriptions";
            this.progressBarSubscriptions.Size = new System.Drawing.Size(231, 14);
            this.progressBarSubscriptions.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBarSubscriptions.TabIndex = 2;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(0, 575);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(745, 22);
            this.statusStrip1.TabIndex = 11;
            this.statusStrip1.Text = "statusStrip1";
            this.statusStrip1.Visible = false;
            // 
            // subscriptionListControl1
            // 
            this.subscriptionListControl1.AutoScroll = true;
            this.subscriptionListControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.subscriptionListControl1.Location = new System.Drawing.Point(0, 0);
            this.subscriptionListControl1.Name = "subscriptionListControl1";
            this.subscriptionListControl1.SelectedSubscriptionTitle = null;
            this.subscriptionListControl1.Size = new System.Drawing.Size(237, 380);
            this.subscriptionListControl1.TabIndex = 0;
            // 
            // RSSMainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(745, 597);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.splitContainer2);
            this.HelpButton = true;
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(400, 400);
            this.Name = "RSSMainForm";
            this.Text = "RSS Reader";
            this.Load += new System.EventHandler(this.RSSMainForm_Load);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPageWebsite.ResumeLayout(false);
            this.tabPageNowPlaying.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.axWindowsMediaPlayer1)).EndInit();
            this.tabPageSubscription.ResumeLayout(false);
            this.tableLayoutPanelSubInfo.ResumeLayout(false);
            this.tableLayoutPanelSubInfo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSubInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newSubcriptionToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.ComboBox comboBoxSource;
        private System.Windows.Forms.ComboBox comboBoxGenre;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripMenuItem refreshToolStripMenuItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn Description;
        private System.Windows.Forms.DataGridViewTextBoxColumn Date;
        private System.Windows.Forms.DataGridViewTextBoxColumn Title;
        private System.Windows.Forms.DataGridViewImageColumn Download;
        private System.Windows.Forms.DataGridViewImageColumn DownloadProgress;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.LinkLabel linkLabelFeed;
        private System.Windows.Forms.LinkLabel linkLabelSite;
        private System.Windows.Forms.PictureBox pictureBoxSubInfo;
        private System.Windows.Forms.Label labelSubscriptionTitle;
        private System.Windows.Forms.TextBox textBoxSubDescription;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelSubInfo;
        private System.Windows.Forms.TabPage tabPageSubscription;
        private System.Windows.Forms.TabPage tabPageNowPlaying;
        public System.Windows.Forms.WebBrowser webBrowser1;
        private System.Windows.Forms.TabPage tabPageWebsite;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TextBox textBoxSearch;
        private AxWMPLib.AxWindowsMediaPlayer axWindowsMediaPlayer1;
        private System.Windows.Forms.Panel panel1;
        private SubscriptionListControl subscriptionListControl1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Button buttonAdd;
        private System.Windows.Forms.Button buttonRefresh;
        private System.Windows.Forms.ProgressBar progressBarSubscriptions;
        private System.Windows.Forms.ToolTip toolTipButtonAdd;
        private System.Windows.Forms.ToolTip toolTipButtonRefresh;
        private System.Windows.Forms.Button buttonSearch;
    }
}

