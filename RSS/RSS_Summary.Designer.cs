namespace RSS
{
    partial class RSS_Summary
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
            this.title = new System.Windows.Forms.Label();
            this.Date = new System.Windows.Forms.Label();
            this.link = new System.Windows.Forms.LinkLabel();
            this.description = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // title
            // 
            this.title.AutoSize = true;
            this.title.Location = new System.Drawing.Point(12, 9);
            this.title.Name = "title";
            this.title.Size = new System.Drawing.Size(23, 13);
            this.title.TabIndex = 0;
            this.title.Text = "title";
            // 
            // Date
            // 
            this.Date.AutoSize = true;
            this.Date.Location = new System.Drawing.Point(12, 22);
            this.Date.Name = "Date";
            this.Date.Size = new System.Drawing.Size(30, 13);
            this.Date.TabIndex = 1;
            this.Date.Text = "Date";
            // 
            // link
            // 
            this.link.AutoSize = true;
            this.link.Location = new System.Drawing.Point(12, 35);
            this.link.Name = "link";
            this.link.Size = new System.Drawing.Size(23, 13);
            this.link.TabIndex = 3;
            this.link.TabStop = true;
            this.link.Text = "link";
            this.link.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.link_LinkClicked);
            // 
            // description
            // 
            this.description.AutoSize = true;
            this.description.Location = new System.Drawing.Point(12, 48);
            this.description.Name = "description";
            this.description.Size = new System.Drawing.Size(58, 13);
            this.description.TabIndex = 4;
            this.description.Text = "description";
            // 
            // RSS_Summary
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(640, 285);
            this.Controls.Add(this.description);
            this.Controls.Add(this.link);
            this.Controls.Add(this.Date);
            this.Controls.Add(this.title);
            this.Name = "RSS_Summary";
            this.Text = "RSS_Summary";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label title;
        private System.Windows.Forms.Label Date;
        private System.Windows.Forms.LinkLabel link;
        private System.Windows.Forms.Label description;
    }
}