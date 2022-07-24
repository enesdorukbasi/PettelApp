using Pettel.Models;
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
    public partial class MyAddressListPage : ContentPage
    {
        public MyAddressListPage()
        {
            InitializeComponent();
            AddressData();
            lstAddresses.RefreshCommand = new Command(() =>
            {
                AddressData();
            });
        }
        public void AddressData()
        {
            var realmDB = Realm.GetInstance();
            var myAddresses = realmDB.All<Address>().ToList();

            if(myAddresses.Count <= 0)
            {
                lblMessage.Text = "Henüz adres eklemediniz.";
                lblMessage.IsVisible = true;
                lstAddresses.ItemsSource = null;
                lstAddresses.IsRefreshing = false;
            }
            else
            {
                lblMessage.IsVisible = false;
                lstAddresses.ItemsSource = null;
                lstAddresses.ItemsSource = myAddresses;
                lstAddresses.IsRefreshing = false;
            }
        }

        private void btnDelete_Clicked(object sender, EventArgs e)
        {
            var realmDB = Realm.GetInstance();

            string id = ((ImageButton)sender).CommandParameter.ToString();

            var selectedAddressesItem = realmDB.All<Address>().First(x => x.Id == Convert.ToInt32(id));

            try
            {
                using (var db = realmDB.BeginWrite())
                {
                    realmDB.Remove(selectedAddressesItem);
                    db.Commit();
                }
                var addressList = realmDB.All<Address>().ToList();
                AddressData();
            }
            catch { }
        }

        private async void ToolBarItemCreateAddress_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddToAddress());
        }

    }
}