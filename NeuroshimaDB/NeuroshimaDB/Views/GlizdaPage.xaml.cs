using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuroshimaDB.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NeuroshimaDB.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GlizdaPage : ContentPage
    {
        private GlizdaViewModel _viewModel;

        public GlizdaPage()
        {
            InitializeComponent();

            BindingContext = _viewModel = new GlizdaViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }

        private void GamblePicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var viewModel = BindingContext as GlizdaViewModel;
            var selectedIndex = (sender as Picker).SelectedIndex;
            viewModel.UpdateGambleIndex(selectedIndex);
        }

        private void DifficultyPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var viewModel = BindingContext as GlizdaViewModel;
            var selectedIndex = (sender as Picker).SelectedIndex;
            viewModel.UpdateDifficultyIndex(selectedIndex);
        }

        private void BigLocationsEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            var viewModel = BindingContext as GlizdaViewModel;
            int.TryParse(e.NewTextValue, out int newCount);
            viewModel.UpdateBigLocationsCount(newCount);
        }

        private void SmallLocationsEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            var viewModel = BindingContext as GlizdaViewModel;
            int.TryParse(e.NewTextValue, out int newCount);
            viewModel.UpdateSmallLocationsCount(newCount);
        }

        private void HomeLocationsEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            var viewModel = BindingContext as GlizdaViewModel;
            int.TryParse(e.NewTextValue, out int newCount);
            viewModel.UpdateHomeLocationsCount(newCount);
        }
    }
}