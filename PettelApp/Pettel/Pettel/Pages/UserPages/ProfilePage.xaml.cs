using Pettel.Models;
using Pettel.Models.UserProperties;
using Pettel.Pages.AdvertPages;
using Pettel.Pages.BasketAndSalesPages;
using Pettel.Pages.PetShop;
using Pettel.Pages.QAPages;
using Pettel.Pages.VetetirenerPages;
using Pettel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Pettel.Pages.UserPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfilePage : ContentPage
    {
        UserRepository _userRepository = new UserRepository();
        VetetirenerRepository _vetetirenerRepository = new VetetirenerRepository();
        private readonly IGoogleManager _googleManager;

        public ProfilePage()
        {
            ProfilePageByAccountType();
            InitializeComponent();
        }

        public async void ProfilePageByAccountType()
        {
            string email = Preferences.Get("currentUserEmail", string.Empty);
            Models.User currentUser = await _userRepository.GetUserTypeByEmail(email);
            
            if (currentUser.AccountType == AccountType.Kullanıcı.ToString())
            {
                btnMyProducts.IsVisible = false;
                btnMyVetAdvert.IsVisible = false;
            }
            else if (currentUser.AccountType == AccountType.PetShop.ToString())
            {
                btnMyProducts.IsVisible = true;
                btnMyVetAdvert.IsVisible = false;
            }
            else if (currentUser.AccountType == AccountType.Vetetirener.ToString())
            {
                btnMyProducts.IsVisible = false;
                btnMyVetAdvert.IsVisible = true;
            }
            else if (currentUser.AccountType == AccountType.PetWalker.ToString())
            {
            }
            lblEMail.Text = currentUser.EMail;
            lblFullName.Text = currentUser.Name + " " + currentUser.Surname;
        }

        private async void btnLogout_Clicked(object sender, EventArgs e)
        {
            bool islogged = Preferences.Get("isLoggedInGmail", false);

            if (islogged == false)
            {
                Preferences.Set("currentUserEmail", string.Empty);
                Preferences.Set("currentUserPassword", string.Empty);

                Application.Current.MainPage = new NavigationPage(new LoginUserPage());
                var navigationPage = Application.Current.MainPage as NavigationPage;
                navigationPage.BarBackgroundColor = Color.DarkOrange;
            }
            else
            {
                Preferences.Set("isLoggedInGmail", false);
                Preferences.Set("currentUserEmail", string.Empty);
                Preferences.Set("currentUserPassword", string.Empty);
                try
                {
                    _googleManager.Logout();
                }
                catch (System.NullReferenceException){ }
                Application.Current.MainPage = new NavigationPage(new LoginUserPage());
                var navigationPage = Application.Current.MainPage as NavigationPage;
                navigationPage.BarBackgroundColor = Color.DarkOrange;
            }
        }

        private async void btnMyAdverts_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MyAdvertsListPage());
        }

        private async void btnMyQuestions_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MyQuestionListPage());
        }

        private async void btnMyProducts_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MyProductListPage());
        }

        private async void btnMyAddresses_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MyAddressListPage());
        }

        private async void btnMyVetAdvert_Clicked(object sender, EventArgs e)
        {
            string email = Preferences.Get("currentUserEmail", string.Empty);
            try
            {
                Vetetirener vetetirener = await _vetetirenerRepository.GetVetetirenerByEmail(email);
                await Navigation.PushAsync(new MyVetetirenerAdvertPage(vetetirener));
            }
            catch (System.ArgumentOutOfRangeException)
            {
                var isOk = await DisplayAlert("", "Henüz bir veteriner ilanı oluşturmamışsınız. Oluşturmak ister misiniz?", "Evet", "Hayır");
                if (isOk)
                {
                    await Navigation.PushAsync(new CreateVetetirenerAdvertPage(false, null));
                }
            }

        }
    }
}