using Pettel.Pages.Interfaces;
using Pettel.Pages.UserPages;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Pettel
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new LoginUserPage());
            var navigationPage = MainPage as NavigationPage;
            navigationPage.BarBackgroundColor = Color.DarkOrange;
            //MainPage = new BottomTabs();

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
