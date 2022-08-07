using NeuroshimaDB.Models;
using NeuroshimaDB.Services;
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
    public class NpcsViewModel : BaseViewModel
    {
        private Npc _selectedNpc;

        public int Power { get; set; } = 1;
        public string Profession { get; set; } = "";

        public ObservableCollection<Npc> Npcs { get; set; } = new();

        public Command CreateNpcCommand { get; set; }
        public Command LoadNpcsCommand { get; set; }
        public Command<Npc> NpcTappedCommand { get; set; }
        public Command ClearNpcsCommand { get; set; }

        public Npc SelectedNpc
        {
            get => _selectedNpc;
            set
            {
                SetProperty(ref _selectedNpc, value);
                OnItemSelected(value);
                OnPropertyChanged("SelectedNpc");
            }
        }

        public NpcsViewModel()
        {
            Title = "Postacie niezależne";

            CreateNpcCommand = new(ExecuteCreateNpc);
            LoadNpcsCommand = new(async () => await ExecuteLoadItemsCommand());
            NpcTappedCommand = new(OnItemSelected);
            ClearNpcsCommand = new(ExecuteClearNpcsCommand);
        }

        public async void OnAppearing()
        {
            IsBusy = true;
            SelectedNpc = null;
            await ExecuteLoadItemsCommand();
        }

        public async void ExecuteClearNpcsCommand()
        {
            Npcs.Clear();
            await NpcsDataStore.ClearItemsAsync();
        }

        public async void ExecuteCreateNpc()
        {
            Npc item = new(Power, Profession);

            Npcs.Add(item);
            await NpcsDataStore.AddItemAsync(item);
        }

        private async Task ExecuteLoadItemsCommand(string input = "", bool isReloading = false)
        {
            IsBusy = true;

            try
            {
                Npcs.Clear();
                var npcs = await NpcsDataStore.GetItemsAsync(true);

                foreach (var item in npcs)
                {
                    Npcs.Add(item);
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

        private async void OnItemSelected(Npc item)
        {
            if (item == null)
                return;

            // This will push the ItemDetailPage onto the navigation stack
            try
            {
                await Shell.Current.GoToAsync($"{nameof(NpcDetailPage)}?{nameof(NpcDetailViewModel.ItemId)}={item.Id}");
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message, e.StackTrace);
            }
        }
    }
}