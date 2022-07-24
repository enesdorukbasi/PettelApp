using Firebase.Database;
using Firebase.Storage;
using Newtonsoft.Json;
using Pettel.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace Pettel.Repositories
{
    public class EmergencyAlertRepository
    {
        FirebaseClient firebaseClient = new FirebaseClient("<Your db url>");
        FirebaseStorage firebaseStorage = new FirebaseStorage("<Your storage url>");

        public async Task<bool> Save(Emergency emergency)
        {
            var data = await firebaseClient.Child(nameof(Emergency)).PostAsync(JsonConvert.SerializeObject(emergency));

            if (!string.IsNullOrEmpty(data.Key))
            {
                Preferences.Set("EmergencyKey", data.Key);
                return true;
            }
            return false;
        }

        public async Task<string> UploadImage(Stream img, string fileName)
        {
            var image = await firebaseStorage.Child("EmergencyAlertImages").Child(fileName).PutAsync(img);
            return image;
        }


        public async Task<List<Emergency>> GetAllByAddress(string province, string district)
        {
            return (await firebaseClient.Child(nameof(Emergency)).OnceAsync<Emergency>()).Select(x => new Emergency
            {
                Id = x.Key,
                Title = x.Object.Title,
                Content = x.Object.Content,
                Province = x.Object.Province,
                District = x.Object.District,
                FullAddress = x.Object.FullAddress,
                Latitude = x.Object.Latitude,
                Longitude = x.Object.Longitude,
                PetType = x.Object.PetType,
                Image1 = x.Object.Image1,
                Image2 = x.Object.Image2,
                Image3 = x.Object.Image3,
                EMail = x.Object.EMail,
                IsActive = x.Object.IsActive
            }).Where(x => x.IsActive == true).Where(x=>x.Province == province).Where(x=>x.District == district).ToList();
        }
    }
}
