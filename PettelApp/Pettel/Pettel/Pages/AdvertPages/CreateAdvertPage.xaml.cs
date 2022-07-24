using Firebase.Storage;
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

namespace Pettel.Pages.AdvertPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreateAdvertPage : ContentPage
    {
        PetTypeRepository _petTypeRepository = new PetTypeRepository();
        PetBreedReposiroty _petBreedReposiroty = new PetBreedReposiroty();
        PetSizeRepository _petSizeRepository = new PetSizeRepository();
        PetGenderRepository _petGenderRepository = new PetGenderRepository();
        AdvertRepository _advertRepository = new AdvertRepository();
        MediaFile mediaFile;

        public CreateAdvertPage()
        {
            InitializeComponent();

            pckPetType.ItemsSource = _petTypeRepository.GetAll();
            pckPetBreed.ItemsSource = _petBreedReposiroty.GetAll();
            pckPetSize.ItemsSource = _petSizeRepository.GetAll();
            pckPetGender.ItemsSource = _petGenderRepository.GetAll();
        }

        private void pckPetType_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selected = pckPetType.SelectedItem.ToString();
            pckPetBreed.ItemsSource = _petBreedReposiroty.GetSelectedTypeToBreed(selected);
        }

        private async void btnSave_Clicked(object sender, EventArgs e)
        {
            string title, explanation, petName, petType, petBreed, petSize, petGender, userEmail, image;
            int petAge;

            title = txtTitle.Text;
            explanation = txtExplanation.Text;
            petName = txtName.Text;
            petType = pckPetType.SelectedItem.ToString();
            petBreed = pckPetBreed.SelectedItem.ToString();
            petSize = pckPetSize.SelectedItem.ToString();
            petGender = pckPetGender.SelectedItem.ToString();
            petAge = Int32.Parse(txtPetAge.Text);
            userEmail = Preferences.Get("currentUserEmail",string.Empty);

            if(String.IsNullOrEmpty(userEmail))
            {
                await DisplayAlert("Hata!", "Bir hatayla karşılaşıldı, uygulamayı kapatıp yeniden açmanız önerilir.","Tamam");

                Application.Current.Quit();
            }

            Advert advert = new Advert();
            advert.Title = title;
            advert.Explanation = explanation;
            advert.PetName = petName;
            advert.PetType = petType;
            advert.PetBreed = petBreed;
            advert.PetSize = petSize;
            advert.PetGender = petGender;
            advert.PetAge = petAge;
            advert.Datetime = DateTime.Now.ToString();
            advert.IsActive = true;
            advert.UserEmail = userEmail;
            if(mediaFile != null)
            {
                image  = await _advertRepository.UploadImage(mediaFile.GetStream(),Path.GetFileName(mediaFile.Path));
                advert.Image = image;
            }

            var isSaved = await _advertRepository.Save(advert);

            if (isSaved)
            {
                await DisplayAlert("Paylaşma Tamamlandı", "İlan başarıyla paylaşıldı.", "Tamam");
                mediaFile = null;
                image = null;
                try
                {
                    GoToHomePage();
                }
                catch { }
            }
            else
            {
                await DisplayAlert("Hata!", "Bir hata ile karşılaşıldı.", "Tamam");
            }
        }

        private async void GoToHomePage()
        {
            await Navigation.PopAsync();
        }

        private void btnNext_Clicked(object sender, EventArgs e)
        {
            StackEntry.IsVisible = false;
            StackImg.IsVisible = true;
        }

        private void btnPrev_Clicked(object sender, EventArgs e)
        {
            StackEntry.IsVisible = true;
            StackImg.IsVisible = false;
        }

        private async void AdvertImageTap_Tapped(object sender, EventArgs e)
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

                AdvertImage.Source = ImageSource.FromStream(() =>
                {
                    return mediaFile.GetStream();
                });
            }
            catch { }
        }

    }
}