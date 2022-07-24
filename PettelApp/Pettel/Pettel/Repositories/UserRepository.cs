using Firebase.Auth;
using Firebase.Database;
using Newtonsoft.Json;
using Pettel.Models;
using Pettel.Models.UserProperties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace Pettel.Repositories
{
    public class UserRepository
    {
        string webAPIKey = "Your auth api key";
        FirebaseClient firebaseClient = new FirebaseClient("<Your db url>");
        FirebaseAuthProvider firebaseAuthProvider;

        public UserRepository()
        {
            firebaseAuthProvider = new FirebaseAuthProvider(new FirebaseConfig(webAPIKey));
        }

        public async Task<bool> Register(Models.User user)
        {
            string fullName = user.Name + " " + user.Surname;
            var token = await firebaseAuthProvider.CreateUserWithEmailAndPasswordAsync(user.EMail,user.Password, fullName);
            var data = await firebaseClient.Child(nameof(Models.User)).PostAsync(JsonConvert.SerializeObject(user));
            if (!string.IsNullOrEmpty(data.Key) && !string.IsNullOrEmpty(token.FirebaseToken))
            {
                Preferences.Set("UserKey", data.Key);
                return true;
            }
            return false;
        }

        public async Task<bool> RegisterWithGmail(Models.User user)
        {
            var data = await firebaseClient.Child(nameof(Models.User)).PostAsync(JsonConvert.SerializeObject(user));
            if (!string.IsNullOrEmpty(data.Key))
            {
                Preferences.Set("UserKey", data.Key);
                return true;
            }
            return false;
        }

        public async Task<string> SignIn(string email,string password)
        {
            var token = await firebaseAuthProvider.SignInWithEmailAndPasswordAsync(email, password);
            
            if (!string.IsNullOrEmpty(token.FirebaseToken))
            {
                return token.FirebaseToken;
            }
            return "";
        }

        public async Task<string> GetByEMail(string email)
        {
            var userList = (await firebaseClient.Child(nameof(Models.User)).OnceAsync<Models.User>()).Select(x => new Models.User
            {
                Id = x.Key,
                Name = x.Object.Name,
                Surname = x.Object.Surname,
                EMail = x.Object.EMail,
                PhoneNumber = x.Object.PhoneNumber,
                AccountType = x.Object.AccountType,
                IsActive = x.Object.IsActive
            }).Where(x => x.EMail.ToLower() == email.ToLower()).ToList();

            Preferences.Set("userPhoneNumber", userList[0].PhoneNumber.ToString());
            return userList[0].PhoneNumber.ToString();
        }

        public async Task<Models.User> GetUserTypeByEmail(string email)
        {
            var userList = (await firebaseClient.Child(nameof(Models.User)).OnceAsync<Models.User>()).Select(x => new Models.User
            {
                Id = x.Key,
                Name = x.Object.Name,
                Surname = x.Object.Surname,
                EMail = x.Object.EMail,
                PhoneNumber = x.Object.PhoneNumber,
                AccountType = x.Object.AccountType,
                IsActive = x.Object.IsActive
            }).Where(x => x.EMail.ToLower() == email.ToLower()).ToList();

            
            return userList[0];
        }

        public List<string> AccountTypes()
        {
            List<string> accountTypes = Enum.GetNames(typeof(AccountType)).ToList();

            return accountTypes;
        }
    }
}
