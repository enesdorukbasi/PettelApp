using Pettel.Models;
using Pettel.Models.UserProperties;
using Pettel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Pettel.Pages.UserPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegisterUserPage : ContentPage
    {
        UserRepository _userRepository = new UserRepository();
        GoogleUser currentGoogleUser;

        public RegisterUserPage(GoogleUser googleUser)
        {
            InitializeComponent();
            if(googleUser != null)
            {
                currentGoogleUser = googleUser;

                string FUllName = googleUser.Name.ToString();

                string[] fullNameSplit = FUllName.Split(' ');
                string name = "";
                string surname = fullNameSplit[fullNameSplit.Length-1].ToString();
                for(int i = 0; i < fullNameSplit.Length-1; i++)
                {
                    name += fullNameSplit[i].ToString() + " ";
                }
                txtEmail.Text = googleUser.Email;
                txtName.Text = name;
                txtSurname.Text = surname;

                lblTitle.Text = "Profilini Tamamla";

                txtEmail.IsEnabled = false;
                txtSurname.IsEnabled = false;
                txtName.IsEnabled = false;
                txtPassword.IsVisible = false;
                txtConfirmPassword.IsVisible = false;
            }

            pckAccountType.ItemsSource = _userRepository.AccountTypes();
        }

        private async void btnRegister_Clicked(object sender, EventArgs e)
        {
            if(currentGoogleUser == null)
            {
                string name = txtName.Text;
                string surname = txtSurname.Text;
                string email = txtEmail.Text;
                string password = txtPassword.Text;
                string confirmPassword = txtConfirmPassword.Text;
                string phoneNumber = txtPhoneNumber.Text;
                string accountType = pckAccountType.SelectedItem.ToString();

                #region Validation
                if (String.IsNullOrEmpty(name))
                {
                    await DisplayAlert("Uyarı!", "Ad alanı boş geçilemez.", "Tamam");
                    return;
                }
                else if (String.IsNullOrEmpty(surname))
                {
                    await DisplayAlert("Uyarı!", "Soyad alanı boş geçilemez.", "Tamam");
                    return;
                }
                else if (String.IsNullOrEmpty(email))
                {
                    await DisplayAlert("Uyarı!", "E-Mail alanı boş geçilemez.", "Tamam");
                    return;
                }
                else if (String.IsNullOrEmpty(password))
                {
                    await DisplayAlert("Uyarı!", "Parola alanı boş geçilemez.", "Tamam");
                    return;
                }
                else if (String.IsNullOrEmpty(confirmPassword))
                {
                    await DisplayAlert("Uyarı!", "Parolayı onaylamanız gerekiyor.", "Tamam");
                    return;
                }
                else if (String.IsNullOrEmpty(accountType))
                {
                    await DisplayAlert("Uyarı!", "Kullanıcı tipi alanı boş geçilemez.", "Tamam");
                }
                else if (String.IsNullOrEmpty(phoneNumber))
                {
                    await DisplayAlert("Uyarı!", "Telefon numarası alanı boş geçilemez.", "Tamam");
                }
                else if (password != confirmPassword)
                {
                    await DisplayAlert("Uyarı!", "Parola onaylanamadı.", "Tamam");
                    return;
                }
                #endregion

                User user = new User();
                user.Name = name;
                user.Surname = surname;
                user.AccountType = accountType;
                user.EMail = email;
                user.Password = password;
                user.PhoneNumber = phoneNumber;
                user.IsActive = true;

                bool IsSave = await _userRepository.Register(user);
                if (IsSave)
                {
                    await DisplayAlert("Başarılı.", "Kullanıcı kaydı başarılı.", "Tamam");
                    await Navigation.PopAsync();
                }
                else
                {
                    await DisplayAlert("Başarısız.", "Kullanıcı kaydı başarısız.", "Tamam");
                }
            }
            else
            {
                string name = txtName.Text;
                string surname = txtSurname.Text;
                string email = txtEmail.Text;
                string phoneNumber = txtPhoneNumber.Text;
                string password = "";
                string accountType = pckAccountType.SelectedItem.ToString();

                #region Validation
                if (String.IsNullOrEmpty(name))
                {
                    await DisplayAlert("Uyarı!", "Ad alanı boş geçilemez.", "Tamam");
                    return;
                }
                else if (String.IsNullOrEmpty(surname))
                {
                    await DisplayAlert("Uyarı!", "Soyad alanı boş geçilemez.", "Tamam");
                    return;
                }
                else if (String.IsNullOrEmpty(email))
                {
                    await DisplayAlert("Uyarı!", "E-Mail alanı boş geçilemez.", "Tamam");
                    return;
                }
                else if (String.IsNullOrEmpty(accountType))
                {
                    await DisplayAlert("Uyarı!", "Kullanıcı tipi alanı boş geçilemez.", "Tamam");
                }
                else if (String.IsNullOrEmpty(phoneNumber))
                {
                    await DisplayAlert("Uyarı!", "Telefon numarası alanı boş geçilemez.", "Tamam");
                }
                #endregion

                User user = new User();
                user.Name = name;
                user.Surname = surname;
                user.AccountType = accountType;
                user.EMail = email;
                user.PhoneNumber = phoneNumber;
                user.IsActive = true;
                user.Password = "";

                bool IsSave = await _userRepository.RegisterWithGmail(user);
                if (IsSave)
                {
                    await DisplayAlert("Başarılı.", "Kullanıcı kaydı başarılı.", "Tamam");
                    await Navigation.PopAsync();
                }
                else
                {
                    await DisplayAlert("Başarısız.", "Kullanıcı kaydı başarısız.", "Tamam");
                }
            }
        }

        private void pckAccountType_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}