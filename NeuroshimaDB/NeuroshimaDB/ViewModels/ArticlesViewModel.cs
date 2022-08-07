using NeuroshimaDB.Models;
using NeuroshimaDB.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using System.Linq;
using NeuroshimaDB.Extensions;

namespace NeuroshimaDB.ViewModels
{
    public class ArticlesViewModel : BaseViewModel
    {
        private Article _selectedItem;

        public ObservableCollection<Article> Articles { get; }
        public List<Article> SelectedArticles { get; set; }
        public Command LoadArticlesCommand { get; }
        public Command AddArticleCommand { get; }
        public Command<Article> ArticleTapped { get; }
        public Dictionary<Models.Region, float[]> PriceModificators;

        private Models.Region _region = Models.Region.bez;

        public Models.Region Region
        {
            get
            {
                return _region;
            }
            set
            {
                _region = value;
                OnRegionChanged(Region);
            }
        }

        private bool _isLoading = false;

        public ArticlesViewModel()
        {
            Title = "Sprzęt";
            Articles = new();
            SelectedArticles = new();
            LoadArticlesCommand = new(async () => await ExecuteLoadItemsCommand());

            ArticleTapped = new(OnItemSelected);

            AddArticleCommand = new(OnAddItem);

            PriceModificators = new()
            {
                {Models.Region.bez,         new float[] { 1F, 1F, 1F, 1F, 1F, 1F, 1F} },
                {Models.Region.detroit,     new float[] { 2F, 1F, 0.5F, 2F, 1.5F, 1F, 2F} },
                {Models.Region.fa,          new float[] { 1F, 1.5F, 2F, 1F, 1.5F, 1.5F, 1F} },
                {Models.Region.hegemonia,   new float[] { 1.5F, 0.5F, 1F, 1.5F, 2F, 1F, 1.5F} },
                {Models.Region.nj,          new float[] { 0.75F, 1.5F, 1.5F, 1F, 1.5F, 2F, 0.25F} },
                {Models.Region.miami,       new float[] { 1F, 1F, 1F, 0.25F, 1.5F, 0.5F, 1F} },
                {Models.Region.posterunek,  new float[] { 2F, 2F, 1.5F, 0.5F, 2F, 2F, 0.25F} },
                {Models.Region.missisipi,   new float[] { 0.75F, 1F, 1F, 1.5F, 1.5F, 1.5F, 1.5F} },
                {Models.Region.pustynia,    new float[] { 1.5F, 0.25F, 0.25F, 0.5F, 0.5F, 1F, 0.5F} },
                {Models.Region.teksas,      new float[] { 0.25F, 0.5F, 1F, 0.25F, 1F, 0.25F, 2F} },
                {Models.Region.vegas,       new float[] { 1F, 1.5F, 1F, 0.25F, 1.5F, 1.5F, 1.5F} },
                {Models.Region.slc,         new float[] { 1F, 1F, 1F, 0.5F, 1.5F, 0.25F, 1F} }
            };

            _ = OnArticleNameTextChanged(null);
        }

        public async Task OnArticleNameTextChanged(string input)
        {
            if (!_isLoading)
            {
                _isLoading = true;
                await ExecuteLoadItemsCommand(input);
                _isLoading = false;
            }
        }

        private async Task ExecuteLoadItemsCommand(string input = "", bool isReloading = false)
        {
            IsBusy = true;

            try
            {
                Articles.Clear();
                List<Article> selectedItems;
                if (isReloading)
                {
                    selectedItems = SelectedArticles;
                }
                else
                {
                    var allItems = await ArticlesDataStore.GetItemsAsync(true);
                    selectedItems = SelectArticles(input, allItems);
                    SelectedArticles = selectedItems;
                }

                foreach (var item in selectedItems)
                {
                    Articles.Add(item);
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

        private List<Article> SelectArticles(string input, IEnumerable<Article> allArticles)
        {
            if (input == "" || input == null)
                return allArticles.ToList();

            string[] inputs = input.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            var articles = new List<Article>();

            for (int i = 0; i < 4; i++)
            {
                foreach (var article in allArticles)
                    if (inputs.All(x => article[i].Contains(x, StringComparison.OrdinalIgnoreCase)))
                        articles.Add(article);
            }
            return articles.Distinct().ToList();
        }

        public void OnAppearing()
        {
            IsBusy = true;
            SelectedItem = null;
        }

        public Article SelectedItem
        {
            get => _selectedItem;
            set
            {
                SetProperty(ref _selectedItem, value);
                OnItemSelected(value);
                OnPropertyChanged("SelectedItem");
            }
        }

        private async void OnAddItem(object obj)
        {
            await Shell.Current.GoToAsync(nameof(NewItemPage));
        }

        private async void OnItemSelected(Article item)
        {
            if (item == null)
                return;

            // This will push the ItemDetailPage onto the navigation stack
            await Shell.Current.GoToAsync($"{nameof(ArticleDetailPage)}?{nameof(ArticleDetailViewModel.ItemId)}={item.Id}");
        }

        private void OnRegionChanged(Models.Region region)
        {
            float[] values = PriceModificators[region];

            foreach (Article item in Articles)
            {
                item.Price = item.ArticleType switch
                {
                    ArticleType.paliwo => newPrice(item.PriceDefault, values[0]),
                    ArticleType.elektronika => newPrice(item.PriceDefault, values[1]),
                    ArticleType.mechanika => newPrice(item.PriceDefault, values[2]),
                    ArticleType.prochy => newPrice(item.PriceDefault, values[3]),
                    ArticleType.bron => newPrice(item.PriceDefault, values[4]),
                    ArticleType.zywnosc => newPrice(item.PriceDefault, values[5]),
                    ArticleType.uslugi => newPrice(item.PriceDefault, values[6]),
                    _ => item.PriceDefault
                };
            }

            _ = ExecuteLoadItemsCommand(isReloading: true);

            int newPrice(int price, float modificator) =>
                (int)Math.Ceiling(price * modificator);
        }
    }
}