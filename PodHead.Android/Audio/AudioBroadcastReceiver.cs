using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace PodHead.Android.Audio
{
    [BroadcastReceiver]
    public class AudioBroadcastReceiver : BroadcastReceiver
    {
        public string ComponentName => Class.Name;

        private static int _headSetHookCount;

        public override void OnReceive(Context context, Intent intent)
        {
            if(intent.Action != Intent.ActionMediaButton)
            {
                return;
            }

            KeyEvent keyEvent = (KeyEvent)intent.GetParcelableExtra(Intent.ExtraKeyEvent);
            switch(keyEvent.KeyCode)
            {
                case Keycode.MediaPlay:
                case Keycode.MediaPlayPause:
                    MainActivity.ActivityContext.OnPlayPauseEvent();
                    break;
                case Keycode.Headsethook:
                    _headSetHookCount++;
                    if (_headSetHookCount == 2)
                    {
                        _headSetHookCount = 0;
                        MainActivity.ActivityContext.OnPlayPauseEvent();
                    }
                    break;
                case Keycode.MediaNext:
                    break;
                case Keycode.MediaPrevious:
                    break;
                default:
                    break;
            }
        }
    }
}