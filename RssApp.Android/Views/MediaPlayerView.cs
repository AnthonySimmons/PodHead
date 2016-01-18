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

using Button = Xamarin.Forms.Button;
using StackLayout = Xamarin.Forms.StackLayout;
using Image = Xamarin.Forms.Image;
using Label = Xamarin.Forms.Label;
using Slider = Xamarin.Forms.Slider;

using Device = Xamarin.Forms.Device;

namespace RssApp.Android.Views
{
    class MediaPlayerView : Xamarin.Forms.ScrollView
    {
        private StackLayout stackLayout = new StackLayout();

        MediaPlayer mediaPlayer;
        Timer timer = new Timer();

        Button playPauseButton = new Button();
        Button fastFowardButton = new Button();
        Button rewindButton = new Button();
        Slider progressSlider = new Slider();
        Label progressLabel = new Label();
        Label durationLabel = new Label();
        Label titleLabel = new Label();
        Label descriptionLabel = new Label();
        Label dateLabel = new Label();
        Label errorLabel = new Label();
        Image image = new Image();

        MediaController mediaController;

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
            titleLabel.FontSize = 24;
            titleLabel.HorizontalTextAlignment = Xamarin.Forms.TextAlignment.Center;

            playPauseButton.TextColor = Color.Black;
            playPauseButton.IsVisible = false;

            progressLabel.TextColor = Color.Black;
            progressLabel.IsVisible = false;
            progressLabel.FontSize = 18;

            durationLabel.TextColor = Color.Black;
            durationLabel.IsVisible = false;
            durationLabel.FontSize = 18;

            descriptionLabel.TextColor = Color.Black;
            descriptionLabel.IsVisible = false;
            descriptionLabel.FontSize = 20;

            fastFowardButton.TextColor = Color.Black;
            fastFowardButton.IsVisible = false;
            fastFowardButton.Text = "Fast Forward";
            fastFowardButton.Clicked += FastFowardButton_Clicked;

            rewindButton.TextColor = Color.Black;
            rewindButton.IsVisible = false;
            rewindButton.Text = "Rewind";
            rewindButton.Clicked += RewindButton_Clicked;

            /*
            var hLayout = new Xamarin.Forms.StackLayout()
            {
                Orientation = Xamarin.Forms.StackOrientation.Horizontal,
            };
            var spaceBox = new Xamarin.Forms.BoxView()
            {
                WidthRequest = Width / 2,
                HeightRequest = Width / 2,
            };
            hLayout.Children.Add(progressLabel);
            hLayout.Children.Add(spaceBox);
            hLayout.Children.Add(durationLabel);
            */
            dateLabel.TextColor = Color.Black;
            dateLabel.IsVisible = false;

            var playLayout = new StackLayout()
            {
                Orientation = Xamarin.Forms.StackOrientation.Horizontal
            };
            
            playLayout.Children.Add(rewindButton);
            playLayout.Children.Add(playPauseButton);
            playLayout.Children.Add(fastFowardButton);
            

            image.HeightRequest = Width / 2;
            image.WidthRequest = Width / 2;

            errorLabel.TextColor = Color.Black;
            errorLabel.IsVisible = false;

            stackLayout.Children.Add(titleLabel);
            stackLayout.Children.Add(descriptionLabel);
            stackLayout.Children.Add(dateLabel);
            stackLayout.Children.Add(image);
            stackLayout.Children.Add(progressSlider);
            stackLayout.Children.Add(playLayout);

            stackLayout.Children.Add(progressLabel);
            stackLayout.Children.Add(durationLabel);
            stackLayout.Children.Add(errorLabel);

            Content = stackLayout;

            //mediaController = new MediaController(MainActivity.ActivityContext);
            
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
            Device.BeginInvokeOnMainThread(() =>
            {
                int progressSec = currentProgressMs / 1000;
                progressLabel.Text = GetTimeFormat(progressSec);
            }
            );
        }

        private string GetTimeFormat(int durationSeconds)
        {
            int min = (durationSeconds / 60);
            int sec = (durationSeconds % 60);
            int hour = min / 60;
            min = min % 60;
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
                if (item.IsDownloaded)
                {
                    uri = item.FilePath;
                }

                mediaPlayer = MediaPlayer.Create(MainActivity.ActivityContext, Uri.Parse(uri));
                
                titleLabel.Text = item.Title;
                mediaPlayer.SetAudioStreamType(Stream.Music);
                //mediaPlayer.Prepare();
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
            progressSlider.ValueChanged -= ProgressSlider_ValueChanged;
            Device.BeginInvokeOnMainThread(() =>
            {
                progressSlider.Value = (double)mediaPlayer.CurrentPosition / (double)mediaPlayer.Duration;
            }
            );
            progressSlider.ValueChanged += ProgressSlider_ValueChanged;
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