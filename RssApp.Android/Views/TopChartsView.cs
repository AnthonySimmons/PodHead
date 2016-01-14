using RSS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace RssApp.Android.Views
{
    class TopChartsView : SubscriptionsView
    {
        private Picker GenrePicker = new Picker();

        private Label GenreLabel = new Label();

        private static int FieldHeight = 50;

        public TopChartsView() : base()
        {
            Initialize();
        }

        private void Initialize()
        {
            PodcastCharts.Instance.PodcastSourceUpdated += Instance_PodcastSourceUpdated;

            GenrePicker.SelectedIndexChanged += SourceGenre_SelectedIndexChanged;
            
            GenrePicker.HeightRequest = FieldHeight;
            GenrePicker.BackgroundColor = Color.Gray;

            GenreLabel.Text = "Genre";
            GenreLabel.TextColor = Color.Black;

            var layout = new StackLayout { Orientation = StackOrientation.Horizontal, };
            layout.Children.Add(GenreLabel);
            layout.Children.Add(GenrePicker);
            
            RefreshButton.IsVisible = false;

            Children.Insert(0, layout);

            LoadTopChartsGenres();
        }


        private void Instance_PodcastSourceUpdated(double updatePercentage)
        {
            if (updatePercentage >= 1.0)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    progressBar.IsVisible = false;
                    LoadTopCharts();
                }
                );
            }
            else
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    progressBar.IsVisible = true;
                    progressBar.Progress = updatePercentage;
                }
                );
            }
        }


        private void LoadTopCharts()
        {
            LoadSubscriptionResults(PodcastCharts.Instance.Podcasts);
        }


        private void LoadTopChartsGenres()
        {
            GenrePicker.Items.Clear();
            foreach (var genre in PodcastCharts.PodcastGenreCodes.Keys)
            {
                GenrePicker.Items.Add(genre);
            }
        }

        private void SourceGenre_SelectedIndexChanged(object sender, EventArgs e)
        {
            PodcastCharts.Genre = GenrePicker.Items[GenrePicker.SelectedIndex];
            PodcastCharts.Instance.Podcasts.Clear();
            PodcastCharts.Instance.GetPodcastsAsync();
        }

    }
}