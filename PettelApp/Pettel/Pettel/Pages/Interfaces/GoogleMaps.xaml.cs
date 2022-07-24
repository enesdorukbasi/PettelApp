using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Xamarin.Forms.Xaml;

namespace Pettel.Pages.Interfaces
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GoogleMaps : ContentPage
    {
        public GoogleMaps()
        {
            InitializeComponent();
            GeneratePins();
        }
        private async void GeneratePins()
        {
            //var location = await Geolocation.GetLastKnownLocationAsync();
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Medium);
                var location = await Geolocation.GetLocationAsync(request);
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

                string Province = placeMark.CountryName;
                string District = placeMark.FeatureName;
                string Dahası = placeMark.Thoroughfare;

                Pin pinTokyo = new Pin()
                {
                    Type = PinType.Generic,
                    Label = "Buradasınız",
                    Position = new Position(location.Latitude, location.Longitude),
                    Rotation = 33.3f,
                    Tag = "Konumunuz",
                };

                map.Pins.Add(pinTokyo);
                map.MoveToRegion(MapSpan.FromCenterAndRadius(pinTokyo.Position, Distance.FromMeters(5000)));
            }
            catch
            {
                await DisplayAlert("Uyarı!", "Konum alınırken bir sorunla karşılaşıldı. Lütfen ekranı yeniden yükleyiniz.", "Tamam");
            }
        }
    }
}