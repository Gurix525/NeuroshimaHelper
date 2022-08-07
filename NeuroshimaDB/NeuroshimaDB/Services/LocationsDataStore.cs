using Android.Content.Res;
using NeuroshimaDB.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuroshimaDB.Services
{
    public class LocationsDataStore : IDataStore<Location>
    {
        private readonly ObservableCollection<Location> _locations;

        public LocationsDataStore()
        {
            var fileName = "NSlocations.json";
            string documentsPath = Environment
                .GetFolderPath(Environment.SpecialFolder.Personal); // Documents folder
            var path = Path.Combine(documentsPath, fileName);

            string json = string.Empty;
            if (File.Exists(path))
            {
                json = File.ReadAllText(path);
                _locations = JsonConvert.DeserializeObject<ObservableCollection<Location>>(json);
            }

            if (_locations == null)
                _locations = new ObservableCollection<Location>();

            _locations.CollectionChanged += (sender, e) =>
            {
                using (var sw = File.CreateText(path))
                {
                    string json = JsonConvert.SerializeObject(_locations);
                    sw.Write(json);
                }
            };
        }

        public async Task<bool> AddItemAsync(Location item)
        {
            _locations.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            var oldArticle = _locations.Where((Location arg) => arg.Id == id).FirstOrDefault();
            _locations.Remove(oldArticle);

            return await Task.FromResult(true);
        }

        public async Task<bool> ClearItemsAsync()
        {
            _locations.Clear();

            return await Task.FromResult(true);
        }

        public async Task<Location> GetItemAsync(string id)
        {
            return await Task.FromResult(_locations.FirstOrDefault(x => x.Id == id));
        }

        public async Task<IEnumerable<Location>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(_locations);
        }

        public async Task<bool> UpdateItemAsync(Location item)
        {
            var oldLocation = _locations
                .Where((Location x) => x.Id == item.Id)
                .FirstOrDefault();
            _locations.Remove(oldLocation);
            _locations.Add(item);

            return await Task.FromResult(true);
        }
    }
}