using Pettel.Models;
using Pettel.Repositories;
using Pettel.Repositories.PetPropertyRepositories;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Pettel.Pages.AdvertPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AdvertListPage : ContentPage
    {
        AdvertRepository _advertRepository = new AdvertRepository();

        public AdvertListPage()
        {
            InitializeComponent();

            lstPet.RefreshCommand = new Command(() =>
            {
                OnAppearing();
            });
        }

        private async void ToolBarItemFilter_Clicked(object sender, EventArgs e)
        {
            if (Preferences.Get("isFiltering", "") == "true")
            {
                string email = Preferences.Get("currentUserEmail", string.Empty);
                string password = Preferences.Get("currentUserPassword", string.Empty);

                Preferences.Clear();
                Preferences.Set("currentUserEmail", email);
                Preferences.Set("currentUserPassword", password);
                Preferences.Set("isFiltering", "false");

                ToolBarItemFilter.IconImageSource = "filter.png";

                OnAppearing();
            }
            else
            {
                await PopupNavigation.Instance.PushAsync(new AdvertsFilterPromptPage());
            }

        }
        private void ToolBarItemSearch_Clicked(object sender, EventArgs e)
        {
            if (AdvertsSearchBar.IsVisible == false)
            {
                AdvertsSearchBar.IsVisible = true;
            }
            else
            {
                AdvertsSearchBar.IsVisible = false;
            }
        }
        private async void btnCreateAdvert_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CreateAdvertPage());
        }

        public void bindingData()
        {
            Preferences.Set("isFiltering", "true");

            OnAppearing();
        }
        protected override async void OnAppearing()
        {
            if (Preferences.Get("isFiltering",null) == "false")
            {
                var adverts = await _advertRepository.GetAll(false);
                
                if(adverts.Count <= 0)
                {
                    lstPet.ItemsSource = null;
                    lstPet.IsRefreshing = false;
                    lblMessage.IsVisible = true;
                    lblMessage.Text = "Listenilecek ilan bulunamadı.";
                }
                else
                {
                    lblMessage.IsVisible = false;
                    lstPet.ItemsSource = null;
                    lstPet.ItemsSource = adverts;
                    lstPet.IsRefreshing = false;
                }
            }
            else
            {
                var adverts = await _advertRepository.GetAll(true);

                if (adverts.Count <= 0)
                {
                    lstPet.ItemsSource = null;
                    lstPet.IsRefreshing = false;
                    lblMessage.IsVisible = true;
                    lblMessage.Text = "Listenilecek ilan bulunamadı.";
                }
                else
                {
                    lblMessage.IsVisible = false;
                    lstPet.ItemsSource = null;
                    lstPet.ItemsSource = adverts;
                    lstPet.IsRefreshing = false;
                }
                ToolBarItemFilter.IconImageSource = "nofilter.png";
            }
            lstPet.IsRefreshing = false;
        }

        private async void lstPet_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
            {
                return;
            }
            var advert = e.Item as Advert;
            await Navigation.PushModalAsync(new NavigationPage(new AdvertDetailsPage(advert,false)));

            ((ListView)sender).SelectedItem = null;
        }

        private async void AdvertsSearchBar_SearchButtonPressed(object sender, EventArgs e)
        {
            string search = AdvertsSearchBar.Text;
            if (!String.IsNullOrEmpty(search))
            {
                var searchToList = await _advertRepository.GetAllByTitle(search);
                if(searchToList.Count <= 0)
                {
                    lblMessage.Text = "Aranan kriterlere uygun bir ilan bulunamadı.";
                    lblMessage.IsVisible = true;
                    lstPet.ItemsSource = null;
                    lstPet.IsRefreshing = false;
                }
                else
                {
                    lblMessage.IsVisible = false;
                    lstPet.ItemsSource = null;
                    lstPet.ItemsSource = searchToList;
                    lstPet.IsRefreshing = false;
                }
            }
            else
            {
                OnAppearing();
            }
        }

        private async void AdvertsSearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            string search = AdvertsSearchBar.Text;
            if (!String.IsNullOrEmpty(search))
            {
                var searchToList = await _advertRepository.GetAllByTitle(search);
                if (searchToList.Count <= 0)
                {
                    lblMessage.Text = "Aranan kriterlere uygun bir ilan bulunamadı.";
                    lblMessage.IsVisible = true;
                    lstPet.ItemsSource = null;
                    lstPet.IsRefreshing = false;
                }
                else
                {
                    lblMessage.IsVisible = false;
                    lstPet.ItemsSource = null;
                    lstPet.ItemsSource = searchToList;
                    lstPet.IsRefreshing = false;
                }
            }
            else
            {
                OnAppearing();
            }
        }
    }
}