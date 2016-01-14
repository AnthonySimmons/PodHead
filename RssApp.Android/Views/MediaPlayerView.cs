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
using System.Timers;

namespace RssApp.Android.Views
{
    class MediaPlayerView : Xamarin.Forms.StackLayout
    {
        MediaPlayer mediaPlayer;
        Timer timer = new Timer();

        Xamarin.Forms.Button playPauseButton = new Xamarin.Forms.Button();
        Xamarin.Forms.Button fastFowardButton = new Xamarin.Forms.Button();
        Xamarin.Forms.Button rewindButton = new Xamarin.Forms.Button();
        Xamarin.Forms.Slider progressSlider = new Xamarin.Forms.Slider();
        Xamarin.Forms.Label progressLabel = new Xamarin.Forms.Label();
        Xamarin.Forms.Label durationLabel = new Xamarin.Forms.Label();
        Xamarin.Forms.Label titleLabel = new Xamarin.Forms.Label();
        Xamarin.Forms.Label descriptionLabel = new Xamarin.Forms.Label();
        Xamarin.Forms.Label dateLabel = new Xamarin.Forms.Label();
        Xamarin.Forms.Label errorLabel = new Xamarin.Forms.Label();
        Xamarin.Forms.Image image = new Xamarin.Forms.Image();

        int currentProgressMs;
        int progressStepMs = 30000;

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

            descriptionLabel.TextColor = Color.Black;
            descriptionLabel.IsVisible = false;

            fastFowardButton.TextColor = Color.Black;
            fastFowardButton.IsVisible = false;
            fastFowardButton.Text = "Fast Forward";
            fastFowardButton.Clicked += FastFowardButton_Clicked;

            rewindButton.TextColor = Color.Black;
            rewindButton.IsVisible = false;
            rewindButton.Text = "Rewind";
            rewindButton.Clicked += RewindButton_Clicked;

            var hLayout = new Xamarin.Forms.StackLayout()
            {
                Orientation = Xamarin.Forms.StackOrientation.Horizontal,
            };

            hLayout.Children.Add(progressLabel);
            hLayout.Children.Add(durationLabel);
            
            dateLabel.TextColor = Color.Black;
            dateLabel.IsVisible = false;

            var playLayout = new Xamarin.Forms.StackLayout()
            {
                Orientation = Xamarin.Forms.StackOrientation.Horizontal
            };
            playLayout.Children.Add(rewindButton);
            playLayout.Children.Add(playPauseButton);
            playLayout.Children.Add(fastFowardButton);

            image.HeightRequest = Height / 2;
            image.WidthRequest = Width / 2;

            errorLabel.TextColor = Color.Black;
            errorLabel.IsVisible = false;

            Children.Add(titleLabel);
            Children.Add(descriptionLabel);
            Children.Add(dateLabel);
            Children.Add(image);
            Children.Add(progressSlider);
            Children.Add(playLayout);
            Children.Add(hLayout);
            Children.Add(errorLabel);
        }

        private void RewindButton_Clicked(object sender, EventArgs e)
        {
            currentProgressMs -= progressStepMs;
            mediaPlayer.SeekTo(currentProgressMs);
            UpdateProgressTime();
            SetMediaProgress();
        }

        private void FastFowardButton_Clicked(object sender, EventArgs e)
        {
            currentProgressMs += progressStepMs;
            mediaPlayer.SeekTo(currentProgressMs);
            UpdateProgressTime();
            SetMediaProgress();
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
            currentProgressMs = (int)(mediaPlayer.Duration * perc);
            
            mediaPlayer.SeekTo(currentProgressMs);

            UpdateProgressTime();
        }

        private void UpdateProgressTime()
        {
            int progressSec = currentProgressMs / 1000;
            progressLabel.Text = GetTimeFormat(progressSec);
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
            try
            {
                currentProgressMs = 0;

                string uri = item.Link;
                if (item.CheckIsDownloaded())
                {
                    uri = item.FilePath;
                }

                mediaPlayer = MediaPlayer.Create(MainActivity.ActivityContext, Uri.Parse(uri));
                if (mediaPlayer == null)
                {
                    mediaPlayer = MediaPlayer.Create(MainActivity.ActivityContext, Uri.Parse(item.Link));
                }

                titleLabel.Text = item.Title;
                mediaPlayer.SetAudioStreamType(Stream.Music);

                image.MinimumHeightRequest = Height / 2;
                image.MinimumWidthRequest = Width / 2;
                image.HeightRequest = Height / 2;
                image.WidthRequest = Width / 2;
                image.Source = item.ParentSubscription.ImageUrl;

                durationLabel.Text = GetTimeFormat(mediaPlayer.Duration / 1000);
                descriptionLabel.Text = item.Description;
                dateLabel.Text = item.PubDate;

                timer.Interval = 1000;
                timer.Elapsed += Timer_Elapsed;

                rewindButton.IsVisible = true;
                fastFowardButton.IsVisible = true;
                dateLabel.IsVisible = true;
                progressLabel.IsVisible = true;
                playPauseButton.IsVisible = true;
                progressSlider.IsVisible = true;
                durationLabel.IsVisible = true;
                titleLabel.IsVisible = true;
                descriptionLabel.IsVisible = true;
                image.IsVisible = true;
            }
            catch(Exception ex)
            {
                DisplayError(ex.Message);
            }
        }

        private void DisplayError(string errorMessage)
        {
            errorLabel.IsVisible = true;
            errorLabel.Text = errorMessage;
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            currentProgressMs += 1000;
            UpdateProgressTime();
        }

        public void Play()
        {
            if (mediaPlayer != null)
            {
                timer.Start();
                mediaPlayer.Start();
                mediaPlayer.SeekTo(currentProgressMs);
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
                timer.Stop();
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