using NeuroshimaDB.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace NeuroshimaDB.Services
{
    public class ArticlesDataStore : IDataStore<Article>
    {
        private readonly List<Article> _articles;

        public ArticlesDataStore()
        {
            ReadAsset readAsset = new ReadAsset();
            string content = readAsset.NsArticles;

            string[] lines = null;
            try
            {
                lines = content?.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            }
            catch (Exception e)
            {
                Debug.Write(e.Message);
            }
            lines = lines ?? new string[0];

            int i = 0;

            _articles = new List<Article>();

            while (i < lines.Length)
            {
                _articles.Add(new Article(lines[i++], lines[i++], lines[i++], lines[i++], lines[i++], lines[i++]));
            }
        }

        public async Task<bool> AddItemAsync(Article article)
        {
            _articles.Add(article);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(Article article)
        {
            var oldArticle = _articles.Where((Article arg) => arg.Id == article.Id).FirstOrDefault();
            _articles.Remove(oldArticle);
            _articles.Add(article);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            var oldArticle = _articles.Where((Article arg) => arg.Id == id).FirstOrDefault();
            _articles.Remove(oldArticle);

            return await Task.FromResult(true);
        }

        public async Task<Article> GetItemAsync(string id)
        {
            return await Task.FromResult(_articles.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<Article>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(_articles);
        }

        public Task<bool> ClearItemsAsync()
        {
            throw new NotImplementedException();
        }
    }
}