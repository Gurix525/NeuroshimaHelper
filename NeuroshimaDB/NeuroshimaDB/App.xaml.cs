using NeuroshimaDB.Services;
using NeuroshimaDB.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NeuroshimaDB
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            DependencyService.Register<ArticlesDataStore>();
            DependencyService.Register<LocationsDataStore>();
            DependencyService.Register<NpcsDataStore>();
            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}