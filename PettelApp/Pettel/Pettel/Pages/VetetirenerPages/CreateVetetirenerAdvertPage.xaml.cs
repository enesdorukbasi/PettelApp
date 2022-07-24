using Pettel.Models;
using Pettel.Models.VeterinaryClinicProperties;
using Pettel.Repositories;
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

namespace Pettel.Pages.VetetirenerPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreateVetetirenerAdvertPage : ContentPage
    {
        VetetirenerRepository _vetetirenerRepository = new VetetirenerRepository();
        bool isGettingLocation;
        string vetetirenerId;
        MediaFile mediaFile;

        public CreateVetetirenerAdvertPage(bool isAddTreatment, Vetetirener vetetirener)
        {
            InitializeComponent();
            UseMarked();
            lstTreatments.RefreshCommand = new Command(() =>
            {
                OnAppearing();
            });
            pckClinicalTreatmentType.ItemsSource = null;
            pckClinicalTreatmentType.ItemsSource = _vetetirenerRepository.GetAllTypesOfClinicalTreatment();

            if (isAddTreatment)
            {
                stkClinicalTreatment.IsVisible = true;
                stkVetetirenerAdvert.IsVisible = false;
                vetetirenerId = vetetirener.Id;
            }
            else
            {
                DisplayAlert("Önemli!", "Bu ilan oluşturulurken kliniğinizde olmanız gerekmektedir. Aksi takdirde konumunuz yanlış alınacaktır.", "Tamam");
            }
        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();

            string email = Preferences.Get("currentUserEmail", string.Empty);
            var myTreatmentList = await _vetetirenerRepository.GetAllClinicalTreatmentInformation(email);

            if(myTreatmentList.Count > 0)
            {
                lstTreatments.ItemsSource = null;
                lstTreatments.ItemsSource = myTreatmentList;
                lstTreatments.IsRefreshing = false;
            }
            else
            {
                lstTreatments.ItemsSource = null;
                lstTreatments.IsRefreshing = false;
            }
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
                await DisplayAlert("Uyarı!", "Lokasyon bilgisi alınamıyor. Bu özellik için önemli bir izindir.", "Tamam");
            }

        }

        private async void btnSave_Clicked(object sender, EventArgs e)
        {
            string title, explanation, email, image;
            if (String.IsNullOrEmpty(txtTitle.Text) || String.IsNullOrEmpty(txtExplanation.Text))
            {
                await DisplayAlert("Uyarı!","Başlık ve açıklama alanları boş geçilemez.", "Tamam");
                return;
            }
            if(String.IsNullOrEmpty(Preferences.Get("currentUserEmail", string.Empty)))
            {
                await DisplayAlert("Hata!", "Bir sorunla karşılaşıldı. Uygulamayı kapatıp yeniden açınız", "Tamam");
                Application.Current.Quit();
            }
            if(String.IsNullOrEmpty(lblProvince.Text) || String.IsNullOrEmpty(lblDistrict.Text) || String.IsNullOrEmpty(lblFullAddress.Text))
            {
                await DisplayAlert("Uyarı!", "Konum bilgileriniz alınamıyor. Uygulama için konum izinleriniz kapalı olabilir. İzinleri verip yeniden deneyiniz.", "Tamam");
                return;
            }
            var location1 = await Geolocation.GetLocationAsync(new GeolocationRequest(GeolocationAccuracy.Best, TimeSpan.FromMinutes(1)));

            title = txtTitle.Text;
            explanation = txtExplanation.Text;
            email = Preferences.Get("currentUserEmail", string.Empty);


            Vetetirener vetetirener = new Vetetirener();
            vetetirener.Title = title;
            vetetirener.Explanation = explanation;
            vetetirener.EMail = email;
            vetetirener.Province = lblProvince.Text;
            vetetirener.District = lblDistrict.Text;
            vetetirener.FullAddress = lblFullAddress.Text;
            vetetirener.Latitude = location1.Latitude;
            vetetirener.Longitude = location1.Longitude;

            if (mediaFile != null)
            {
                image = await _vetetirenerRepository.UploadImage(mediaFile.GetStream(), Path.GetFileName(mediaFile.Path));
                vetetirener.Image = image;
            }
            else
            {
                await DisplayAlert("Uyarı!", "Görsel, daha iyi bir kullanıcı deneyimi için zorunlu bir alandır.", "Tamam");
            }

            var isSaved = await _vetetirenerRepository.Save(vetetirener);
            vetetirenerId = Preferences.Get("VetetirenerKey", string.Empty);

            if (isSaved)
            {
                var nextPage = await DisplayAlert("Kayıt Oluşturuldu.", "İlan kaydınız oluşturuldu. Artık listeleme ekranından kullanıcılar görebilecek. Daha fazla kullanıcı için fiyatlandırma yapmak ister misiniz?", "Evet", "Hayır");

                if (nextPage)
                {
                    OnAppearing();
                    stkVetetirenerAdvert.IsVisible = false;
                    stkClinicalTreatment.IsVisible = true;
                }
                else
                {
                    await Navigation.PopAsync();
                }
            }
            else
            {
                await DisplayAlert("Hata!", "Bir hata ile karşılaşıldı.", "Tamam");
            }
        }

        private void btnAddTreatment_Clicked(object sender, EventArgs e)
        {
            pckClinicalTreatmentType.ItemsSource = null;
            pckClinicalTreatmentType.ItemsSource = _vetetirenerRepository.GetAllTypesOfClinicalTreatment();

            lstTreatments.IsVisible = false;
            stkAddTreatment.IsVisible = true;
        }

        private async void btnSaveTreatment_Clicked(object sender, EventArgs e)
        {
            string treatmentType, treatmentContent, email;
            decimal treatmentPrice;

            if(String.IsNullOrEmpty(txtTreatmentContent.Text) || String.IsNullOrEmpty(txtTreatmentPrice.Text) || String.IsNullOrEmpty(pckClinicalTreatmentType.SelectedItem.ToString()))
            {
                await DisplayAlert("Uyarı!", "Tedavi tipi, açıklaması veya fiyatı boş geçilemez.", "Tamam");
                return;
            }
            if (String.IsNullOrEmpty(Preferences.Get("currentUserEmail", string.Empty)))
            {
                await DisplayAlert("Hata!", "Bir sorunla karşılaşıldı. Uygulamayı kapatıp yeniden açınız", "Tamam");
                Application.Current.Quit();
            }

            treatmentType = pckClinicalTreatmentType.SelectedItem.ToString();
            treatmentContent = txtTreatmentContent.Text;
            treatmentPrice = decimal.Parse(txtTreatmentPrice.Text);
            email = Preferences.Get("currentUserEmail", string.Empty);

            ClinicalTreatmentInformation clinicalTreatmentInformation = new ClinicalTreatmentInformation();
            clinicalTreatmentInformation.ClinicalTreatmentType = treatmentType;
            clinicalTreatmentInformation.Content = treatmentContent;
            clinicalTreatmentInformation.Price = treatmentPrice;
            clinicalTreatmentInformation.EMail = email;
            clinicalTreatmentInformation.VetetirenerId = vetetirenerId;

            var isSaved = await _vetetirenerRepository.SaveTreatment(clinicalTreatmentInformation);
            if (isSaved)
            {
                var next = await DisplayAlert("Kayıt Başarılı", "Tedavi tipinin kaydı tamamlandı. İlanınızda daha fazla tedavi tipi görünmesini ister misiniz? Bu ilanınızı daha kolay bulunabilir hale getirecektir.", "Evet", "Hayır");
                if (next)
                {
                    txtTreatmentContent.Text = null;
                    pckClinicalTreatmentType.SelectedItem = null;
                    txtTreatmentPrice.Text = null;

                    stkAddTreatment.IsVisible = false;
                    return;
                }
                else
                {
                    await Navigation.PopAsync();
                }
            }
            else
            {
                await DisplayAlert("Hata!", "Bir hata ile karşılaşıldı.", "Tamam");
            }
        }

        private void btnCancel_Clicked(object sender, EventArgs e)
        {
            txtTreatmentContent.Text = null;
            txtTreatmentPrice.Text = null;
            pckClinicalTreatmentType.SelectedItem = null;
            lstTreatments.IsVisible = true;
            stkAddTreatment.IsVisible = false;
        }

        private async void VetetirenerImageTap_Tapped(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();

            try
            {
                mediaFile = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions
                {
                    PhotoSize = PhotoSize.Custom
                });
                if (mediaFile == null)
                {
                    return;
                }

                VetetirenerImage.Source = ImageSource.FromStream(() =>
                {
                    return mediaFile.GetStream();
                });
            }
            catch { }
        }
    }
}