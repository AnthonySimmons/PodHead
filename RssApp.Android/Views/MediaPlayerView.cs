using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Net;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Media;
using Uri = Android.Net.Uri;
using Color = Xamarin.Forms.Color;
using RSS;

namespace RssApp.Android.Views
{
    class MediaPlayerView : Xamarin.Forms.StackLayout
    {
        MediaPlayer mediaPlayer;

        Xamarin.Forms.Button playPauseButton = new Xamarin.Forms.Button();
        Xamarin.Forms.Slider progressSlider = new Xamarin.Forms.Slider();
        Xamarin.Forms.Label progressLabel = new Xamarin.Forms.Label();
        Xamarin.Forms.Label durationLabel = new Xamarin.Forms.Label();
        Xamarin.Forms.Label titleLabel = new Xamarin.Forms.Label();

        

        public MediaPlayerView()
        {
            Initialize();
        }

        private void Initialize()
        {
            playPauseButton.Text = "Play";
            playPauseButton.Clicked += PlayPauseButton_Clicked;
            playPauseButton.IsVisible = false;

            progressSlider.ValueChanged += ProgressSlider_ValueChanged;
            progressSlider.BackgroundColor = Color.Navy;
            progressSlider.IsVisible = false;

            titleLabel.TextColor = Color.Black;
            titleLabel.IsVisible = false;

            playPauseButton.TextColor = Color.Black;
            playPauseButton.IsVisible = false;

            progressLabel.TextColor = Color.Black;
            progressLabel.IsVisible = false;

            durationLabel.TextColor = Color.Black;
            durationLabel.IsVisible = false;

            Children.Add(titleLabel);
            Children.Add(playPauseButton);
            Children.Add(progressSlider);
            Children.Add(progressLabel);
            Children.Add(durationLabel);
        }

        private void ProgressSlider_ValueChanged(object sender, Xamarin.Forms.ValueChangedEventArgs e)
        {
            if (mediaPlayer != null)
            {
                SetProgressTime();
            }
        }

        private void SetProgressTime()
        {
            double perc = progressSlider.Value;
            int valMs = (int)(mediaPlayer.Duration * perc);
            
            mediaPlayer.SeekTo(valMs);
            progressLabel.Text = GetTimeFormat(valMs / 1000);
        }

        private string GetTimeFormat(int durationSeconds)
        {
            int min = (durationSeconds / 60);
            int sec = (durationSeconds % 60);
            int hour = min / 60;

            string minStr = min.ToString();
            string secStr = sec.ToString();
            string hourStr = hour.ToString();

            if(min < 10)
            {
                minStr = string.Format("0{0}", minStr);
            }
            if(sec < 10)
            {
                secStr = string.Format("0{0}", secStr);
            }
            
            return string.Format("{0}:{1}:{2}", hourStr, minStr, secStr);
        }

        private void PlayPauseButton_Clicked(object sender, EventArgs e)
        {
            TogglePlay();
        }

        private void StopButton_Clicked(object sender, EventArgs e)
        {
            Stop();
        }

        public void TogglePlay()
        {
            if(mediaPlayer != null)
            {
                if(mediaPlayer.IsPlaying)
                {
                    Pause();
                }
                else
                {
                    Play();
                }
            }
        }

        public void LoadPlayer(Item item)
        {
            mediaPlayer = MediaPlayer.Create(MainActivity.ActivityContext, Uri.Parse(item.Link));
            titleLabel.Text = item.Title;
            mediaPlayer.SetAudioStreamType(Stream.Music);

            durationLabel.Text = GetTimeFormat(mediaPlayer.Duration / 1000);
            progressLabel.IsVisible = true;
            playPauseButton.IsVisible = true;
            progressSlider.IsVisible = true;
            durationLabel.IsVisible = true;
            titleLabel.IsVisible = true;
        }


        public void Play()
        {
            if (mediaPlayer != null)
            {
                mediaPlayer.Start();
                SetMediaProgress();
                playPauseButton.Text = "Pause";
            }
        }

        private void SetMediaProgress()
        {
            progressSlider.Value = mediaPlayer.CurrentPosition / mediaPlayer.Duration;
        }

        public void Pause()
        {
            if(mediaPlayer != null)
            {
                mediaPlayer.Pause();
                playPauseButton.Text = "Play";
            }
        }

        public void Stop()
        {
            if (mediaPlayer != null)
            {
                mediaPlayer.Stop();
                playPauseButton.Text = "Play";
            }
        }
    }
}