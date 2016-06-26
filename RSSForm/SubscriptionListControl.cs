using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using RSS;

namespace RSSForm
{
    public delegate void SubscriptionSelectedEvent(string subscriptionName);

    public partial class SubscriptionListControl : UserControl
    {
        private const int ButtonSize = 25;

        private const int ImageSize = 50;

        private static int PaddingSize => (50 - 25) / 2;

        private const int ToggleColumn = 0;

        private const int PictureColumn = 1;

        private const int TitleColumn = 2;

        public event SubscriptionSelectedEvent SubscriptionSelectedEventHandler;

        public event EventHandler SubscriptionsLoadComplete;

        public string SelectedSubscriptionTitle { get; set; }
        
        public SubscriptionListControl()
        {
            InitializeComponent();
        }

        private ContextMenuStrip GetContextMenuStrip(object tag)
        {
            var contextMenuStrip = new ContextMenuStrip();

            var viewToolStripItem = new ToolStripMenuItem("View", null, ViewSubscriptionClicked) { Tag = tag };
            var subscribeToolStripItem = new ToolStripMenuItem("Subscribe", null, ToggleSubscriptionClicked) { Tag = tag };

            contextMenuStrip.Items.Add(viewToolStripItem);
            contextMenuStrip.Items.Add(subscribeToolStripItem);

            tableLayoutPanel1.ContextMenuStrip = contextMenuStrip;
            return contextMenuStrip;
        }

        private void ViewSubscriptionClicked(object sender, EventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem)sender;
            Subscription sub = (Subscription)item.Tag;
            OnSubscriptionSelected(sub.Title);
        }

        private void ToggleSubscriptionClicked(object sender, EventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem)sender;
            Subscription sub = (Subscription)item.Tag;
            
            Feeds.Instance.ToggleSubscription(sub);

            if (!Feeds.Instance.ContainsSubscription(sub.Title))
            {
                RemoveRow((Control)sender);
            }
        }

        private Subscription GetSubscriptionFromSender(object sender)
        {
            Control control = (Control)sender;
            return (Subscription)control.Tag;
        }

        private void OnSubscriptionSelected(string subscriptionName)
        {
            SubscriptionSelectedEventHandler?.Invoke(subscriptionName);
        }

        private void OnSubscriptionsLoadComplete()
        {
            SubscriptionsLoadComplete?.Invoke(this, null);
        }

        public void LoadSubscriptions(List<Subscription> subscriptions)
        {
            tableLayoutPanel1.SuspendLayout();
            tableLayoutPanel1.Controls.Clear();
            int i = 0;
            foreach (var sub in subscriptions)
            {
                AddSubscriptionRow(sub, i);
                i++;
            }
            AddEmptySubscriptionRow();
            
            tableLayoutPanel1.ResumeLayout();
            OnSubscriptionsLoadComplete();
        }

        private void AddSubscriptionRow(Subscription subscription, int row)
        {
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.Controls.Add(GetToggleButton(subscription), ToggleColumn, row);
            tableLayoutPanel1.Controls.Add(GetSubscriptionImage(subscription), PictureColumn, row);
            tableLayoutPanel1.Controls.Add(GetSubscriptionLabel(subscription), TitleColumn, row);
        }

        private void AddEmptySubscriptionRow()
        {
            var subscription = new Subscription();
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.Controls.Add(GetToggleButton(subscription));
            tableLayoutPanel1.Controls.Add(GetSubscriptionImage(subscription));
            tableLayoutPanel1.Controls.Add(GetSubscriptionLabel(subscription));
        }
        
        private Button GetToggleButton(Subscription sub)
        {
            Button btn = null;
            if (!string.IsNullOrEmpty(sub.Title))
            {
                btn = new Button()
                {
                    Tag = sub,
                    Image = Properties.Resources.deleteIcon,
                    FlatStyle = FlatStyle.Flat,
                    Height = ButtonSize,
                    Width = ButtonSize,
                    BackgroundImageLayout = ImageLayout.Stretch,
                    Padding = new Padding(PaddingSize),
                    ImageAlign = ContentAlignment.MiddleCenter,
                    Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right,
                };

                btn.Click += BtnToggle_Click;
            }
            return btn;
        }

        private void BtnToggle_Click(object sender, EventArgs e)
        {
            Subscription sub = GetSubscriptionFromSender(sender);
            Feeds.Instance.ToggleSubscription(sub);

            if (!Feeds.Instance.ContainsSubscription(sub.Title))
            {
                int row = GetSubscriptionRow(sub.Title);
                RemoveRow(row);
            }
        }
        
        private int GetSubscriptionRow(string subscriptionTitle)
        {
            int row = -1;
            for(int i = 0; i < tableLayoutPanel1.RowCount; i++)
            {
                Label lbl = (Label)tableLayoutPanel1.GetControlFromPosition(TitleColumn, i);
                if(lbl.Text == subscriptionTitle)
                {
                    row = i;
                    break;
                }
            }
            return row;
        }


        private PictureBox GetSubscriptionImage(Subscription sub)
        {
            var pbx = new PictureBox()
            {
                ImageLocation = sub.ImageUrl,
                Height = ButtonSize * 2,
                Width = ButtonSize * 2,
                BackgroundImageLayout = ImageLayout.Center,
                Tag = sub,
                SizeMode = PictureBoxSizeMode.StretchImage,
                ContextMenuStrip = GetContextMenuStrip(sub),
            };
            pbx.Click += Pbx_Click;
            return pbx;
        }

        private void Pbx_Click(object sender, EventArgs e)
        {
            var control = (Control)sender;

            SetDefaultControlColor();
            int row = tableLayoutPanel1.GetRow(control);
            if (row >= 0)
            {
                var titleControl = tableLayoutPanel1.GetControlFromPosition(TitleColumn, row);
                if (titleControl != null)
                {
                    titleControl.BackColor = Color.Green;
                }

                var sub = (Subscription)control.Tag;
                OnSubscriptionSelected(sub.Title);
            }
        }

        private void SetDefaultControlColor()
        {
            foreach(Control control in tableLayoutPanel1.Controls)
            {
                control.BackColor = Color.Transparent;
            }
        }

        private Label GetSubscriptionLabel(Subscription sub)
        {
            var lbl = new Label()
            {
                Text = sub.Title,
                Font = new Font(FontFamily.GenericSansSerif, 10f, FontStyle.Italic),
                AutoSize = true,
                Tag = sub,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right,
                ContextMenuStrip = GetContextMenuStrip(sub),
            };
            lbl.Click += Lbl_Click;
            return lbl;
        }

        private void Lbl_Click(object sender, EventArgs e)
        {
            var control = (Control)sender;
            SetDefaultControlColor();
            control.BackColor = Color.Green;
            var sub = (Subscription)control.Tag;
            OnSubscriptionSelected(sub.Title);
        }

        private void RemoveRow(Control controlInRow)
        {
            int row = tableLayoutPanel1.GetCellPosition(controlInRow).Row;
            RemoveRow(row);
        }

        private void RemoveRow(int row)
        {
            if (row >= 0)
            {
                tableLayoutPanel1.SuspendLayout();
                for (int i = 0; i < 3; i++)
                {
                    var control = tableLayoutPanel1.GetControlFromPosition(i, row);
                    tableLayoutPanel1.Controls.Remove(control);
                }

                for(int r = row+1; r < tableLayoutPanel1.RowCount; r++)
                {
                    for(int c = 0; c < 3; c++)
                    {
                        var control = tableLayoutPanel1.GetControlFromPosition(c, r);
                        if (control != null)
                        {
                            tableLayoutPanel1.SetRow(control, r);
                        }
                    }
                }

                tableLayoutPanel1.RowCount--;

                tableLayoutPanel1.ResumeLayout();
                tableLayoutPanel1.Refresh();
                Application.DoEvents();
            }
        }
        
    }
}
