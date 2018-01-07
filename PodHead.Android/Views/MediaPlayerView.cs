using System;
using System.Timers;
using Android.Media;
using PodHead.Android.Utilities;

using Uri = Android.Net.Uri;
using Color = Xamarin.Forms.Color;
using StackLayout = Xamarin.Forms.StackLayout;
using Image = Xamarin.Forms.Image;
using Label = Xamarin.Forms.Label;
using Slider = Xamarin.Forms.Slider;
using TapGestureRecognizer = Xamarin.Forms.TapGestureRecognizer;

using Device = Xamarin.Forms.Device;
using Xamarin.Forms;
using Java.IO;

namespace PodHead.Android.Views
{
    class MediaPlayerView : Xamarin.Forms.ScrollView
    {
        private StackLayout _stackLayout = new StackLayout();
        
        MediaPlayer _mediaPlayer;
        Timer _timer = new Timer();

        Image _playPauseButton = new Image();
        Image _fastFowardButton = new Image();
        Image _rewindButton = new Image();
        Slider _progressSlider = new Slider();
        Label _progressLabel = new Label();
        Label _durationLabel = new Label();
        Label _titleLabel = new Label();
        Label _descriptionLabel = new Label();
        Label _dateLabel = new Label();
        Label _errorLabel = new Label();
        Label _streamingLabel = new Label();
        Image _image = new Image();
        
        private static Item _nowPlaying;

        private const int ImageSize = 50;
        
        private int _currentProgressMs;
        private const int ProgressStepMs = 30000;

        private readonly ErrorLogger _errorLogger;
        private readonly Parser _parser;
        private readonly Feeds _feeds;

        public MediaPlayerView()
        {
            _errorLogger = ErrorLogger.Get(Config.Instance);
            _parser = Parser.Get(Config.Instance);
            _feeds = Feeds.Get(_parser, Config.Instance);
            _feeds.AllFeedsParsed += Feeds_AllFeedsParsed;
            
            Initialize();
        }

        private void Feeds_AllFeedsParsed(object sender, EventArgs e)
        {
            LoadNowPlaying();            
        }

        private void LoadNowPlaying()
        {
            if (_feeds.NowPlaying != null)
            {
                LoadPlayer(_feeds.NowPlaying);
            }
        }

        private void Initialize()
        {
            _timer.Interval = 1000;
            _timer.Elapsed += Timer_Elapsed;

            _playPauseButton.Source = "Play.png";
            _playPauseButton.IsVisible = false;
            _playPauseButton.HeightRequest = _playPauseButton.WidthRequest = ImageSize;
            _playPauseButton.GestureRecognizers.Add(new TapGestureRecognizer(sender => PlayPauseButton_Clicked(sender, null)));

            _progressSlider.ValueChanged += ProgressSlider_ValueChanged;
            _progressSlider.BackgroundColor = Color.Navy;
            _progressSlider.IsVisible = false;
            
            _titleLabel.TextColor = Color.Black;
            _titleLabel.IsVisible = false;
            _titleLabel.FontSize = 24;
            _titleLabel.HorizontalTextAlignment = TextAlignment.Center;
            
            _playPauseButton.IsVisible = false;
            _playPauseButton.HorizontalOptions = LayoutOptions.CenterAndExpand;

            _progressLabel.TextColor = Color.Black;
            _progressLabel.IsVisible = false;
            _progressLabel.FontSize = 18;
            _progressLabel.HorizontalOptions = LayoutOptions.StartAndExpand;

            _durationLabel.TextColor = Color.Black;
            _durationLabel.IsVisible = false;
            _durationLabel.FontSize = 18;
            _durationLabel.HorizontalOptions = LayoutOptions.EndAndExpand;

            _descriptionLabel.TextColor = Color.Black;
            _descriptionLabel.IsVisible = false;
            _descriptionLabel.FontSize = 16;
            
            _fastFowardButton.IsVisible = false;
            _fastFowardButton.Source = "FastForward.png";
            _fastFowardButton.HeightRequest = _fastFowardButton.WidthRequest = ImageSize;
            _fastFowardButton.GestureRecognizers.Add(new TapGestureRecognizer(sender => FastFowardButton_Clicked(sender, null)));
            _fastFowardButton.HorizontalOptions = LayoutOptions.Center;

            _rewindButton.IsVisible = false;
            _rewindButton.Source = "Rewind.png";
            _rewindButton.HeightRequest = _rewindButton.WidthRequest = ImageSize;
            _rewindButton.GestureRecognizers.Add(new TapGestureRecognizer(sender => RewindButton_Clicked(sender, null)));
            _rewindButton.HorizontalOptions = LayoutOptions.Center;
       
            _dateLabel.TextColor = Color.Black;
            _dateLabel.IsVisible = false;

            _streamingLabel.IsVisible = false;
            _streamingLabel.FontSize = 18;
            _streamingLabel.HorizontalTextAlignment = TextAlignment.Center;
            _streamingLabel.TextColor = Color.Red;
            _streamingLabel.Text = "Streaming";
            _streamingLabel.HorizontalOptions = LayoutOptions.Center;

            var playLayout = new StackLayout()
            {
                Orientation = StackOrientation.Horizontal,
            };

            playLayout.Children.Add(_progressLabel);
            playLayout.Children.Add(_rewindButton);
            playLayout.Children.Add(_playPauseButton);
            playLayout.Children.Add(_fastFowardButton);
            playLayout.Children.Add(_durationLabel);

            playLayout.WidthRequest = Width;

            _image.MinimumHeightRequest = Height / 2;
            _image.MinimumWidthRequest = Width / 2;
            _image.HeightRequest = Height / 2;
            _image.WidthRequest = Width / 2;

            _errorLabel.TextColor = Color.Black;
            _errorLabel.IsVisible = false;
            
            _stackLayout.Children.Add(_titleLabel);
            _stackLayout.Children.Add(_descriptionLabel);
            _stackLayout.Children.Add(_dateLabel);
            _stackLayout.Children.Add(_streamingLabel);
            _stackLayout.Children.Add(_image);
            _stackLayout.Children.Add(_progressSlider);
            
            _stackLayout.Children.Add(playLayout);

            _stackLayout.Children.Add(_errorLabel);

            Content = _stackLayout;

            InitNowPlaying();
        }

        private void InitNowPlaying()
        {
            if(_nowPlaying != null)
            {
                LoadPlayer(_nowPlaying);
                SetNowPlaying(_nowPlaying);
            }
        }

        private void RewindButton_Clicked(object sender, EventArgs e)
        {
            _currentProgressMs -= ProgressStepMs;
            _mediaPlayer.SeekTo(_currentProgressMs);
            
            UpdateProgressTime();
            SetMediaProgress();
        }
        
        private void FastFowardButton_Clicked(object sender, EventArgs e)
        {
            _currentProgressMs += ProgressStepMs;
            _mediaPlayer.SeekTo(_currentProgressMs);

            UpdateProgressTime();
            SetMediaProgress();
        }

        private void ProgressSlider_ValueChanged(object sender, Xamarin.Forms.ValueChangedEventArgs e)
        {
            if (_mediaPlayer != null)
            {
                SetProgressTime();
            }
        }

        private void SetProgressTime()
        {
            double perc = _progressSlider.Value;
            _nowPlaying.PercentPlayed = perc;
            _currentProgressMs = (int)(_mediaPlayer.Duration * perc);
            
            _mediaPlayer.SeekTo(_currentProgressMs);

            UpdateProgressTime();
        }

        private void UpdateProgressTime()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                int progressSec = _currentProgressMs / 1000;
                _progressLabel.Text = GetTimeFormat(progressSec);
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
            if(_mediaPlayer != null)
            {
                if(_mediaPlayer.IsPlaying)
                {
                    Pause();
                }
                else
                {
                    Play();
                }
            }
        }

        private void SetNowPlaying(Item item)
        {
            if(_nowPlaying != null)
            {
                _nowPlaying.IsPlaying = false;
            }

            _nowPlaying = item;
            _nowPlaying.IsPlaying = true;

            _progressSlider.Value = item.PercentPlayed / 100.0;
        }
        
        public void LoadPlayer(Item item)
        {
            try
            {                
                _titleLabel.Text = item.Title;

                _image.Source = item.ParentSubscription.ImageUrl;
                
                _image.HeightRequest = Height / 2;
                _image.WidthRequest = Width / 2;


                _descriptionLabel.Text = item.Description;
                _dateLabel.Text = item.PubDate;
                
                _rewindButton.IsVisible = true;
                _fastFowardButton.IsVisible = true;
                _dateLabel.IsVisible = true;
                _progressLabel.IsVisible = true;
                _playPauseButton.IsVisible = true;
                _progressSlider.IsVisible = true;
                _durationLabel.IsVisible = true;
                _titleLabel.IsVisible = true;
                _descriptionLabel.IsVisible = true;
                _image.IsVisible = true;

                _streamingLabel.IsVisible = !item.IsDownloaded;

                _feeds.NowPlaying = item;
                
                SetMediaPlayer(item);
            }
            catch(Exception ex)
            {
                _errorLogger.Log(ex);
                DisplayError(ex.Message);
            }
        }

        private void SetMediaPlayer(Item item)
        {
            Uri uri = Uri.Parse(item.Link);
            if (item.IsDownloaded)
            {
                uri = Uri.FromFile(new File(item.FilePath));
            }

            Stop();
            
            _mediaPlayer = MediaPlayer.Create(MainActivity.ActivityContext, uri);

            if (_mediaPlayer == null)
            {
                throw new InvalidOperationException("Cannot play media. URI: " + uri);
            }

            _mediaPlayer.SetAudioStreamType(Stream.Music);
            SetNowPlaying(item);

            _durationLabel.Text = GetTimeFormat(_mediaPlayer.Duration / 1000);
        }

        private void DisplayError(string errorMessage)
        {
            _errorLabel.IsVisible = true;
            _errorLabel.Text = errorMessage;
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _currentProgressMs += 1000;
            SetNowPlayingPercentPlayed();
            UpdateProgressTime();
        }

        public void Play()
        {
            if (_mediaPlayer != null && _nowPlaying != null)
            {                
                _nowPlaying.Position = 0;
                _timer.Start();
                _mediaPlayer.Start();
                _mediaPlayer.SeekTo(_currentProgressMs);
                SetMediaProgress();
                _nowPlaying.Duration = _mediaPlayer.Duration;
                SetNowPlayingPercentPlayed();
                _playPauseButton.Source = "Pause.png";
                _nowPlaying.IsPlaying = true;
            }
        }

        private void SetMediaProgress()
        {
            _progressSlider.ValueChanged -= ProgressSlider_ValueChanged;
            Device.BeginInvokeOnMainThread(() =>
            {
                _progressSlider.Value = _mediaPlayer.GetPercentage();
            }
            );
            SetNowPlayingPercentPlayed();
            _progressSlider.ValueChanged += ProgressSlider_ValueChanged;
        }
        
        public void Pause()
        {
            if(_mediaPlayer != null)
            {
                SetNowPlayingPercentPlayed();
                _timer.Stop();
                _mediaPlayer.Pause();
                _playPauseButton.Source = "Play.png";
                _nowPlaying.IsPlaying = false;
            }
        }

        public void Stop()
        {
            if (_mediaPlayer != null && _nowPlaying != null)
            {
                SetNowPlayingPercentPlayed();
                _mediaPlayer.Stop();
                _playPauseButton.Source = "Play.png";
                _nowPlaying.IsPlaying = false;
            }
        }

        private void SetNowPlayingPercentPlayed()
        {
            _nowPlaying.PercentPlayed = _mediaPlayer.GetPercentage();
        }

    }
}