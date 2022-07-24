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

namespace Pettel.Pages.PetShop
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MyProductListPage : ContentPage
    {
        ProductRepository _productRepository = new ProductRepository();

        public MyProductListPage()
        {
            InitializeComponent();
            lstProduct.RefreshCommand = new Command(() =>
            {
                OnAppearing();
            });
        }
        protected async override void OnAppearing()
        {
            string email = Preferences.Get("currentUserEmail", string.Empty);
            if(!String.IsNullOrEmpty(email))
            {
                var products = await _productRepository.GetAllByEMail(email);

                if (products.Count <= 0)
                {
                    lblMessage.Text = "Listelenecek ürün bulunmamaktadır.";
                    lblMessage.IsVisible = true;
                    lstProduct.ItemsSource = null;
                    lstProduct.IsRefreshing = false;
                }
                else
                {
                    lblMessage.IsVisible = false;
                    lstProduct.ItemsSource = null;
                    lstProduct.ItemsSource = products;
                    lstProduct.IsRefreshing = false;
                }
            }
            else
            {
                await DisplayAlert("Hata!", "Bir sorunla karşılaşıldı. Uygulamayı kapatıp yeniden açınız.", "Tamam");
                Application.Current.Quit();
            }
        }

        private void ToolBarItemSearch_Clicked(object sender, EventArgs e)
        {
            if (ProductSearchBar.IsVisible == false)
            {
                ProductSearchBar.IsVisible = true;
            }
            else
            {
                ProductSearchBar.IsVisible = false;
            }
        }

        private void ToolBarItemFilter_Clicked(object sender, EventArgs e)
        {

        }

        private async void btnCreateProduct_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CreateProductPage());
        }

        private async void ProductSearchBar_SearchButtonPressed(object sender, EventArgs e)
        {
            string email = Preferences.Get("currentUserEmail", string.Empty);
            if(!String.IsNullOrEmpty(email))
            {
                string search = ProductSearchBar.Text;
                if (!String.IsNullOrEmpty(search))
                {
                    var searchToList = await _productRepository.GetAllByEmailAndProductNameOrTags(email, search);
                    if (searchToList.Count <= 0)
                    {
                        lblMessage.Text = "Aranan kriterlere uygun bir ürün bulunamadı.";
                        lblMessage.IsVisible = true;
                        lstProduct.ItemsSource = null;
                        lstProduct.IsRefreshing = false;
                    }
                    else
                    {
                        lblMessage.IsVisible = false;
                        lstProduct.ItemsSource = null;
                        lstProduct.ItemsSource = searchToList;
                        lstProduct.IsRefreshing = false;
                    }
                }
                else
                {
                    OnAppearing();
                }
            }
            else
            {
                await DisplayAlert("Hata!", "Bir sorunla karşılaşıldı. Uygulamayı kapatıp yeniden açınız.", "Tamam");
                Application.Current.Quit();
            }
        }

        private async void ProductSearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            string email = Preferences.Get("currentUserEmail", string.Empty);
            if (!String.IsNullOrEmpty(email))
            {
                string search = ProductSearchBar.Text;
                if (!String.IsNullOrEmpty(search))
                {
                    var searchToList = await _productRepository.GetAllByEmailAndProductNameOrTags(email, search);
                    if (searchToList.Count <= 0)
                    {
                        lblMessage.Text = "Aranan kriterlere uygun bir ürün bulunamadı.";
                        lblMessage.IsVisible = true;
                        lstProduct.ItemsSource = null;
                        lstProduct.IsRefreshing = false;
                    }
                    else
                    {
                        lblMessage.IsVisible = false;
                        lstProduct.ItemsSource = null;
                        lstProduct.ItemsSource = searchToList;
                        lstProduct.IsRefreshing = false;
                    }
                }
                else
                {
                    OnAppearing();
                }
            }
            else
            {
                await DisplayAlert("Hata!", "Bir sorunla karşılaşıldı. Uygulamayı kapatıp yeniden açınız.", "Tamam");
                Application.Current.Quit();
            }
        }

        private async void lstProduct_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
            {
                return;
            }
            var product = e.Item as Product;
            await Navigation.PushModalAsync(new NavigationPage(new ProductDetailsPage(product)));

            ((ListView)sender).SelectedItem = null;
        }


        private async void btnEdit_Clicked(object sender, EventArgs e)
        {
            string id = ((ImageButton)sender).CommandParameter.ToString();
            var product = await _productRepository.GetById(id);

            if (product == null)
            {
                await DisplayAlert("Uyarı!", "Veri kayboldu.", "Tamam");
            }
            product.Id = id;

            await Navigation.PushModalAsync(new ProductEditPage(product));
        }

        private async void btnDelete_Clicked(object sender, EventArgs e)
        {
            var isOk = await DisplayAlert("Sil", "Seçilen ilanı silmek istediğinize emin misiniz?", "Evet", "Hayır");

            if (isOk)
            {
                string id = ((ImageButton)sender).CommandParameter.ToString();
                var isComplete = await _productRepository.Delete(id);

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