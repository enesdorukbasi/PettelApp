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
    public class Images
    {
        public string Title { get; set; }
        public string Url { get; set; }
    }

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EmergencyAlertDetailsPage : ContentPage
    {
        Emergency currentEmergency = new Emergency();
        UserRepository _userRepository = new UserRepository();

        public EmergencyAlertDetailsPage(Emergency emergency)
        {
            InitializeComponent();
            currentEmergency = emergency;

            List<Images> Images = new List<Images>();
            if (emergency.Image1 != null)
            {
                Images.Add(new Images() { Title = "Image1", Url = emergency.Image1 });
            }
            if (emergency.Image2 != null)
            {
                Images.Add(new Images() { Title = "Image2", Url = emergency.Image2 });
            }
            if (emergency.Image3 != null)
            {
                Images.Add(new Images() { Title = "Image3", Url = emergency.Image3 });
            }

            EmergencyImages.ItemsSource = Images;

            lblTitle.Text = emergency.Title;
            lblDatetime.Text = emergency.Datetime.ToString();
            lblContent.Text = emergency.Content;
            lblProvince.Text = emergency.Province;
            lblDistrict.Text = emergency.District;
            lblFullAddress.Text = emergency.FullAddress;
            lblLatitude.Text = emergency.Latitude.ToString();
            lblLongitude.Text = emergency.Longitude.ToString();

        }

        private async void btnMoveTheMap_Clicked(object sender, EventArgs e)
        {
            var result = await Geolocation.GetLocationAsync(new GeolocationRequest(GeolocationAccuracy.Best, TimeSpan.FromMinutes(1)));
            result.Latitude = currentEmergency.Latitude;
            result.Longitude = currentEmergency.Longitude;
            await result.OpenMapsAsync();
        }

        [Obsolete]
        private async void btnOpenWhatsapp_Clicked(object sender, EventArgs e)
        {
            try
            {
                Models.User user = await _userRepository.GetUserTypeByEmail(currentEmergency.EMail);
                var uriString = "whatsapp://send?phone=" + user.PhoneNumber;
                string message = "Merhaba, '" + currentEmergency.Title + "' başlıklı acil durum ile ilgili mesaj atıyorum.";

                if (!string.IsNullOrWhiteSpace(message))
                    uriString += "&text=" + message;

                Device.OpenUri(new Uri(uriString));

            }
            catch
            {
                await DisplayAlert("Hata!", "Telefonunuzda Whatsapp Messanger uygulaması yüklü olmayabilir.", "Tamam");
            }
        }

        private async void btnCallNumber_Clicked(object sender, EventArgs e)
        {
            Models.User user = await _userRepository.GetUserTypeByEmail(currentEmergency.EMail);
            try
            {
                PhoneDialer.Open(user.PhoneNumber);
            }
            catch (ArgumentNullException)
            {
                // Number was null or white space
                await DisplayAlert("Uyarı!", "Telefon numarası ekli değil! İlanı bildirin.", "Tamam");
            }
            catch (FeatureNotSupportedException)
            {
                // Phone Dialer is not supported on this device.
                await DisplayAlert("Uyarı!", "Telefon çevirici bu cihazda desteklenmiyor.", "Tamam");
            }
            catch (Exception)
            {
                // Other error has occurred.
                await DisplayAlert("Uyarı!", "Hata ile ilgili iletişime geçiniz.", "Tamam");
            }
        }
    }
}