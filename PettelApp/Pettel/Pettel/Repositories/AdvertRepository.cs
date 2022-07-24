using Firebase.Database;
using Firebase.Storage;
using Newtonsoft.Json;
using Pettel.Models;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace Pettel.Repositories
{
    public class AdvertRepository
    {
        FirebaseClient firebaseClient = new FirebaseClient("<Your db url>");
        FirebaseStorage firebaseStorage = new FirebaseStorage("<Your storage url>");
        public async Task<bool> Save(Advert advert)
        {
            var data = await firebaseClient.Child(nameof(Advert)).PostAsync(JsonConvert.SerializeObject(advert));

            if (!string.IsNullOrEmpty(data.Key))
            {
                Preferences.Set("AdvertKey", data.Key);
                return true;
            }
            return false;
        }

        public async Task<List<Advert>> GetAll(bool typeIsFilter)
        {
            if(typeIsFilter == true)
            {
                string petType = "";
                string petBreed = "";
                string petGender = "";
                string petSize = "";
                string district = "";
                string province = "";

                if (!string.IsNullOrEmpty(Preferences.Get("petTypeFilter", "")))
                {
                    petType = Preferences.Get("petTypeFilter", "");
                }
                if (!string.IsNullOrEmpty(Preferences.Get("petBreedFilter", "")))
                {
                    petBreed = Preferences.Get("petBreedFilter", "");
                }
                if (!string.IsNullOrEmpty(Preferences.Get("petGenderFilter", "")))
                {
                    petGender = Preferences.Get("petGenderFilter", "");
                }
                if (!string.IsNullOrEmpty(Preferences.Get("petSizeFilter", "")))
                {
                    petSize = Preferences.Get("petSizeFilter", "");
                }
                if (!string.IsNullOrEmpty(Preferences.Get("district", "")))
                {
                    district = Preferences.Get("district", "");
                }
                if (!string.IsNullOrEmpty(Preferences.Get("province", "")))
                {
                    province = Preferences.Get("province", "");
                }

                return (await firebaseClient.Child(nameof(Advert)).OnceAsync<Advert>()).Select(x => new Advert
                {
                    Id = x.Key,
                    Title = x.Object.Title,
                    Explanation = x.Object.Explanation,
                    PetName = x.Object.PetName,
                    PetAge = x.Object.PetAge,
                    Image = x.Object.Image,
                    PetType = x.Object.PetType,
                    PetBreed = x.Object.PetBreed,
                    PetSize = x.Object.PetSize,
                    PetGender = x.Object.PetGender,
                    District = x.Object.District,
                    Province = x.Object.Province,
                    UserEmail = x.Object.UserEmail,
                    Datetime = x.Object.Datetime,
                    IsActive = x.Object.IsActive
                }).Where(x => x.PetType.ToLower().Contains(petType.ToLower())).Where(x => x.PetBreed.ToLower().Contains(petBreed.ToLower())).Where(x => x.PetGender.ToLower().Contains(petGender.ToLower()))
                .Where(x => x.PetSize.ToLower().Contains(petSize.ToLower())).ToList();
            }
            else
            {
                return (await firebaseClient.Child(nameof(Advert)).OnceAsync<Advert>()).Select(x => new Advert
                {
                    Id = x.Key,
                    Title = x.Object.Title,
                    Explanation = x.Object.Explanation,
                    PetName = x.Object.PetName,
                    PetAge = x.Object.PetAge,
                    Image = x.Object.Image,
                    PetType = x.Object.PetType,
                    PetBreed = x.Object.PetBreed,
                    PetSize = x.Object.PetSize,
                    PetGender = x.Object.PetGender,
                    UserEmail = x.Object.UserEmail,
                    Province = x.Object.Province,
                    District = x.Object.District,
                    Datetime = x.Object.Datetime,
                    IsActive = x.Object.IsActive
                }).ToList();
            }
        }

        public async Task<List<Advert>> GetAllByTitle(string title)
        {
            return (await firebaseClient.Child(nameof(Advert)).OnceAsync<Advert>()).Select(x => new Advert
            {
                Id = x.Key,
                Title = x.Object.Title,
                Explanation = x.Object.Explanation,
                PetName = x.Object.PetName,
                PetAge = x.Object.PetAge,
                Image = x.Object.Image,
                PetType = x.Object.PetType,
                PetBreed = x.Object.PetBreed,
                PetSize = x.Object.PetSize,
                PetGender = x.Object.PetGender,
                UserEmail = x.Object.UserEmail,
                Datetime = x.Object.Datetime,
                IsActive = x.Object.IsActive
            }).Where(x => x.Title.ToLower().Contains(title.ToLower())).ToList();
        }

        public async Task<List<Advert>> GetAllByEMail(string email)
        {
            return (await firebaseClient.Child(nameof(Advert)).OnceAsync<Advert>()).Select(x => new Advert
            {
                Id = x.Key,
                Title = x.Object.Title,
                Explanation = x.Object.Explanation,
                PetName = x.Object.PetName,
                PetAge = x.Object.PetAge,
                Image = x.Object.Image,
                PetType = x.Object.PetType,
                PetBreed = x.Object.PetBreed,
                PetSize = x.Object.PetSize,
                PetGender = x.Object.PetGender,
                UserEmail = x.Object.UserEmail,
                Datetime = x.Object.Datetime,
                IsActive = x.Object.IsActive
            }).Where(x => x.UserEmail.ToLower().Contains(email.ToLower())).ToList();
        }

        public async Task<List<Advert>> GetAllByEMailAndTitle(string email,string title)
        {
            return (await firebaseClient.Child(nameof(Advert)).OnceAsync<Advert>()).Select(x => new Advert
            {
                Id = x.Key,
                Title = x.Object.Title,
                Explanation = x.Object.Explanation,
                PetName = x.Object.PetName,
                PetAge = x.Object.PetAge,
                Image = x.Object.Image,
                PetType = x.Object.PetType,
                PetBreed = x.Object.PetBreed,
                PetSize = x.Object.PetSize,
                PetGender = x.Object.PetGender,
                UserEmail = x.Object.UserEmail,
                Datetime = x.Object.Datetime,
                IsActive = x.Object.IsActive
            }).Where(x => x.UserEmail.ToLower().Contains(email.ToLower())).Where(x=>x.Title.ToLower().Contains(title.ToLower())).ToList();
        }

        public async Task<Advert> GetById(string id)
        {
            return (await firebaseClient.Child(nameof(Advert) + "/" + id).OnceSingleAsync<Advert>());
        }

        public async Task<bool> Update(Advert advert)
        {
            await firebaseClient.Child(nameof(Advert) + "/" + advert.Id).PutAsync(JsonConvert.SerializeObject(advert));
            return true;
        }

        public async Task<bool> Delete(string id)
        {
            await firebaseClient.Child(nameof(Advert) + "/" + id).DeleteAsync();
            return true;
        }

        public async Task<string> UploadImage(Stream img, string fileName)
        {
            var image = await firebaseStorage.Child("AdvertImages").Child(fileName).PutAsync(img);
            return image;
        }
    }
}
