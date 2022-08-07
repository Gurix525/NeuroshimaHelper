using NeuroshimaDB.Models;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace NeuroshimaDB.ViewModels
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    public class ArticleDetailViewModel : BaseViewModel
    {
        private string articleId;
        private string name;
        private int price;
        private int availability;
        private string description;
        private string keywords;
        public string Id { get; set; }

        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }

        public int Price
        {
            get => price;
            set => SetProperty(ref price, value);
        }

        public int Availability
        {
            get => availability;
            set => SetProperty(ref availability, value);
        }

        public string Description
        {
            get => description;
            set => SetProperty(ref description, value);
        }

        public string Keywords
        {
            get => keywords;
            set => SetProperty(ref keywords, value);
        }

        public string ItemId
        {
            get
            {
                return articleId;
            }
            set
            {
                articleId = value;
                LoadItemId(value);
            }
        }

        public ArticleDetailViewModel()
        {
            Title = "Szczegóły artykułu";
        }

        public async void LoadItemId(string itemId)
        {
            try
            {
                var item = await ArticlesDataStore.GetItemAsync(itemId);
                Id = item.Id;
                Name = item.Name;
                Price = item.Price;
                Availability = item.Availability;
                Description = item.Description;
                Keywords = item.Keywords;
            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to Load Item");
            }
        }
    }
}