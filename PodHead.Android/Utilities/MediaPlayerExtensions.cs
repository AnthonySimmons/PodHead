

using Android.Media;

namespace PodHead.Android.Utilities
{
    public static class MediaPlayerExtensions
    {
        public static double GetPercentage(this MediaPlayer mediaPlayer)
        {
            return (double)mediaPlayer.CurrentPosition / (double)mediaPlayer.Duration;
        }
    }
}