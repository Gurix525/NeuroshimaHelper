using NeuroshimaDB.ViewModels;
using NeuroshimaDB.Views;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace NeuroshimaDB
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ArticleDetailPage), typeof(ArticleDetailPage));
            Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
            Routing.RegisterRoute(nameof(LocationDetailPage), typeof(LocationDetailPage));
            Routing.RegisterRoute(nameof(NpcDetailPage), typeof(NpcDetailPage));
        }

        //protected override async void OnAppearing()
        //{
        //    base.OnAppearing();
        //    await Current.GoToAsync(nameof(AboutPage));
        //}

        //private async void OnMenuItemClicked(object sender, EventArgs e)
        //{
        //    await Shell.Current.GoToAsync("//LoginPage");
        //}

        ///////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////
        // Ten kod ratuje dupe i sprawia że flyout przestaje wyglądać jak zlagowane gówno :DDDDDDDDDD
        // Ale z drugiej strony z jakiegoś powodu przestała sie wyświetlać strona startowa, irytujące
        //protected override async void OnPropertyChanged([CallerMemberName] string propertyName = null)
        //{
        //    if (propertyName.Equals("CurrentItem") && Device.RuntimePlatform == Device.Android)
        //    {
        //        FlyoutIsPresented = false;
        //        await Task.Delay(300);
        //    }
        //    base.OnPropertyChanged(propertyName);
        //}
        ///////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////
    }
}