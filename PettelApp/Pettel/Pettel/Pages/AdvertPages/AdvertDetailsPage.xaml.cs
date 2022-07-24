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

namespace Pettel.Pages.AdvertPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AdvertDetailsPage : ContentPage
    {
        UserRepository _userRepository = new UserRepository();
        Advert currentAdvert = new Advert();
        string phoneNumber = "";

        public AdvertDetailsPage(Advert advert,bool IsMyAdvert)
        {
            InitializeComponent();

            imgAdvertImage.Source = advert.Image;
            lblTitle.Text = advert.Title;
            lblPetName.Text = advert.PetName;
            lblEmail.Text = advert.UserEmail;
            lblExplanation.Text = advert.Explanation;
            lblAge.Text = advert.PetAge.ToString();
            lblSize.Text = advert.PetSize;
            lblGender.Text = advert.PetGender;
            lblProvince.Text = advert.Province;
            lblDistrict.Text = advert.District;

            if(IsMyAdvert == false)
            {
                currentAdvert = advert;
                var phoneNumber1 = _userRepository.GetByEMail(currentAdvert.UserEmail).ToString();
                phoneNumber = phoneNumber1.ToString();
            }
            else
            {
                btnCallNumber.IsVisible = false;
                btnOpenWhatsapp.IsVisible = false;
            }

        }

        [Obsolete]
        private void btnOpenWhatsapp_Clicked(object sender, EventArgs e)
        {
            try
            {
                phoneNumber = Preferences.Get("userPhoneNumber", null);
                var uriString = "whatsapp://send?phone=" + phoneNumber;
                string message = "Merhaba, '" + currentAdvert.Title + "' başlıklı ilanınız ile ilgili mesaj atıyorum.";

                if (!string.IsNullOrWhiteSpace(message))
                    uriString += "&text=" + message;

                Device.OpenUri(new Uri(uriString));

            }
            catch
            {
                DisplayAlert("Hata!", "Telefonunuzda Whatsapp Messanger uygulaması yüklü olmayabilir.", "Tamam");
            }
        }

        private void btnCallNumber_Clicked(object sender, EventArgs e)
        {
            try
            {
                phoneNumber = Preferences.Get("userPhoneNumber", null);
                PhoneDialer.Open(phoneNumber);
            }
            catch (ArgumentNullException)
            {
                // Number was null or white space
                DisplayAlert("Uyarı!", "Telefon numarası ekli değil! İlanı bildirin.", "Tamam");
            }
            catch (FeatureNotSupportedException)
            {
                // Phone Dialer is not supported on this device.
                DisplayAlert("Uyarı!", "Telefon çevirici bu cihazda desteklenmiyor.", "Tamam");
            }
            catch (Exception)
            {
                // Other error has occurred.
                DisplayAlert("Uyarı!", "Hata ile ilgili iletişime geçiniz.", "Tamam");
            }
        }
    }
}