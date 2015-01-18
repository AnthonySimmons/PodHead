namespace RSS
{
    partial class Remove_Rename_Chanel
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
            this.Channels = new System.Windows.Forms.ListBox();
            this.rename = new System.Windows.Forms.Button();
            this.remove = new System.Windows.Forms.Button();
            this.cancel = new System.Windows.Forms.Button();
            this.option = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // Channels
            // 
            this.Channels.FormattingEnabled = true;
            this.Channels.Location = new System.Drawing.Point(13, 13);
            this.Channels.Name = "Channels";
            this.Channels.Size = new System.Drawing.Size(120, 212);
            this.Channels.TabIndex = 0;
            this.Channels.SelectedIndexChanged += new System.EventHandler(this.Channels_SelectedIndexChanged);
            // 
            // rename
            // 
            this.rename.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rename.Location = new System.Drawing.Point(139, 206);
            this.rename.Name = "rename";
            this.rename.Size = new System.Drawing.Size(75, 23);
            this.rename.TabIndex = 1;
            this.rename.Text = "RENAME";
            this.rename.UseVisualStyleBackColor = true;
            this.rename.Click += new System.EventHandler(this.rename_Click);
            // 
            // remove
            // 
            this.remove.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.remove.Location = new System.Drawing.Point(220, 206);
            this.remove.Name = "remove";
            this.remove.Size = new System.Drawing.Size(75, 23);
            this.remove.TabIndex = 2;
            this.remove.Text = "REMOVE";
            this.remove.UseVisualStyleBackColor = true;
            this.remove.Click += new System.EventHandler(this.remove_Click);
            // 
            // cancel
            // 
            this.cancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cancel.Location = new System.Drawing.Point(301, 206);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(75, 23);
            this.cancel.TabIndex = 3;
            this.cancel.Text = "CANCEL";
            this.cancel.UseVisualStyleBackColor = true;
            this.cancel.Click += new System.EventHandler(this.cancel_Click);
            // 
            // option
            // 
            this.option.Location = new System.Drawing.Point(139, 180);
            this.option.Name = "option";
            this.option.Size = new System.Drawing.Size(237, 20);
            this.option.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(140, 161);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(81, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Re-Name Here:";
            // 
            // Remove_Rename_Chanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(380, 240);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.option);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.remove);
            this.Controls.Add(this.rename);
            this.Controls.Add(this.Channels);
            this.Name = "Remove_Rename_Chanel";
            this.Text = "Remove_Rename_Chanel";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox Channels;
        private System.Windows.Forms.Button rename;
        private System.Windows.Forms.Button remove;
        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.TextBox option;
        private System.Windows.Forms.Label label1;
    }
}