using Pettel.Repositories;
using Pettel.Repositories.PetPropertyRepositories;
using Rg.Plugins.Popup.Extensions;
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
    public partial class AdvertsFilterPromptPage : Rg.Plugins.Popup.Pages.PopupPage
    {
        PetTypeRepository _petTypeRepository = new PetTypeRepository();
        PetBreedReposiroty _petBreedReposiroty = new PetBreedReposiroty();
        PetGenderRepository _petGenderRepository = new PetGenderRepository();
        PetSizeRepository _petSizeRepository = new PetSizeRepository();

        public EventHandler<List<string>> DataEventHandler; 


        public AdvertsFilterPromptPage()
        {
            InitializeComponent();

            var lstPetType = _petTypeRepository.GetAll();
            var lstPetBreed = _petBreedReposiroty.GetAll();
            var lstPetGender = _petGenderRepository.GetAll();
            var lstPetSize = _petSizeRepository.GetAll();

            pckPetType.ItemsSource = lstPetType;
            pckPetBreed.ItemsSource = lstPetBreed;
            pckPetGender.ItemsSource = lstPetGender;
            pckPetSize.ItemsSource = lstPetSize;
        }

        private void btnFilterAccept_Clicked(object sender, EventArgs e)
        {
            List<string> dataFilters = new List<string>();

            try
            {
                if (!String.IsNullOrEmpty(pckPetType.SelectedItem.ToString()))
                {
                    string petType = pckPetType.SelectedItem.ToString();
                    Preferences.Set("petTypeFilter", petType);
                }
            }
            catch { }
            try
            {
                if (!String.IsNullOrEmpty(pckPetBreed.SelectedItem.ToString()))
                {
                    string petBreed = pckPetBreed.SelectedItem.ToString();
                    Preferences.Set("petBreedFilter", petBreed);
                }
            }
            catch { }
            try
            {
                if (!String.IsNullOrEmpty(pckPetGender.SelectedItem.ToString()))
                {
                    string petGender = pckPetGender.SelectedItem.ToString();
                    Preferences.Set("petGenderFilter", petGender);
                }
            }
            catch { }
            try 
            {
                if (!String.IsNullOrEmpty(pckPetSize.SelectedItem.ToString()))
                {
                    string petSize = pckPetSize.SelectedItem.ToString();
                    Preferences.Set("petSizeFilter", petSize);
                }
            }
            catch { }
            try
            {
                if (!String.IsNullOrEmpty(pckDistrict.SelectedItem.ToString()))
                {
                    string district = pckDistrict.SelectedItem.ToString();
                    Preferences.Set("district", district);
                }
            }
            catch { }
            try
            {
                if (!String.IsNullOrEmpty(pckProvince.SelectedItem.ToString()))
                {
                    string province = pckProvince.SelectedItem.ToString();
                    Preferences.Set("province", province);
                }
            }
            catch { }
            //Navigation.PopPopupAsync();
            AdvertListPage advertListPage = new AdvertListPage();
            advertListPage.bindingData();
            Rg.Plugins.Popup.Services.PopupNavigation.Instance.PopAsync();
            //DataEventHandler?.Invoke(this, dataFilters);
        }

        private void btnFilterCancel_Clicked(object sender, EventArgs e)
        {
            //Rg.Plugins.Popup.Services.PopupNavigation.Instance.PopAsync();
            Navigation.PopPopupAsync();
        }

        private void pckPetType_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selected = pckPetType.SelectedItem.ToString();
            pckPetBreed.ItemsSource = _petBreedReposiroty.GetSelectedTypeToBreed(selected);
        }
    }
}