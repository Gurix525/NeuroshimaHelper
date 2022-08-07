using NeuroshimaDB.Models;
using NeuroshimaDB.ViewModels;
using NeuroshimaDB.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NeuroshimaDB.Views
{
    public partial class ArticlesPage : ContentPage
    {
        private ArticlesViewModel _viewModel;

        public ArticlesPage()
        {
            InitializeComponent();

            BindingContext = _viewModel = new ArticlesViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }

        public void OnArticleNameTextChanged(object sender, TextChangedEventArgs e)
        {
            _ = _viewModel.OnArticleNameTextChanged(e.NewTextValue);
        }

        public void OnRegionPickerSelectionChanged(object sender, EventArgs e)
        {
            string selection = ((Picker)sender).SelectedItem as string;
            Models.Region region = selection switch
            {
                "Bez regionu" => Models.Region.bez,
                "Detroit" => Models.Region.detroit,
                "Federacja Appalachów" => Models.Region.fa,
                "Hegemonia" => Models.Region.hegemonia,
                "Nowy Jork" => Models.Region.nj,
                "Miami" => Models.Region.miami,
                "Posterunek" => Models.Region.posterunek,
                "Missisipi" => Models.Region.missisipi,
                "Pustynia" => Models.Region.pustynia,
                "Teksas" => Models.Region.teksas,
                "Vegas" => Models.Region.vegas,
                "Salt Lake City" => Models.Region.slc,
                _ => Models.Region.bez
            };
            _viewModel.Region = region;
        }
    }
}