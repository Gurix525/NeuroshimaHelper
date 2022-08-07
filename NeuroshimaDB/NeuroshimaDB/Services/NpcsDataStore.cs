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
    public class NpcsDataStore : IDataStore<Npc>
    {
        private readonly ObservableCollection<Npc> _npcs;

        public NpcsDataStore()
        {
            var fileName = "NSnpcs.json";
            string documentsPath = Environment
                .GetFolderPath(Environment.SpecialFolder.Personal); // Documents folder
            var path = Path.Combine(documentsPath, fileName);

            string json = string.Empty;
            if (File.Exists(path))
            {
                json = File.ReadAllText(path);
                _npcs = JsonConvert.DeserializeObject<ObservableCollection<Npc>>(json);
            }

            if (_npcs == null)
                _npcs = new ObservableCollection<Npc>();

            _npcs.CollectionChanged += (sender, e) =>
            {
                using (var sw = File.CreateText(path))
                {
                    string json = JsonConvert.SerializeObject(_npcs);
                    sw.Write(json);
                }
            };
        }

        public async Task<bool> AddItemAsync(Npc item)
        {
            _npcs.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> ClearItemsAsync()
        {
            _npcs.Clear();

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            var oldNpc = _npcs.Where((Npc npc) => npc.Id == id).FirstOrDefault();
            _npcs.Remove(oldNpc);

            return await Task.FromResult(true);
        }

        public async Task<Npc> GetItemAsync(string id)
        {
            return await Task.FromResult(_npcs.FirstOrDefault(x => x.Id == id));
        }

        public async Task<IEnumerable<Npc>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(_npcs);
        }

        public async Task<bool> UpdateItemAsync(Npc item)
        {
            var oldNpc = _npcs
                .Where((Npc x) => x.Id == item.Id)
                .FirstOrDefault();
            _npcs.Remove(oldNpc);
            _npcs.Add(item);

            return await Task.FromResult(true);
        }
    }
}