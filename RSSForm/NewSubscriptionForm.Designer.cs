namespace RSSForm
{
    partial class NewSubscriptionForm
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
            this.labelRssUrl = new System.Windows.Forms.Label();
            this.url_entry = new System.Windows.Forms.TextBox();
            this.ADD_button = new System.Windows.Forms.Button();
            this.cancel_button = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // labelRssUrl
            // 
            this.labelRssUrl.AutoSize = true;
            this.labelRssUrl.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelRssUrl.Location = new System.Drawing.Point(12, 30);
            this.labelRssUrl.Name = "labelRssUrl";
            this.labelRssUrl.Size = new System.Drawing.Size(56, 13);
            this.labelRssUrl.TabIndex = 0;
            this.labelRssUrl.Text = "RSS Url:";
            // 
            // url_entry
            // 
            this.url_entry.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.url_entry.Location = new System.Drawing.Point(74, 27);
            this.url_entry.Name = "url_entry";
            this.url_entry.Size = new System.Drawing.Size(387, 20);
            this.url_entry.TabIndex = 1;
            // 
            // ADD_button
            // 
            this.ADD_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ADD_button.Location = new System.Drawing.Point(296, 69);
            this.ADD_button.Name = "ADD_button";
            this.ADD_button.Size = new System.Drawing.Size(75, 23);
            this.ADD_button.TabIndex = 4;
            this.ADD_button.Text = "ADD";
            this.ADD_button.UseVisualStyleBackColor = true;
            this.ADD_button.Click += new System.EventHandler(this.ADD_button_Click);
            // 
            // cancel_button
            // 
            this.cancel_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cancel_button.Location = new System.Drawing.Point(386, 69);
            this.cancel_button.Name = "cancel_button";
            this.cancel_button.Size = new System.Drawing.Size(75, 23);
            this.cancel_button.TabIndex = 5;
            this.cancel_button.Text = "CANCEL";
            this.cancel_button.UseVisualStyleBackColor = true;
            this.cancel_button.Click += new System.EventHandler(this.cancel_button_Click);
            // 
            // NewSubscriptionForm
            // 
            this.AcceptButton = this.ADD_button;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(473, 105);
            this.Controls.Add(this.cancel_button);
            this.Controls.Add(this.ADD_button);
            this.Controls.Add(this.url_entry);
            this.Controls.Add(this.labelRssUrl);
            this.Name = "NewSubscriptionForm";
            this.Text = "Add Subscription";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelRssUrl;
        private System.Windows.Forms.TextBox url_entry;
        private System.Windows.Forms.Button ADD_button;
        private System.Windows.Forms.Button cancel_button;
    }
}