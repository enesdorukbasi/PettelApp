using Pettel.Models;
using Pettel.Repositories;
using Pettel.Repositories.PetPropertyRepositories;
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
    public partial class AdvertEditPage : ContentPage
    {
        PetTypeRepository _petTypeRepository = new PetTypeRepository();
        PetBreedReposiroty _petBreedReposiroty = new PetBreedReposiroty();
        PetSizeRepository _petSizeRepository = new PetSizeRepository();
        PetGenderRepository _petGenderRepository = new PetGenderRepository();
        AdvertRepository _advertRepository = new AdvertRepository();

        public AdvertEditPage(Advert advert)
        {
            InitializeComponent();

            pckPetType.ItemsSource = _petTypeRepository.GetAll();
            pckPetBreed.ItemsSource = _petBreedReposiroty.GetAll();
            pckPetSize.ItemsSource = _petSizeRepository.GetAll();
            pckPetGender.ItemsSource = _petGenderRepository.GetAll();

            lblId.Text = advert.Id;
            lblImg.Text = advert.Image;
            lblEmail.Text = advert.UserEmail;
            txtTitle.Text = advert.Title;
            txtExplanation.Text = advert.Explanation;
            txtName.Text = advert.PetName;
            pckPetType.SelectedItem = advert.PetType;
            pckPetBreed.SelectedItem = advert.PetBreed;
            pckPetSize.SelectedItem = advert.PetSize;
            pckPetGender.SelectedItem = advert.PetGender;
            txtPetAge.Text = advert.PetAge.ToString();
        }

        private void pckPetType_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selected = pckPetType.SelectedItem.ToString();
            pckPetBreed.ItemsSource = _petBreedReposiroty.GetSelectedTypeToBreed(selected);
        }

        private async void btnSave_Clicked(object sender, EventArgs e)
        {
            string id, image, userEmail, title, explanation, petName, petType, petBreed, petSize, petGender ;
            int petAge;

            id = lblId.Text;
            image = lblImg.Text;
            userEmail = lblEmail.Text;
            title = txtTitle.Text;
            explanation = txtExplanation.Text;
            petName = txtName.Text;
            petType = pckPetType.SelectedItem.ToString();
            petBreed = pckPetBreed.SelectedItem.ToString();
            petSize = pckPetSize.SelectedItem.ToString();
            petGender = pckPetGender.SelectedItem.ToString();
            petAge = Int32.Parse(txtPetAge.Text);

            if (String.IsNullOrEmpty(userEmail))
            {
                await DisplayAlert("Hata!", "Bir hatayla karşılaşıldı, uygulamayı kapatıp yeniden açmanız önerilir.", "Tamam");

                Application.Current.Quit();
            }

            Advert advert = new Advert();
            advert.Id = id;
            advert.Image = image;
            advert.UserEmail = userEmail;
            advert.Title = title;
            advert.Explanation = explanation;
            advert.PetName = petName;
            advert.PetType = petType;
            advert.PetBreed = petBreed;
            advert.PetSize = petSize;
            advert.PetGender = petGender;
            advert.PetAge = petAge;

            var isEdit = await _advertRepository.Update(advert);

            if (isEdit)
            {
                await DisplayAlert("Başarılı.", "Düzenleme başarıyla kaydedildi.", "Tamam");
                await Navigation.PopModalAsync();
            }
            else
            {
                await DisplayAlert("Başarısız", "Düzenleme yapılamadı.", "Tamam");
            }
        }

    }
}