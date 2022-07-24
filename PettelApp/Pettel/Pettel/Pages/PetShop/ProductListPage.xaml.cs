using Pettel.Models;
using Pettel.Models.UserProperties;
using Pettel.Pages.BasketAndSalesPages;
using Pettel.Repositories;
using Realms;
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
    public partial class ProductListPage : ContentPage
    {
        ProductRepository _productRepository = new ProductRepository();

        public ProductListPage()
        {
            InitializeComponent();


            lstProduct.RefreshCommand = new Command(() =>
            {
                OnAppearing();
            });
        }
        protected async override void OnAppearing()
        {
            var products = await _productRepository.GetAll();

            if(products.Count <= 0)
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

        private async void ToolBarBasketPage_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new BasketPage());
        }

        private async void ProductSearchBar_SearchButtonPressed(object sender, EventArgs e)
        {
            string search = ProductSearchBar.Text;
            if (!String.IsNullOrEmpty(search))
            {
                var searchToList = await _productRepository.GetAllByProductNameOrTags(search);
                if(searchToList.Count <= 0)
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

        private async void ProductSearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            string search = ProductSearchBar.Text;
            if (!String.IsNullOrEmpty(search))
            {
                var searchToList = await _productRepository.GetAllByProductNameOrTags(search);
                if (searchToList.Count <= 0)
                {
                    lblMessage.Text = "Aranan kriterlere uygun bir ürün bulunamadı.";
                    lblMessage.IsVisible = true;
                    lstProduct.ItemsSource = null;
                }
                else
                {
                    lblMessage.IsVisible = false;
                    lstProduct.ItemsSource = null;
                    lstProduct.ItemsSource = searchToList;
                }
            }
            else
            {
                OnAppearing();
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

        private async void btnAddBasket_Tapped(object sender, EventArgs e)
        {
            Frame frmClicked = (Frame)sender;
            var item = (TapGestureRecognizer)frmClicked.GestureRecognizers[0];
            string id = item.CommandParameter.ToString();

            var realmDB = Realm.GetInstance();
            var myBasket = realmDB.All<Basket>().ToList();

            var maxBasketItemId = 0;
            if(myBasket.Count != 0)
            {
                maxBasketItemId = myBasket.Max(x => x.Id) + 1;
            }

            var isAdded = realmDB.All<Basket>().Where(x => x.ProductId == id);
            Product currentProduct = await _productRepository.GetById(id);

            if(isAdded.Count() == 0)
            {
                var Basket = new Basket
                {
                    Id = maxBasketItemId,
                    Image = currentProduct.Image1,
                    ProductName = currentProduct.ProductName.ToString(),
                    TotalPrice = decimal.ToDouble(currentProduct.Price),
                    ProductId = id,
                    Number = 1
                };
                realmDB.Write(() =>
                {
                    realmDB.Add(Basket);
                });
            }
            else
            {
                try
                {
                    var isAdd = realmDB.All<Basket>().First(x => x.ProductId == id);
                    using (var db = realmDB.BeginWrite())
                    {
                        int number = isAdd.Number;
                        if (currentProduct.Number > number)
                        {
                            isAdd.Number = number+1;
                            isAdd.TotalPrice = decimal.ToDouble(currentProduct.Price * isAdd.Number);
                            db.Commit();
                        }
                        else
                        {
                            await DisplayAlert("Uyarı!", "Bu ürün sepete daha fazla eklenemiyor. Satıcının eklediği stok sınırına yaklaşılmış olabilir.", "Tamam");
                        }
                    }
                }
                catch { }
            }

        }

    }
}