using System;
using Android.Content;

namespace PodHead.Android.Utilities
{
    internal class NetworkChangeBroadcastReceiver : BroadcastReceiver
    {
        public event EventHandler NetworkChangeEvent;

        public override void OnReceive(Context context, Intent intent)
        {
            OnNetworkChange();
        }

        protected virtual void OnNetworkChange()
        {
            EventHandler copy = NetworkChangeEvent;
            if(copy != null)
            {
                copy.Invoke(this, EventArgs.Empty);
            }
        }
    }
}