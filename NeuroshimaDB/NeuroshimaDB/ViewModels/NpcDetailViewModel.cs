using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Xamarin.Forms;

namespace NeuroshimaDB.ViewModels
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    public class NpcDetailViewModel : BaseViewModel
    {
        private string _npcId;
        private string _name;

        private int _zr;
        private int _prc;
        private int _cha;
        private int _spr;
        private int _bud;
        private Dictionary<string, int> _skills;
        private string _perk;
        private string _perkDescripion;

        public string Id { get; set; }

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public int Zr
        {
            get => _zr;
            set => SetProperty(ref _zr, value);
        }

        public int Prc
        {
            get => _prc;
            set => SetProperty(ref _prc, value);
        }

        public int Cha
        {
            get => _cha;
            set => SetProperty(ref _cha, value);
        }

        public int Spr
        {
            get => _spr;
            set => SetProperty(ref _spr, value);
        }

        public int Bud
        {
            get => _bud;
            set => SetProperty(ref _bud, value);
        }

        public Dictionary<string, int> Skills
        {
            get => _skills;
            set => SetProperty(ref _skills, value);
        }

        public string Perk
        {
            get => _perk;
            set => SetProperty(ref _perk, value);
        }

        public string PerkDescription
        {
            get => _perkDescripion;
            set => SetProperty(ref _perkDescripion, value);
        }

        public string ItemId
        {
            get
            {
                return _npcId;
            }
            set
            {
                _npcId = value;
                LoadItemId(value);
            }
        }

        public NpcDetailViewModel()
        {
            Title = "Szczegóły postaci";
        }

        public async void LoadItemId(string itemId)
        {
            try
            {
                var item = await NpcsDataStore.GetItemAsync(itemId);
                Id = item.Id;
                Name = item.Name;
                Zr = item.Zr;
                Prc = item.Pr;
                Cha = item.Cha;
                Spr = item.Spr;
                Bud = item.Bd;
                Skills = item.Skills;
                Perk = item.Perk;
                PerkDescription = item.PerkDescription;
            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to Load Item");
            }
        }
    }
}