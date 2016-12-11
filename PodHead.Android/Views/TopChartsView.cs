using PodHead;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace PodHead.Android.Views
{
    class TopChartsView : SubscriptionsView
    {
        private Picker GenrePicker = new Picker();

        private Label GenreLabel = new Label();

        private static int FieldHeight = 50;

        public TopChartsView()
        {
            Initialize();
        }

		protected override void Initialize()
        {
			base.Initialize ();

            PodcastCharts.Instance.PodcastSourceUpdated += Instance_PodcastSourceUpdated;
            
            GenreLabel.Text = "Genre:";
            GenreLabel.TextColor = Color.Black;
            GenreLabel.FontSize = 18;
            GenreLabel.HorizontalTextAlignment = TextAlignment.Center;

            GenrePicker.HeightRequest = FieldHeight;
            GenrePicker.BackgroundColor = Color.Gray;
                        
            Children.Insert(0, GenreLabel);
            Children.Insert(1, GenrePicker);
            
            LoadTopChartsGenres();

            //GenrePicker.SelectedIndex = GenrePicker.Items.IndexOf(PodcastCharts.PodcastGenreCodes.Keys.First());
            GenrePicker.SelectedIndexChanged += SourceGenre_SelectedIndexChanged;
        }

        public void InitializeTopCharts()
        {
            GenrePicker.SelectedIndex = GenrePicker.Items.IndexOf(PodcastCharts.PodcastGenreCodes.Keys.First());
        }

        protected override void LoadMore()
        {
            PodcastCharts.Limit += 10;
            LoadTopChartsAsync();
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
            LoadTopChartsAsync();
        }

        private void LoadTopChartsAsync()
        {
            if (GenrePicker.SelectedIndex >= 0)
            {
                PodcastCharts.Genre = GenrePicker.Items[GenrePicker.SelectedIndex];
                PodcastCharts.Instance.ClearPodcasts();
                PodcastCharts.Instance.GetPodcastsAsync();
            }
        }

    }
}