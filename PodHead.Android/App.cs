using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using Xamarin.Forms;

namespace PodHead.Android
{
    class App : Xamarin.Forms.Application
    {
        public App()
        {
            // The root page of your application
            MainPage = new NavigationPage(new HomePage())
            {
                BarTextColor = Color.White
            };

        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
        
    }
}