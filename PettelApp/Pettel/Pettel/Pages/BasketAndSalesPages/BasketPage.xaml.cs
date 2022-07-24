using Pettel.Models;
using Pettel.Pages.PetShop;
using Pettel.Repositories;
using Realms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Pettel.Pages.BasketAndSalesPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BasketPage : ContentPage
    {
        ProductRepository _productRepository = new ProductRepository();

        public BasketPage()
        {
            InitializeComponent();

            RefreshBasket();
        }
        public void RefreshBasket()
        {
            var realmDB = Realm.GetInstance();
            var basketList = realmDB.All<Basket>().ToList();

            if(basketList.Count <= 0)
            {
                lblMessage.Text = "Sepetinizde henüz hiç ürün bulunmamaktadır.";
                lblMessage.IsVisible = true;
                lstBasket.ItemsSource = null;
                lblTotalPriceTxt.IsVisible = false;
                lblBasketTotalPrice.IsVisible = false;
                btnAcceptBasket.IsVisible = false;
            }
            else
            {
                lblMessage.IsVisible = false;
                lstBasket.ItemsSource = basketList;
                double basketTotalPrice = 0;
                foreach(Basket i in basketList)
                {
                    basketTotalPrice += i.TotalPrice;
                }
                lblBasketTotalPrice.Text = basketTotalPrice.ToString();
                lblTotalPriceTxt.IsVisible = true;
                lblBasketTotalPrice.IsVisible = true;
                btnAcceptBasket.IsVisible = true;
            }
        }

        private void btnDeleteBasketItem_Tapped(object sender, EventArgs e)
        {
            var realmDB = Realm.GetInstance();

            Frame frmClicked = (Frame)sender;
            var item = (TapGestureRecognizer)frmClicked.GestureRecognizers[0];
            string id = item.CommandParameter.ToString();

            var selectedBasketItem = realmDB.All<Basket>().First(x => x.Id == Convert.ToInt32(id));

            try
            {
                using (var db = realmDB.BeginWrite())
                {
                    realmDB.Remove(selectedBasketItem);
                    db.Commit();
                }
                var basketList = realmDB.All<Basket>().ToList();
                RefreshBasket();
            }
            catch { }
        }

        private async void btnGoDetailsPage_Tapped(object sender, EventArgs e)
        {
            var realmDB = Realm.GetInstance();

            Frame frmClicked = (Frame)sender;
            var item = (TapGestureRecognizer)frmClicked.GestureRecognizers[0];
            string ProductId = item.CommandParameter.ToString();

            Product currentProduct = await _productRepository.GetById(ProductId);

            await Navigation.PushModalAsync(new ProductDetailsPage(currentProduct));
        }

        private void btnAcceptBasket_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new DeliveryInfoEntryPage());
        }
    }
}