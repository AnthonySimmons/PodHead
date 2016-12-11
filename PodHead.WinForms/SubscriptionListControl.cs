using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PodHead;

namespace PodHeadForms
{
    public enum SubscriptionState { Subscription, TopCharts, SearchResults, }

    public delegate void SubscriptionEvent(Subscription subscription);

    public partial class SubscriptionListControl : UserControl
    {
        private Subscription _selectedSubscription;

        private SubscriptionState _subscriptionState;

        private const int ButtonSize = 25;

        private const int ImageSize = 50;

        private static int PaddingSize => (50 - 25) / 2;

        private const int ToggleColumn = 0;

        private const int PictureColumn = 1;

        private const int TitleColumn = 2;

        private const string Subscribe = @"Subscribe";

        private const string Unsubscribe = @"Unsubscribe";

        public event EventHandler LoadMoreEventHandler;

        public event SubscriptionEvent SubscriptionSelectedEventHandler;

        public event SubscriptionEvent SubscriptionRemovedEventHandler;

        public event EventHandler SubscriptionsLoadComplete;

        private ToolTip _toolTipSubscribe;

        private const int SubscribeMenuItemIndex = 1;

        private List<Subscription> _subscriptions;

        public string SelectedSubscriptionTitle { get; set; }
        
        public SubscriptionListControl()
        {
            InitializeComponent();
            _toolTipSubscribe = new ToolTip();
        }

        private ContextMenuStrip GetContextMenuStrip(Subscription sub)
        {
            var contextMenuStrip = new ContextMenuStrip();

            var viewToolStripItem = new ToolStripMenuItem("View", Properties.Resources.Next, ViewSubscriptionClicked) { Tag = sub };
            var subscribeToolStripItem = new ToolStripMenuItem(GetSubscriptionState(sub), GetSubscriptionMenuImage(sub), ContextMenuSubscribed) { Tag = sub };

            contextMenuStrip.Items.Add(viewToolStripItem);
            contextMenuStrip.Items.Add(subscribeToolStripItem);

            tableLayoutPanel1.ContextMenuStrip = contextMenuStrip;
            return contextMenuStrip;
        }

        private void ViewSubscriptionClicked(object sender, EventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem)sender;
            Subscription sub = (Subscription)item.Tag;
            OnSubscriptionSelected(sub);
        }
        
        private Subscription GetSubscriptionFromSender(object sender)
        {
            Control control = (Control)sender;
            return (Subscription)control.Tag;
        }

        private void OnSubscriptionSelected(Subscription subscription)
        {
            _selectedSubscription = subscription;
            SubscriptionSelectedEventHandler?.Invoke(subscription);
        }

        private void OnSubscriptionsLoadComplete()
        {
            SubscriptionsLoadComplete?.Invoke(this, null);
        }

        public void AddEmptySubscriptionsNote()
        {
            if (_subscriptionState == SubscriptionState.Subscription)
            {
                Label noSubsLabel = new Label()
                {
                    Text = "No Subscriptions",
                    Padding = new Padding(PaddingSize),
                    Dock = DockStyle.Fill,
                    Font = new Font(FontFamily.GenericSansSerif, 14f, FontStyle.Bold),
                };
                tableLayoutPanel1.Controls.Add(noSubsLabel, TitleColumn, 0);
            }
        }

        private void AddLoadMoreButton()
        {
            if(_subscriptionState == SubscriptionState.SearchResults 
            || _subscriptionState == SubscriptionState.TopCharts)
            {
                Button loadMoreButton = GetLoadMoreButton();
                tableLayoutPanel1.Controls.Add(loadMoreButton, TitleColumn, tableLayoutPanel1.RowCount);
            }
        }

        private Button GetLoadMoreButton()
        {
            var btn = new Button()
            {
                AutoSize = true,
                Text = @"Load More",
                FlatStyle = FlatStyle.Flat,
                Font = new Font(FontFamily.GenericSansSerif, 14f, FontStyle.Bold),
            };
            btn.Click += BtnLoadMore_Click;
            return btn;
        }

        private void BtnLoadMore_Click(object sender, EventArgs e)
        {
            OnLoadMore();
        }

        private void OnLoadMore()
        {
            LoadMoreEventHandler?.Invoke(this, new EventArgs());
        }
        

        public void LoadSubscriptions(List<Subscription> subscriptions, SubscriptionState subscriptionState = SubscriptionState.Subscription)
        {
            tableLayoutPanel1.RowCount = 0;
            _subscriptions = subscriptions;
            _subscriptionState = subscriptionState;
            tableLayoutPanel1.SuspendLayout();
            tableLayoutPanel1.Controls.Clear();

            if (subscriptions.Any())
            {
                int i = 0;
                foreach (var sub in subscriptions)
                {
                    AddSubscriptionRow(sub, i);
                    i++;
                }
                SelectSubscription();
                AddLoadMoreButton();
                AddEmptySubscriptionRow();                
            }
            else
            {
                AddEmptySubscriptionsNote();
            }

            tableLayoutPanel1.ResumeLayout();
            OnSubscriptionsLoadComplete();
            Refresh();
        }

        public void Clear()
        {
            tableLayoutPanel1.SuspendLayout();
            tableLayoutPanel1.RowCount = 0;
            tableLayoutPanel1.Controls.Clear();
            tableLayoutPanel1.ResumeLayout();
        }

        public void SelectSubscription(Subscription sub)
        {
            _selectedSubscription = sub;
            SelectSubscription();
        }

        private void SelectSubscription()
        {
            if (_selectedSubscription == null)
            {
                _selectedSubscription = _subscriptions.FirstOrDefault();
            }

            if (_selectedSubscription != null)
            {
                int row = GetSubscriptionRow(_selectedSubscription.Title);
                if (row >= 0)
                {
                    SetDefaultControlColor();
                    Label subLabel = (Label)tableLayoutPanel1.GetControlFromPosition(TitleColumn, row);
                    subLabel.BackColor = Color.Green;
                    OnSubscriptionSelected(_selectedSubscription);
                }
            }
        }

        private void AddSubscriptionRow(Subscription subscription, int row)
        {
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.Controls.Add(GetCheckbox(subscription), ToggleColumn, row);
            tableLayoutPanel1.Controls.Add(GetSubscriptionImage(subscription), PictureColumn, row);
            tableLayoutPanel1.Controls.Add(GetSubscriptionLabel(subscription), TitleColumn, row);
            tableLayoutPanel1.RowCount++;
        }

        private void AddEmptySubscriptionRow()
        {
            var subscription = new Subscription();
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.Controls.Add(GetCheckbox(subscription));
            tableLayoutPanel1.Controls.Add(GetSubscriptionImage(subscription));
            tableLayoutPanel1.Controls.Add(GetSubscriptionLabel(subscription));
        }
        
        private CheckBox GetCheckbox(Subscription sub)
        {
            CheckBox cbx = null;
            if (!string.IsNullOrEmpty(sub.Title))
            {
                cbx = new CheckBox()
                {
                    Tag = sub,
                    FlatStyle = FlatStyle.Flat,
                    Padding = new Padding(PaddingSize),
                    Height = ButtonSize,
                    Width = ButtonSize,
                    ImageAlign = ContentAlignment.MiddleCenter,
                    Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right,
                    Checked = Feeds.Instance.ContainsSubscription(sub.Title),
                    ContextMenuStrip = GetContextMenuStrip(sub),
                };
                _toolTipSubscribe.SetToolTip(cbx, GetSubscriptionState(sub));
                cbx.CheckedChanged += Cbx_CheckedChanged;
            }
            return cbx;
        }

        private void Cbx_CheckedChanged(object sender, EventArgs e)
        {
            var control = (Control)sender;
            var sub = (Subscription)control.Tag;

            string subState = GetSubscriptionState(sub);

            control.ContextMenuStrip.Items[SubscribeMenuItemIndex].Text = subState;
            _toolTipSubscribe.SetToolTip(control, subState);

            ToggleSubscription(sub);
        }

        private void ContextMenuSubscribed(object sender, EventArgs e)
        {
            var control = (ToolStripMenuItem)sender;
            var sub = (Subscription)control.Tag;
            control.Text = GetSubscriptionState(sub);

            int row = GetSubscriptionRow(sub.Title);
            if (row >= 0)
            {
                CheckBox cbx = (CheckBox)tableLayoutPanel1.GetControlFromPosition(ToggleColumn, row);
                cbx.Checked = Feeds.Instance.ContainsSubscription(sub.Title);
                ToggleSubscription(sub);
            }

        }

        private void ToggleSubscription(Subscription sub)
        { 
            Feeds.Instance.ToggleSubscription(sub);
            
            if(_subscriptionState == SubscriptionState.Subscription)
            {
                LoadSubscriptions(Feeds.Instance.Subscriptions, SubscriptionState.Subscription);
            }
        }
        
        private Image GetSubscriptionMenuImage(Subscription sub)
        {
            var image = Properties.Resources.Remove;
            if (!Feeds.Instance.ContainsSubscription(sub?.Title))
            {
                image = Properties.Resources.Subscribe;
            }
            return image;
        }

        private string GetSubscriptionState(Subscription sub)
        {
            var subState = "Unsubscribe";
            if(!Feeds.Instance.ContainsSubscription(sub?.Title))
            {
                subState = "Subscribe";
            }
            return subState;
        }

        private int GetSubscriptionRow(string subscriptionTitle)
        {
            int row = -1;
            for(int i = 0; i < tableLayoutPanel1.RowCount; i++)
            {
                Label lbl = (Label)tableLayoutPanel1.GetControlFromPosition(TitleColumn, i);
                if(lbl?.Text == subscriptionTitle)
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
            pbx.MouseDown += Pbx_Click;
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
                OnSubscriptionSelected(sub);
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
            lbl.MouseDown += Lbl_Click;
            return lbl;
        }

        private void Lbl_Click(object sender, EventArgs e)
        {
            var control = (Control)sender;
            SetDefaultControlColor();
            control.BackColor = Color.Green;
            var sub = (Subscription)control.Tag;
            OnSubscriptionSelected(sub);
        }
        
    }
}
