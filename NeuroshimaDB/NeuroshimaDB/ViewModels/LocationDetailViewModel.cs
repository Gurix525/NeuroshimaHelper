using NeuroshimaDB.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace NeuroshimaDB.ViewModels
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    public class LocationDetailViewModel : BaseViewModel
    {
        private string _locationId;
        private string _name;

        private int _difficultyLevel;
        private int _modifier;
        private string _danger;
        private string _articles1;
        private string _articles2;
        private string _articles3;
        private string _articles4;

        public string Id { get; set; }

        public Command RandomizeAgain { get; set; }

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public int DifficultyLevel
        {
            get => _difficultyLevel;
            set => SetProperty(ref _difficultyLevel, value);
        }

        public int Modifier
        {
            get => _modifier;
            set => SetProperty(ref _modifier, value);
        }

        public string Danger
        {
            get => _danger;
            set => SetProperty(ref _danger, value);
        }

        public string Articles1
        {
            get => _articles1;
            set => SetProperty(ref _articles1, value);
        }

        public string Articles2
        {
            get => _articles2;
            set => SetProperty(ref _articles2, value);
        }

        public string Articles3
        {
            get => _articles3;
            set => SetProperty(ref _articles3, value);
        }

        public string Articles4
        {
            get => _articles4;
            set => SetProperty(ref _articles4, value);
        }

        public string ItemId
        {
            get
            {
                return _locationId;
            }
            set
            {
                _locationId = value;
                LoadItemId(value);
            }
        }

        public LocationDetailViewModel()
        {
            Title = "Szczegóły lokacji";

            RandomizeAgain = new(ExecuteRandomizeAgainCommand);
        }

        public async void LoadItemId(string itemId)
        {
            try
            {
                var item = await LocationsDataStore.GetItemAsync(itemId);
                Id = item.Id;
                Name = item.Name;
                DifficultyLevel = item.DifficultyLevel;
                Modifier = item.Modifier;
                Danger = item.Danger;
                Articles1 = item.Articles1;
                Articles2 = item.Articles2;
                Articles3 = item.Articles3;
                Articles4 = item.Articles4;
            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to Load Item");
            }
        }

        private async void ExecuteRandomizeAgainCommand()
        {
            await UpdateLocation();
        }

        private async Task<bool> UpdateLocation()
        {
            var oldLocation = await LocationsDataStore.GetItemAsync(Id);

            await LocationsDataStore.UpdateItemAsync(await Location.RandomizeLocationAgain(oldLocation));
            LoadItemId(Id);

            return await Task.FromResult(true);
        }
    }
}