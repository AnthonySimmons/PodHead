using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using PodHead;
using Android.Content.PM;
using Android.Net;
using PodHead.Android.Utilities;
using PodHead.Android.Audio;
using Android.Media;

namespace PodHead.Android
{
    [Activity(Label = "PodHead", MainLauncher = true, Icon = "@drawable/icon", ScreenOrientation = ScreenOrientation.Portrait)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
    {
        private bool _isDisposed;

        public static MainActivity ActivityContext;

        private ErrorLogger _errorLogger;

        private NetworkChangeBroadcastReceiver _networkChangedReceiver;

        private int _networkAlertCount;

        private AudioFocusChangeListener _audioFocusChangeListener;

        public event Action PlayPauseEvent;

        public event Action PlayEvent;

        public event Action PauseEvent;

        protected override void OnCreate(Bundle bundle)
        {
            _errorLogger = ErrorLogger.Get(Config.Instance);

            base.OnCreate(bundle);
            
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            AndroidEnvironment.UnhandledExceptionRaiser += AndroidEnvironment_UnhandledExceptionRaiser;
            ActivityContext = this;

			global::Xamarin.Forms.Forms.Init (this, bundle);
            
			LoadApplication (new App());
            
            _networkChangedReceiver = new NetworkChangeBroadcastReceiver();
            _networkChangedReceiver.NetworkChangeEvent += _networkChangedReceiver_NetworkChangeEvent;

            Application.Context.RegisterReceiver(_networkChangedReceiver, new IntentFilter(ConnectivityManager.ConnectivityAction));

            _audioFocusChangeListener = new AudioFocusChangeListener();
            _audioFocusChangeListener.PlayEvent += AudioFocusChangeListener_PlayEvent;
            _audioFocusChangeListener.PauseEvent += AudioFocusChangeListener_PauseEvent;
        }

        private void AudioFocusChangeListener_PauseEvent()
        {
            PauseEvent?.Invoke();
        }

        private void AudioFocusChangeListener_PlayEvent()
        {
            PlayEvent?.Invoke();
        }

        public void OnPlayPauseEvent()
        {
            PlayPauseEvent?.Invoke();
        }

        public void RequestAudioFocus()
        {
            AudioManager audioManager = (AudioManager)GetSystemService(AudioService);
            audioManager.RequestAudioFocus(_audioFocusChangeListener, Stream.Music, AudioFocus.Gain);
        }

        public void ReleaseAudioFocus()
        {
            AudioManager audioManager = (AudioManager)GetSystemService(AudioService);
            audioManager.AbandonAudioFocus(_audioFocusChangeListener);
        }

        private void _networkChangedReceiver_NetworkChangeEvent(object sender, EventArgs e)
        {
            CheckWifi();
        }

        private void CheckWifi()
        {
            if(_networkAlertCount > 0)
            {
                return;
            }

            var callback = new ConnectivityManager.NetworkCallback();
            ConnectivityManager connectivityManager = (ConnectivityManager)GetSystemService(ConnectivityService);
            NetworkInfo wifiInfo = connectivityManager.GetNetworkInfo(ConnectivityType.Wifi);
            if (!wifiInfo.IsConnected)
            {
                AlertDialog.Builder alert = new AlertDialog.Builder(this);
                alert.SetTitle("Network Connectivity");
                alert.SetMessage("Wifi is not connected.");
                alert.SetNeutralButton("Ok", (senderAlert, args) => { _networkAlertCount--; });
                Dialog dialog = alert.Create();

                _networkAlertCount++;
                dialog.Show();
            }
        }

        private void AndroidEnvironment_UnhandledExceptionRaiser(object sender, RaiseThrowableEventArgs e)
        {
            _errorLogger.Log(e.Exception);
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            _errorLogger.Log(e.ExceptionObject as Exception);
        }

        protected override void OnStart()
        {
            base.OnStart();

            Application.ApplicationContext.RegisterReceiver(_networkChangedReceiver, new IntentFilter(ConnectivityManager.ConnectivityAction));
        }

        protected override void OnStop ()
		{
			base.OnStop ();

            Parser parser = Parser.Get(Config.Instance);
            Feeds feeds = Feeds.Get(parser, Config.Instance);
			feeds.Save();
		}

        protected override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);

            Parser parser = Parser.Get(Config.Instance);
            Feeds feeds = Feeds.Get(parser, Config.Instance);
            feeds.Save();            
        }

        public override void OnBackPressed()
        {
            if (HomePage.HasPreviousPages)
            {
                App.Current.MainPage.SendBackButtonPressed();
            }
            else
            {
                base.OnBackPressed();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if(disposing && !_isDisposed)
            {
                _isDisposed = true;
                Application.ApplicationContext.UnregisterReceiver(_networkChangedReceiver);

                ReleaseAudioFocus();

                if (_audioFocusChangeListener != null)
                {
                    _audioFocusChangeListener.PlayEvent -= AudioFocusChangeListener_PlayEvent;
                    _audioFocusChangeListener.PauseEvent -= AudioFocusChangeListener_PauseEvent;
                }
            }
            base.Dispose(disposing);
        }
    }
}

