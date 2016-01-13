using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using RssApp;

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

            // Set our view from the "main" layout resource
            //SetContentView(Resource.Layout.Main);
			LoadApplication (new App());
            
        }
    }
}

