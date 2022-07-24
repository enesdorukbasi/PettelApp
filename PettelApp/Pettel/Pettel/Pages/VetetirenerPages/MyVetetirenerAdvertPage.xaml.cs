using Pettel.Models;
using Pettel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Pettel.Pages.VetetirenerPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MyVetetirenerAdvertPage : ContentPage
    {
        VetetirenerRepository _vetetirenerRepository = new VetetirenerRepository();
        Vetetirener currentVetetirener = new Vetetirener();

        public MyVetetirenerAdvertPage(Vetetirener vetetirener)
        {
            InitializeComponent();

            lblTitle.Text = vetetirener.Title;
            lblExplanation.Text = vetetirener.Explanation;
            lblProvince.Text = vetetirener.Province;
            lblDistrict.Text = vetetirener.District;
            lblFullAddress.Text = vetetirener.FullAddress;
            imgAdvertImage.Source = vetetirener.Image;

            currentVetetirener = vetetirener;

            lstTreatments.RefreshCommand = new Command(() =>
            {
                OnAppearing();
            });
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            var treatmentList = await _vetetirenerRepository.GetAllClinicalTreatmentInformationByVetetirenerId(currentVetetirener.Id);

            if(treatmentList.Count > 0)
            {
                lblMessage.IsVisible = false;
                lstTreatments.ItemsSource = null;
                lstTreatments.ItemsSource = treatmentList;
                lstTreatments.IsRefreshing = false;
            }
            else
            {
                lblMessage.Text = "Henüz bir tedavi bilgisi eklenmemiş";
                lblMessage.IsVisible = true;
                lstTreatments.ItemsSource = null;
                lstTreatments.IsRefreshing = false;
            }
        }

        private async void btnAddTreatment_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CreateVetetirenerAdvertPage(true, currentVetetirener));
        }

        private async void btnEditAdvert_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CreateVetetirenerAdvertPage(false, null));
        }

        private void btnStkInfoVisible_Clicked(object sender, EventArgs e)
        {
            stkInfo.IsVisible = true;
            stkTreatments.IsVisible = false;
            btnStkInfoVisible.IsEnabled = false;
            btnStkTreatmentsVisible.IsEnabled = true;
        }

        private void btnStkTreatmentsVisible_Clicked(object sender, EventArgs e)
        {
            stkInfo.IsVisible = false;
            stkTreatments.IsVisible = true;
            btnStkInfoVisible.IsEnabled = true;
            btnStkTreatmentsVisible.IsEnabled = false;
        }
    }
}