using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using PodHead;
using PodHead;

namespace PodHead.Android
{
    [Activity(Label = "PodHead", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
    {
        public static Activity ActivityContext;

        private ErrorLogger _errorLogger;

        protected override void OnCreate(Bundle bundle)
        {
            _errorLogger = ErrorLogger.Get(Config.Instance);

            base.OnCreate(bundle);
            
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            AndroidEnvironment.UnhandledExceptionRaiser += AndroidEnvironment_UnhandledExceptionRaiser;
            ActivityContext = this;

			global::Xamarin.Forms.Forms.Init (this, bundle);
            
			LoadApplication (new App());
        }

        private void AndroidEnvironment_UnhandledExceptionRaiser(object sender, RaiseThrowableEventArgs e)
        {
            _errorLogger.Log(e.Exception);
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            _errorLogger.Log(e.ExceptionObject as Exception);
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
    }
}

