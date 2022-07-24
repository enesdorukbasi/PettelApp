using Pettel.Models;
using Realms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Xamarin.Forms.Xaml;

namespace Pettel.Pages.BasketAndSalesPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddToAddress : ContentPage
    {
        public AddToAddress()
        {
            InitializeComponent();
        }

        private async void btnSave_Clicked(object sender, EventArgs e)
        {
            string province, district, fullAddress;
            if (String.IsNullOrEmpty(lblProvince.Text) && String.IsNullOrEmpty(lblDistrict.Text) && String.IsNullOrEmpty(lblFullAddress.Text))
            {
                province = pckProvince.SelectedItem.ToString();
                district = pckDistrict.SelectedItem.ToString();
                fullAddress = txtFullAddress.Text;
            }
            else
            {
                province = lblProvince.Text;
                district = lblDistrict.Text;
                fullAddress = lblFullAddress.Text;
            }

            var realmDB = Realm.GetInstance();
            var myAddresses = realmDB.All<Address>().ToList();

            var maxAddressId = 0;
            if (myAddresses.Count != 0)
            {
                maxAddressId = myAddresses.Max(x => x.Id) + 1;
            }

            var isAdded = realmDB.All<Address>().Where(x => x.AddressTitle == txtTitle.Text);
            Location location1 = await Geolocation.GetLastKnownLocationAsync();

            if (isAdded.Count() == 0)
            {
                var Address = new Address
                {
                    Id = maxAddressId,
                    AddressTitle = txtTitle.Text,
                    Province = province,
                    District = district,
                    FullAddress = fullAddress,
                    Latitude = location1.Latitude,
                    Longitude = location1.Longitude
                };
                realmDB.Write(() =>
                {
                    realmDB.Add(Address);
                });

                DeliveryInfoEntryPage deliveryInfoEntryPage = new DeliveryInfoEntryPage();
                MyAddressListPage myAddressListPage = new MyAddressListPage();
                deliveryInfoEntryPage.AddressData();
                myAddressListPage.AddressData();
                await Navigation.PopAsync();
            }
            else
            {
                await DisplayAlert("Uyarı!", "Bu başlığı kullanan bir adres zaten var.", "Tamam");
            }

        }

        private async void btnMoveTheMap_Clicked(object sender, EventArgs e)
        {
            if(String.IsNullOrEmpty(lblProvince.Text) && String.IsNullOrEmpty(lblDistrict.Text))
            {
                Location location1 = await Geolocation.GetLastKnownLocationAsync();
                if (location1 == null)
                {
                    location1 = await Geolocation.GetLocationAsync(new GeolocationRequest
                    {
                        DesiredAccuracy = GeolocationAccuracy.Medium,
                        Timeout = TimeSpan.FromSeconds(30)
                    });
                }
                var placeMarks = await Geocoding.GetPlacemarksAsync(location1.Latitude, location1.Longitude);
                var placeMark = placeMarks?.FirstOrDefault();

                string Province = "İl : " + placeMark.AdminArea;
                string District = "İlçe : " + placeMark.SubAdminArea;
                string FullAddress = "Tam Adres : " + placeMark.SubLocality + " Mahalle, " + placeMark.Thoroughfare + " Sokak, " + placeMark.SubThoroughfare + " - " + placeMark.PostalCode + " " + placeMark.SubAdminArea + "/" +
                    placeMark.AdminArea + "/" + placeMark.CountryName;

                btnMoveTheMap.Text = "Konumumu Kullanma";

                lblProvince.Text = Province;
                lblDistrict.Text = District;
                lblFullAddress.Text = FullAddress;

                txtFullAddress.IsVisible = false;
                pckDistrict.IsVisible = false;
                pckProvince.IsVisible = false;

                lblDistrict.IsVisible = true;
                lblProvince.IsVisible = true;
                lblFullAddress.IsVisible = true;
            }
            else
            {
                btnMoveTheMap.Text = "Konumumu Kullan";

                lblProvince.Text = null;
                lblFullAddress.Text = null;
                lblDistrict.Text = null;

                lblDistrict.IsVisible = false;
                lblProvince.IsVisible = false;
                lblFullAddress.IsVisible = false;

                txtFullAddress.IsVisible = true;
                pckDistrict.IsVisible = true;
                pckProvince.IsVisible = true;
            }
        }
    }
}
