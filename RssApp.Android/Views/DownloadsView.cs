using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RSS;

namespace RssApp.Android.Views
{
    class DownloadsView : ItemsView
    {
        public DownloadsView()
        {
            Initialize();
        }

        protected override void Initialize()
        {   
            Item.AnyDownloadComplete += Item_AnyDownloadComplete;
            Item.AnyDownloadRemoved += Item_AnyDownloadRemoved;
            scrollView.Content = stackLayout;
            Children.Add(scrollView);
        }

        private void Item_AnyDownloadRemoved(Item item)
        {
            RemoveItemControls(item);
        }


        protected void RemoveItemControls(Item item)
        {
            if (ItemControls.ContainsKey(item))
            {
                var itemLayout = ItemControls[item]["ItemLayout"];
                ItemControls.Remove(item);
                if (stackLayout.Children.Contains(itemLayout))
                {
                    stackLayout.Children.Remove(itemLayout);
                }
            }
        }

        private void Item_AnyDownloadComplete(Item item)
        {
            InsertItem(item, 0);
        }
    }
}