using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using RssApp;
using RSS;

namespace RssApp.Android
{
    [Activity(Label = "RSS", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
    {
        public static Activity ActivityContext;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            ActivityContext = this;

			global::Xamarin.Forms.Forms.Init (this, bundle);
            
			LoadApplication (new App());
        }
        

		protected override void OnStop ()
		{
			base.OnStop ();
			Feeds.Instance.Save(RSSConfig.ConfigFileName);
		}

        protected override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);
            Feeds.Instance.Save(RSSConfig.ConfigFileName);
        }
    }
}

