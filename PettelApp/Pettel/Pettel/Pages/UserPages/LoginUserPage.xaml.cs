using Firebase.Auth;
using Newtonsoft.Json;
using Pettel.Models.UserProperties;
using Pettel.Pages.Interfaces;
using Pettel.Repositories;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Pettel.Pages.UserPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginUserPage : ContentPage
    {
        UserRepository _userRepository = new UserRepository();
        private readonly IGoogleManager _googleManager;
        GoogleUser GoogleUser = new GoogleUser();
        public bool IsLogedIn { get; set; }

        public LoginUserPage()
        {
            _googleManager = DependencyService.Get<IGoogleManager>();
            IsLogedIn = Preferences.Get("isLoggedInGmail", false);
            if (IsLogedIn == true)
            {
                CheckUserLoggedIn();
            }
            else if(IsLogedIn == false)
            {
                try
                {
                    AutoSignIn();
                }
                catch { }
            }
            InitializeComponent();
            
        }
        private void CheckUserLoggedIn()
        {
            _googleManager.Login(OnLoginComplete);
        }

        private async void AutoSignIn()
        {
            try
            {
                string email = Preferences.Get("currentUserEmail", string.Empty);
                string password = Preferences.Get("currentUserPassword", string.Empty);

                string SignInToken = await _userRepository.SignIn(email, password);

                if (SignInToken != null)
                {
                    Preferences.Set("currentUserEmail", email);
                    Preferences.Set("currentUserPassword", password);
                    GoToMainPage();
                }
                else
                {
                    return;
                }
            }
            catch { }
        }

        private async void btnLogin_Clicked(object sender, EventArgs e)
        {
            string email = txtEmail.Text;
            string password = txtPassword.Text;

            if (String.IsNullOrEmpty(email))
            {
                await DisplayAlert("Uyarı!", "E-Mail adresi boş geçilemez.", "Tamam");
                return;
            }
            else if (String.IsNullOrEmpty(password))
            {
                await DisplayAlert("Uyarı!", "Parola boş geçilemez.", "Tamam");
                return;
            }

            string SignInToken = await _userRepository.SignIn(email, password);
            if (SignInToken != null)
            {
                Preferences.Set("currentUserEmail", email);
                Preferences.Set("currentUserPassword", password);
                GoToMainPage();
            }
            else
            {
                return;
            }

        }

        private void GoToMainPage()
        {
            Application.Current.MainPage = new BottomTabs();
        }

        private async void btnRegisterPage_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RegisterUserPage(null));
        }

        private void btnGmailSignIn_Clicked(object sender, EventArgs e)
        {
            //_googleManager.Logout();
            _googleManager.Login(OnLoginComplete);
        }

        private void btnFacebookSignIn_Clicked(object sender, EventArgs e)
        {

        }

        private async void OnLoginComplete(GoogleUser googleUser, string message)
        {
            if (googleUser != null)
            {
                GoogleUser = googleUser;
                Pettel.Models.User user;
                try
                {
                    user = await _userRepository.GetUserTypeByEmail(googleUser.Email);
                }
                catch (System.ArgumentOutOfRangeException)
                {
                    user = null;
                }

                if (user == null)
                {
                    if (IsLogedIn == true)
                    {
                        GoogleLogout();
                        IsLogedIn = false;
                        GoogleUser = null;
                        return;
                    }
                    await Navigation.PushAsync(new RegisterUserPage(googleUser));
                }
                else
                {
                    Preferences.Set("currentUserEmail", user.EMail);
                    IsLogedIn = true;
                    Preferences.Set("isLoggedInGmail", IsLogedIn);
                    GoToMainPage();
                }
            }
            else
            {
                await DisplayAlert("Message", message, "Ok");
            }
        }

        private void GoogleLogout()
        {
            _googleManager.Logout();
            IsLogedIn = false;
            Preferences.Set("isLoggedInGmail", IsLogedIn);
        }

        //private void btnLogout_Clicked(object sender, EventArgs e)
        //{
        //    _googleManager.Logout();

        //}
    } 
}