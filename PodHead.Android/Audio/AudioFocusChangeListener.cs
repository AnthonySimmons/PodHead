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
using static Android.Media.AudioManager;

namespace PodHead.Android.Audio
{
    internal class AudioFocusChangeListener : Java.Lang.Object, IOnAudioFocusChangeListener
    {
        public event Action PlayEvent;

        public event Action PauseEvent;

        public void OnAudioFocusChange([GeneratedEnum] AudioFocus focusChange)
        {
            switch(focusChange)
            {
                case AudioFocus.Gain:
                case AudioFocus.GainTransient:
                case AudioFocus.GainTransientExclusive:
                    PlayEvent?.Invoke();
                    break;
                case AudioFocus.Loss:
                case AudioFocus.LossTransient:
                    PauseEvent?.Invoke();
                    break;
            }
        }
    }
}