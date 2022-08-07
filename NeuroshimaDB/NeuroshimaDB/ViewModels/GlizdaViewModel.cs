using NeuroshimaDB.Models;
using NeuroshimaDB.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace NeuroshimaDB.ViewModels
{
    public class GlizdaViewModel : BaseViewModel
    {
        private Location _selectedLocation;

        public int GambleIndex { get; set; } = 0;
        public int DifficultyIndex { get; set; } = 0;
        public int BigLocationsCount { get; set; } = 0;
        public int SmallLocationsCount { get; set; } = 0;
        public int HomeLocationsCount { get; set; } = 0;

        public ObservableCollection<Location> Locations { get; set; } = new();

        public Command CreateArea { get; set; }
        public Command LoadLocationsCommand { get; set; }
        public Command<Location> LocationTapped { get; }

        public Location SelectedLocation
        {
            get => _selectedLocation;
            set
            {
                SetProperty(ref _selectedLocation, value);
                OnItemSelected(value);
                OnPropertyChanged("SelectedLocation");
            }
        }

        public GlizdaViewModel()
        {
            Title = "GLIZDA";

            CreateArea = new(ExecuteCreateArea);
            LoadLocationsCommand = new(async () => await ExecuteLoadItemsCommand());
            LocationTapped = new(OnItemSelected);
        }

        public void UpdateGambleIndex(int index) =>
            GambleIndex = index;

        public void UpdateDifficultyIndex(int index) =>
            DifficultyIndex = index;

        public void UpdateBigLocationsCount(int count) =>
            BigLocationsCount = count;

        public void UpdateSmallLocationsCount(int count) =>
            SmallLocationsCount = count;

        public void UpdateHomeLocationsCount(int count) =>
            HomeLocationsCount = count;

        public async void OnAppearing()
        {
            IsBusy = true;
            SelectedLocation = null;
            await ExecuteLoadItemsCommand();
        }

        private async Task ExecuteLoadItemsCommand(string input = "", bool isReloading = false)
        {
            IsBusy = true;

            try
            {
                Locations.Clear();
                var locations = await LocationsDataStore.GetItemsAsync(true);

                foreach (var item in locations)
                {
                    Locations.Add(item);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async void ExecuteCreateArea()
        {
            List<Location> locations = new();

            for (int i = 0; i < BigLocationsCount; i++)
                locations.Add(new(GambleIndex, DifficultyIndex, 0));
            for (int i = 0; i < SmallLocationsCount; i++)
                locations.Add(new(GambleIndex, DifficultyIndex, 1));
            for (int i = 0; i < HomeLocationsCount; i++)
                locations.Add(new(GambleIndex, DifficultyIndex, 2));

            Locations.Clear();
            await LocationsDataStore.ClearItemsAsync();

            foreach (var item in locations)
            {
                Locations.Add(item);
                await LocationsDataStore.AddItemAsync(item);
            }
        }

        private async void OnItemSelected(Location item)
        {
            if (item == null)
                return;

            // This will push the ItemDetailPage onto the navigation stack
            try
            {
                await Shell.Current.GoToAsync($"{nameof(LocationDetailPage)}?{nameof(LocationDetailViewModel.ItemId)}={item.Id}");
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message, e.StackTrace);
            }
        }
    }
}