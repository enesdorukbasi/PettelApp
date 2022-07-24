using Pettel.Models;
using Pettel.Pages.Interfaces;
using Pettel.Repositories;
using Pettel.Repositories.PetPropertyRepositories;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Pettel.Pages.EmergencyPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreateEmergencyAlertPage : ContentPage
    {
        EmergencyAlertRepository _emergencyAlertRepository = new EmergencyAlertRepository();
        PetTypeRepository _petTypeRepository = new PetTypeRepository();
        MediaFile mediaFile1;
        MediaFile mediaFile2;
        MediaFile mediaFile3;
        bool isGettingLocation;

        public CreateEmergencyAlertPage()
        {
            InitializeComponent();
            UseMarked();
            DisplayAlert("Önemli", "Bulunduğunuz noktanın konumu alınacaktır. Sapma olmaması için hareket halinde olmamanız önerilir.", "Tamam");
            pckPetType.ItemsSource = _petTypeRepository.GetAll();
        }

        private async void UseMarked()
        {
            isGettingLocation = true;
            var status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
            if (status == PermissionStatus.Granted)
            {
                try
                {
                    while (isGettingLocation)
                    {
                        //Xamarin.Essentials.Location location1 = await Geolocation.GetLastKnownLocationAsync();
                        var location1 = await Geolocation.GetLocationAsync(new GeolocationRequest(GeolocationAccuracy.Best, TimeSpan.FromMinutes(1)));

                        if (location1 == null)
                        {
                            //location1 = await Geolocation.GetLocationAsync(new GeolocationRequest
                            //{
                            //    DesiredAccuracy = GeolocationAccuracy.Medium,
                            //    Timeout = TimeSpan.FromSeconds(30)
                            //});
                            await DisplayAlert("Uyarı!", "Konum bilgisi alınamadı. Konum izinlerinin sağlandığından emin olun.", "Tamam");
                        }
                        var placeMarks = await Geocoding.GetPlacemarksAsync(location1.Latitude, location1.Longitude);
                        var placeMark = placeMarks?.FirstOrDefault();

                        string Province = placeMark.AdminArea;
                        string District = placeMark.SubAdminArea;
                        string FullAddress = placeMark.SubLocality + " Mahalle, " + placeMark.Thoroughfare + " Sokak, " + placeMark.SubThoroughfare + " - " + placeMark.PostalCode + " " + placeMark.SubAdminArea + "/" +
                            placeMark.AdminArea + "/" + placeMark.CountryName;

                        lblProvince.Text = Province;
                        lblDistrict.Text = District;
                        lblFullAddress.Text = FullAddress;
                        lblLatitude.Text = location1.Latitude.ToString();
                        lblLongitude.Text = location1.Longitude.ToString();
                        //location1 = null;
                        await Task.Delay(3000);
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

        private async void btnMoveTheMap_Clicked(object sender, EventArgs e)
        {
            var result = await Geolocation.GetLocationAsync(new GeolocationRequest(GeolocationAccuracy.Best, TimeSpan.FromMinutes(1)));
            await result.OpenMapsAsync();
        }

        private async void btnSave_Clicked(object sender, EventArgs e)
        {
            isGettingLocation = false;

            string title, content, province, district, fullAddress, userEmail, petType,image1,image2,image3;
            double latitude, longitude;
            userEmail = Preferences.Get("currentUserEmail", string.Empty);

            if (String.IsNullOrEmpty(txtTitle.Text) && String.IsNullOrEmpty(txtContent.Text))
            {
                await DisplayAlert("Uyarı!", "Kullanıcıları yeteri kadar bilgilendirmek amacıyla başlık ve özet alanları boş geçilemez.", "Tamam");
                return;
            }
            if (String.IsNullOrEmpty(lblLatitude.Text) && String.IsNullOrEmpty(lblLongitude.Text))
            {
                await DisplayAlert("Uyarı!", "Konumunuza erişilemiyor. Konum hizmetine izin vermemiş olabilirsiniz. İzinleri sağladıktan sonra yeniden deneyiniz.", "Tamam");
                return;
            }
            if (String.IsNullOrEmpty(userEmail))
            {
                await DisplayAlert("Hata!", "Bir hatayla karşılaşıldı, uygulamayı kapatıp yeniden açmanız önerilir.", "Tamam");

                Application.Current.Quit();
            }

            title = txtTitle.Text;
            content = txtContent.Text;
            province = lblProvince.Text;
            district = lblDistrict.Text;
            fullAddress = lblFullAddress.Text;
            petType = pckPetType.SelectedItem.ToString();
            latitude = double.Parse(lblLatitude.Text);
            longitude = double.Parse(lblLongitude.Text);

            Emergency emergency = new Emergency();
            emergency.Title = title;
            emergency.Content = content;
            emergency.Province = province;
            emergency.District = district;
            emergency.FullAddress = fullAddress;
            emergency.Latitude = latitude;
            emergency.Longitude = longitude;
            emergency.PetType = petType;
            emergency.EMail = userEmail;
            emergency.Datetime = DateTime.Now;
            emergency.IsActive = true;
            if (mediaFile1 == null && mediaFile2 == null && mediaFile3 == null)
            {
                await DisplayAlert("Uyarı!", "En az 1 adet ürün görseli eklemeniz gerekmektedir.", "Tamam");
                return;
            }
            if (mediaFile1 != null)
            {
                image1 = await _emergencyAlertRepository.UploadImage(mediaFile1.GetStream(), Path.GetFileName(mediaFile1.Path));
                emergency.Image1 = image1;
            }
            if (mediaFile2 != null)
            {
                image2 = await _emergencyAlertRepository.UploadImage(mediaFile2.GetStream(), Path.GetFileName(mediaFile2.Path));
                emergency.Image2 = image2;
            }
            if (mediaFile3 != null)
            {
                image3 = await _emergencyAlertRepository.UploadImage(mediaFile3.GetStream(), Path.GetFileName(mediaFile3.Path));
                emergency.Image3 = image3;
            }

            var isSaved = await _emergencyAlertRepository.Save(emergency);

            if (isSaved)
            {
                await DisplayAlert("Paylaşma Tamamlandı", "Ürün paylaşıldı.", "Tamam");
                mediaFile1 = null;
                image1 = null;
                mediaFile2 = null;
                image2 = null;
                mediaFile3 = null;
                image3 = null;
                try
                {
                    await Navigation.PopAsync();
                }
                catch { }
            }
            else
            {
                await DisplayAlert("Hata!", "Bir hata ile karşılaşıldı.", "Tamam");
            }
        }

        private async void EmergencyAlertImageTap1_Tapped(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();

            try
            {
                mediaFile1 = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions
                {
                    PhotoSize = PhotoSize.Custom
                });
                if (mediaFile1 == null)
                {
                    return;
                }

                EmergencyAlertImage1.Source = ImageSource.FromStream(() =>
                {
                    return mediaFile1.GetStream();
                });
            }
            catch { }
        }

        private async void EmergencyAlertImageTap2_Tapped(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();

            try
            {
                mediaFile2 = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions
                {
                    PhotoSize = PhotoSize.Custom
                });
                if (mediaFile2 == null)
                {
                    return;
                }

                EmergencyAlertImage2.Source = ImageSource.FromStream(() =>
                {
                    return mediaFile2.GetStream();
                });
            }
            catch { }
        }

        private async void EmergencyAlertImageTap3_Tapped(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();

            try
            {
                mediaFile3 = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions
                {
                    PhotoSize = PhotoSize.Custom
                });
                if (mediaFile3 == null)
                {
                    return;
                }

                EmergencyAlertImage3.Source = ImageSource.FromStream(() =>
                {
                    return mediaFile3.GetStream();
                });
            }
            catch { }
        }
    }
}