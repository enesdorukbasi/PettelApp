using Pettel.Models;
using Pettel.Repositories;
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
    public partial class MyAdvertsListPage : ContentPage
    {
        AdvertRepository _advertRepository = new AdvertRepository();

        public MyAdvertsListPage()
        {
            InitializeComponent();

            lstAdverts.RefreshCommand = new Command(() =>
            {
                OnAppearing();
            });
        }
        protected override async void OnAppearing()
        {
            string email;
            try
            {
                email = Preferences.Get("currentUserEmail", string.Empty);
            }
            catch
            {
                email = null;
            }

            if (!String.IsNullOrEmpty(email))
            {
                var myAdverts = await _advertRepository.GetAllByEMail(email);
                if(myAdverts.Count <= 0)
                {
                    lstAdverts.ItemsSource = null;
                    lstAdverts.IsRefreshing = false;
                    lblMessage.IsVisible = true;
                    lblMessage.Text = "Henüz bir ilanınız yok.";
                }
                else
                {
                    lstAdverts.ItemsSource = null;
                    lstAdverts.ItemsSource = myAdverts;
                    lstAdverts.IsRefreshing = false;
                    lblMessage.IsVisible = false;
                }
            }
            else
            {
                await DisplayAlert("Hata!", "Kullanmakta olduğunuz hesaptan çıkış yapıldı. Lütfen tekrar giriniz.", "Tamam");
            }
        }

        private async void AdvertsSearchBar_SearchButtonPressed(object sender, EventArgs e)
        {
            string email; 
            try
            {
                email = Preferences.Get("currentUserEmail", string.Empty);
            }
            catch
            {
                email = null;
            }
            string search = AdvertsSearchBar.Text;

            if (!String.IsNullOrEmpty(search) && !String.IsNullOrEmpty(email))
            {
                var searchToList = await _advertRepository.GetAllByEMailAndTitle(email,search);
                if(searchToList.Count <= 0)
                {
                    lblMessage.Text = "Aranan kriterlere uygun bir ilan bulunamadı.";
                    lblMessage.IsVisible = false;
                    lstAdverts.ItemsSource = null;
                    lstAdverts.ItemsSource = searchToList;
                    lstAdverts.IsRefreshing = false;
                }
                else
                {
                    lblMessage.IsVisible = false;
                    lstAdverts.ItemsSource = null;
                    lstAdverts.ItemsSource = searchToList;
                    lstAdverts.IsRefreshing = false;
                }
            }
            else
            {
                OnAppearing();
            }
        }

        private async void AdvertsSearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            string email;
            try
            {
                email = Preferences.Get("currentUserEmail", string.Empty);
            }
            catch
            {
                email = null;
            }
            string search = AdvertsSearchBar.Text;
            if (!String.IsNullOrEmpty(search))
            {
                var searchToList = await _advertRepository.GetAllByEMailAndTitle(email,search);
                if(searchToList.Count <= 0)
                {
                    lblMessage.Text = "Aranan kriterlere uygun bir ilan bulunamadı.";
                    lblMessage.IsVisible = true;
                    lstAdverts.ItemsSource = null;
                    lstAdverts.ItemsSource = searchToList;
                    lstAdverts.IsRefreshing = false;
                }
                else
                {
                    lblMessage.IsVisible = false;
                    lstAdverts.ItemsSource = null;
                    lstAdverts.ItemsSource = searchToList;
                    lstAdverts.IsRefreshing = false;
                }
            }
            else
            {
                OnAppearing();
            }
        }

        private async void lstAdverts_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
            {
                return;
            }
            var advert = e.Item as Advert;
            await Navigation.PushModalAsync(new NavigationPage(new AdvertDetailsPage(advert,true)));

            ((ListView)sender).SelectedItem = null;
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

        private async void btnEdit_Clicked(object sender, EventArgs e)
        {
            string id = ((ImageButton)sender).CommandParameter.ToString();
            var advert = await _advertRepository.GetById(id);

            if (advert == null)
            {
                await DisplayAlert("Uyarı!", "Veri kayboldu.", "Tamam");
            }
            advert.Id = id;

            await Navigation.PushModalAsync(new AdvertEditPage(advert));
        }

        private async void btnDelete_Clicked(object sender, EventArgs e)
        {
            var isOk = await DisplayAlert("Sil", "Seçilen ilanı silmek istediğinize emin misiniz?", "Evet", "Hayır");

            if (isOk)
            {
                string id = ((ImageButton)sender).CommandParameter.ToString();
                var isComplete = await _advertRepository.Delete(id);

                if (isComplete)
                {
                    await DisplayAlert("Başarılı.", "Silme işlemi başarılı.", "Tamam");
                }
                else
                {
                    await DisplayAlert("Başarısız.", "Silme işlemi başarısız.", "Tamam");
                }
                OnAppearing();
            }
        }
    }
}