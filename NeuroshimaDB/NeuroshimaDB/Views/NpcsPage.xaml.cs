using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using NeuroshimaDB.ViewModels;

namespace NeuroshimaDB.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NpcsPage : ContentPage
    {
        private NpcsViewModel _viewModel;

        public NpcsPage()
        {
            InitializeComponent();

            BindingContext = _viewModel = new NpcsViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }

        public void PowerPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            _viewModel.Power = ((Picker)sender).SelectedIndex;
        }

        public void ProfessionPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            _viewModel.Profession = (string)((Picker)sender).SelectedItem;
        }
    }
}