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

namespace Pettel.Pages.VetetirenerPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VetetirenerListPage : ContentPage
    {
        VetetirenerRepository _vetetirenerRepository = new VetetirenerRepository();
        bool isMarked = false;
        string province, district;
        public VetetirenerListPage()
        {
            InitializeComponent();

            lstVetetirener.RefreshCommand = new Command(() =>
            {
                OnAppearing();
            });
        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();

            var vetetirenerList = await _vetetirenerRepository.GetAllVetetirener(isMarked, province, district);
            if (vetetirenerList.Count > 0)
            {
                lblMessage.IsVisible = false;
                lstVetetirener.ItemsSource = null;
                lstVetetirener.ItemsSource = vetetirenerList;
                lstVetetirener.IsRefreshing = false;
            }
            else
            {
                if (!String.IsNullOrEmpty(province) && !String.IsNullOrEmpty(district))
                {
                    district = null;
                    OnAppearing();
                }
                lblMessage.Text = "Listelenebilecek bir veteriner ilanı bulunmamaktadır.";
                lblMessage.IsVisible = true;
                lstVetetirener.ItemsSource = null;
                lstVetetirener.IsRefreshing = false;
            }
        }

        private async void btnUseMarker_Clicked(object sender, EventArgs e)
        {
            if (isMarked)
            {
                btnUseMarker.IsEnabled = false;
                isMarked = false;
                province = null;
                district = null;
                btnUseMarker.IconImageSource = "marker.png";
                btnUseMarker.IsEnabled = true;
            }
            else
            {
                var status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
                if (status == PermissionStatus.Granted)
                {
                    try
                    {
                        btnUseMarker.IsEnabled = false;
                        var location1 = await Geolocation.GetLocationAsync(new GeolocationRequest(GeolocationAccuracy.Default, TimeSpan.FromMinutes(1)));
                        var placeMarks = await Geocoding.GetPlacemarksAsync(location1.Latitude, location1.Longitude);
                        var placeMark = placeMarks?.FirstOrDefault();
                        province = placeMark.AdminArea;
                        district = placeMark.SubAdminArea;
                        isMarked = true;
                        btnUseMarker.IconImageSource = "no_marker.png";
                        btnUseMarker.IsEnabled = true;
                    }
                    catch
                    {
                        await DisplayAlert("Uyarı!", "Lokasyon bilgisi alınamıyor. Konum servisinin açık olduğundan emin olun.", "Tamam");
                    }
                }
                else
                {
                    //Ok, maybe I will ask again
                    await DisplayAlert("Uyarı!", "Lokasyon bilgisi alınamıyor. Bu özellik için önemli bir izindir.", "Tamam");
                }

            }
            OnAppearing();
        }

        private async void lstVetetirener_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
            {
                return;
            }
            var vetetirener = e.Item as Vetetirener;
            await Navigation.PushModalAsync(new NavigationPage(new VetetirenerAdvertDetailsPage(vetetirener)));

            ((ListView)sender).SelectedItem = null;
        }
    }
}