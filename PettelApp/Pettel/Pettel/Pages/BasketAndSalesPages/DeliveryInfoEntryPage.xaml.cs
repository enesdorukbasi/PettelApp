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
    public partial class DeliveryInfoEntryPage : ContentPage
    {
        public DeliveryInfoEntryPage()
        {
            InitializeComponent();
            AddressData();
        }

        public void AddressData()
        {
            pckAddress.ItemsSource = null;
            var realmDB = Realm.GetInstance();
            var addressList = realmDB.All<Address>().ToList();
            var addressTitleList = addressList.Select(x => x.AddressTitle).ToList();
            pckAddress.ItemsSource = addressTitleList;
        }

        private void btnCreateAddress_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AddToAddress());
        }

        private void pckAddress_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedItem = pckAddress.SelectedItem.ToString();
            
            var realmDB = Realm.GetInstance();
            var selected = realmDB.All<Address>().Where(x => x.AddressTitle == selectedItem).ToList();

            if (selected.Count > 0)
            {
                Address selectedAddress = selected[0];
                lblTitle.Text = selectedAddress.AddressTitle;
                lblProvince.Text = selectedAddress.Province;
                lblDistrict.Text = selectedAddress.District;
                lblFullAddress.Text = selectedAddress.FullAddress;
            }
            else
            {
                DisplayAlert("Uyarı!", "Seçilen adresin bilgileri girilmemiş.", "Tamam");
            }
        }
    }
}