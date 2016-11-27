namespace RSSForm
{
    partial class About
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(About));
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.ok = new System.Windows.Forms.Button();
            this.labelProjectHome = new System.Windows.Forms.Label();
            this.linkLabelProjectHomeUrl = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(18, 20);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Author:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(18, 40);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(138, 20);
            this.label4.TabIndex = 3;
            this.label4.Text = "Anthony Simmons";
            // 
            // ok
            // 
            this.ok.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ok.Location = new System.Drawing.Point(232, 150);
            this.ok.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ok.Name = "ok";
            this.ok.Size = new System.Drawing.Size(112, 35);
            this.ok.TabIndex = 8;
            this.ok.Text = "OK";
            this.ok.UseVisualStyleBackColor = true;
            this.ok.Click += new System.EventHandler(this.ok_Click);
            // 
            // labelProjectHome
            // 
            this.labelProjectHome.AutoSize = true;
            this.labelProjectHome.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelProjectHome.Location = new System.Drawing.Point(18, 77);
            this.labelProjectHome.Name = "labelProjectHome";
            this.labelProjectHome.Size = new System.Drawing.Size(122, 20);
            this.labelProjectHome.TabIndex = 9;
            this.labelProjectHome.Text = "Project Home:";
            // 
            // linkLabelProjectHomeUrl
            // 
            this.linkLabelProjectHomeUrl.AutoSize = true;
            this.linkLabelProjectHomeUrl.Location = new System.Drawing.Point(22, 101);
            this.linkLabelProjectHomeUrl.Name = "linkLabelProjectHomeUrl";
            this.linkLabelProjectHomeUrl.Size = new System.Drawing.Size(302, 20);
            this.linkLabelProjectHomeUrl.TabIndex = 10;
            this.linkLabelProjectHomeUrl.TabStop = true;
            this.linkLabelProjectHomeUrl.Text = "https://github.com/AnthonySimmons/RSS";
            this.linkLabelProjectHomeUrl.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelProjectHomeUrl_LinkClicked);
            // 
            // About
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(357, 199);
            this.Controls.Add(this.linkLabelProjectHomeUrl);
            this.Controls.Add(this.labelProjectHome);
            this.Controls.Add(this.ok);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "About";
            this.ShowInTaskbar = false;
            this.Text = "About";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button ok;
        private System.Windows.Forms.Label labelProjectHome;
        private System.Windows.Forms.LinkLabel linkLabelProjectHomeUrl;
    }
}