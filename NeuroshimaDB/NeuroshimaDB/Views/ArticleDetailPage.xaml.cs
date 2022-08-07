using NeuroshimaDB.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace NeuroshimaDB.Views
{
    public partial class ArticleDetailPage : ContentPage
    {
        public ArticleDetailPage()
        {
            InitializeComponent();
            BindingContext = new ArticleDetailViewModel();
        }
    }
}