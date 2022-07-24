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

namespace Pettel.Pages.EmergencyPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EmergencyListPage : ContentPage
    {
        EmergencyAlertRepository _emergencyAlertRepository = new EmergencyAlertRepository();

        public EmergencyListPage()
        {
            InitializeComponent();

            lstEmergencyAlert.RefreshCommand = new Command(() =>
            {
                OnAppearing();
            });
        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            var status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
            if (status == PermissionStatus.Granted)
            {
                //YES, now we have permission
                try
                {
                    var location1 = await Geolocation.GetLocationAsync(new GeolocationRequest(GeolocationAccuracy.Best, TimeSpan.FromMinutes(1)));
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
                    string Province = placeMark.AdminArea;
                    string District = placeMark.SubAdminArea;

                    var emergencyList = await _emergencyAlertRepository.GetAllByAddress(Province, District);
                    if (emergencyList.Count <= 0)
                    {
                        lblMessage.Text = "Bölgenizde şu an için aktif bir acil durum bildirisi yok.";
                        lblMessage.IsVisible = true;
                        lstEmergencyAlert.ItemsSource = null;
                        lstEmergencyAlert.IsRefreshing = false;
                    }
                    else
                    {
                        lblMessage.IsVisible = false;
                        lstEmergencyAlert.ItemsSource = null;
                        lstEmergencyAlert.ItemsSource = emergencyList;
                        lstEmergencyAlert.IsRefreshing = false;
                    }
                }
                catch
                {
                    await DisplayAlert("Uyarı!", "Lokasyon bilgisi alınamıyor. Konum servisinin açık olduğundan emin olun.", "Tamam");
                }
            }
            else
            {
                //Ok, maybe I will ask again
                await DisplayAlert("Uyarı!", "Lokasyon bilgisi alınamıyor. Bu alan için önemli bir izindir.", "Tamam");
            }
            
        }

        private async void btnCreateEmergencyAlert_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CreateEmergencyAlertPage());
        }

        private async void lstEmergencyAlert_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
            {
                return;
            }
            var emergency = e.Item as Emergency;
            await Navigation.PushModalAsync(new NavigationPage(new EmergencyAlertDetailsPage(emergency)));

            ((ListView)sender).SelectedItem = null;
        }
    }
}