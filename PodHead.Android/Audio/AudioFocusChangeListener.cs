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
        public event Action<AudioFocus> AudioFocusChange;

        public void OnAudioFocusChange([GeneratedEnum] AudioFocus focusChange)
        {
            AudioFocusChange?.Invoke(focusChange);
        }
    }
}