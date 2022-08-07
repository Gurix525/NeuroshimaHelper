using NeuroshimaDB.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NeuroshimaDB.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NpcDetailPage : ContentPage
    {
        public NpcDetailPage()
        {
            InitializeComponent();

            BindingContext = new NpcDetailViewModel();
        }
    }
}