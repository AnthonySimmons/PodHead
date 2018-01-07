using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace PodHead.Android.Views
{
    class TopChartsView : SubscriptionsView
    {
        private Picker GenrePicker = new Picker();

        private Label GenreLabel = new Label();

        private static int FieldHeight = 50;

        private readonly PodcastCharts _podcastCharts;

        public TopChartsView()
            : base()
        {
            _podcastCharts = PodcastCharts.Get(Config.Instance, _parser);
            Initialize();
        }

		protected override void Initialize()
        {
			base.Initialize ();

            _podcastCharts.PodcastSourceUpdated += Instance_PodcastSourceUpdated;
            
            GenreLabel.Text = "Genre:";
            GenreLabel.TextColor = Color.Black;
            GenreLabel.FontSize = 18;
            GenreLabel.HorizontalTextAlignment = TextAlignment.Center;

            GenrePicker.HeightRequest = FieldHeight;
            GenrePicker.BackgroundColor = Color.Gray;
                                    
            LoadTopChartsGenres();

            //GenrePicker.SelectedIndex = GenrePicker.Items.IndexOf(PodcastCharts.PodcastGenreCodes.Keys.First());
            GenrePicker.SelectedIndexChanged += SourceGenre_SelectedIndexChanged;
            AddControls();
        }
        

        protected override void AddControls()
        {
            stackLayout.Children.Insert(0, GenreLabel);
            stackLayout.Children.Insert(1, GenrePicker);
        }

        public void InitializeTopCharts()
        {
            GenrePicker.SelectedIndex = GenrePicker.Items.IndexOf(PodcastCharts.PodcastGenreCodes.Keys.First());
        }

        protected override void LoadMore()
        {
            _podcastCharts.Limit += 20;
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
            LoadSubscriptionResults(_podcastCharts.Podcasts);
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
                _podcastCharts.Genre = GenrePicker.Items[GenrePicker.SelectedIndex];
                _podcastCharts.ClearPodcasts();
                _podcastCharts.GetPodcastsAsync();
            }
        }

    }
}