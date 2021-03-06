﻿using Wild.Pokenizer.Xamarin.IoC;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Wild.Pokenizer.Xamarin
{
    // ReSharper disable once RedundantExtendsListEntry
    public partial class App : Application
    {
        public App(AppSetup setup)
        {
            AppContainer.Container = setup.CreateContainer();

            InitializeComponent();

            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
