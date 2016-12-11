using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PodHead;

namespace PodHead.Android.Views
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
            if (ItemControls.ContainsKey(item) && ItemControls[item].ContainsKey(itemLayout))
            {
                var itemLayoutControl = ItemControls[item][itemLayout];
                Dictionary<string, Xamarin.Forms.View> view;
                ItemControls.TryRemove(item, out view);
                if (stackLayout.Children.Contains(itemLayoutControl))
                {
                    stackLayout.Children.Remove(itemLayoutControl);
                }
            }
        }

        private void Item_AnyDownloadComplete(Item item)
        {
            InsertItem(item, 0);
        }
    }
}