using Firebase.Database;
using Firebase.Storage;
using Newtonsoft.Json;
using Pettel.Models;
using Pettel.Models.VeterinaryClinicProperties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace Pettel.Repositories
{
    public class VetetirenerRepository
    {
        FirebaseClient firebaseClient = new FirebaseClient("<Your db url>");
        FirebaseStorage firebaseStorage = new FirebaseStorage("<Your storage url>");
        public async Task<bool> Save(Vetetirener vetetirener)
        {
            var data = await firebaseClient.Child(nameof(Vetetirener)).PostAsync(JsonConvert.SerializeObject(vetetirener));

            if (!string.IsNullOrEmpty(data.Key))
            {
                Preferences.Set("VetetirenerKey", data.Key);
                return true;
            }
            return false;
        }
        public async Task<string> UploadImage(Stream img, string fileName)
        {
            var image = await firebaseStorage.Child("VetetirenerImages").Child(fileName).PutAsync(img);
            return image;
        }

        public async Task<bool> SaveTreatment(ClinicalTreatmentInformation clinicalTreatmentInformation)
        {
            var data = await firebaseClient.Child(nameof(ClinicalTreatmentInformation)).PostAsync(JsonConvert.SerializeObject(clinicalTreatmentInformation));

            if (!string.IsNullOrEmpty(data.Key))
            {
                Preferences.Set("ClinicalTreatmentInformationKey", data.Key);
                return true;
            }
            return false;
        }

        public List<string> GetAllTypesOfClinicalTreatment()
        {
            List<string> TypesOfClinicalTreatment = Enum.GetNames(typeof(TypesOfClinicalTreatment)).ToList();

            return TypesOfClinicalTreatment;
        }


        public async Task<List<ClinicalTreatmentInformation>> GetAllClinicalTreatmentInformation(string email)
        {
            return (await firebaseClient.Child(nameof(ClinicalTreatmentInformation)).OnceAsync<ClinicalTreatmentInformation>()).Select(x => new ClinicalTreatmentInformation
            {
                Id = x.Key,
                ClinicalTreatmentType = x.Object.ClinicalTreatmentType,
                Content = x.Object.Content,
                Price = x.Object.Price,
                EMail = x.Object.EMail,
                VetetirenerId = x.Object.VetetirenerId
            }).Where(x => x.EMail == email).ToList();
        }

        public async Task<List<ClinicalTreatmentInformation>> GetAllClinicalTreatmentInformationByVetetirenerId(string vetetirenerId)
        {
            return (await firebaseClient.Child(nameof(ClinicalTreatmentInformation)).OnceAsync<ClinicalTreatmentInformation>()).Select(x => new ClinicalTreatmentInformation
            {
                Id = x.Key,
                ClinicalTreatmentType = x.Object.ClinicalTreatmentType,
                Content = x.Object.Content,
                Price = x.Object.Price,
                EMail = x.Object.EMail,
                VetetirenerId = x.Object.VetetirenerId
            }).Where(x => x.VetetirenerId == vetetirenerId).ToList();
        }

        public async Task<List<Vetetirener>> GetAllVetetirener(bool isMarked, string province, string district)
        {
            if (isMarked && !String.IsNullOrEmpty(province) && !String.IsNullOrEmpty(district))
            {
                return (await firebaseClient.Child(nameof(Vetetirener)).OnceAsync<Vetetirener>()).Select(x => new Vetetirener
                {
                    Id = x.Key,
                    Image = x.Object.Image,
                    FullAddress = x.Object.FullAddress,
                    Title = x.Object.Title,
                    Explanation = x.Object.Explanation,
                    EMail = x.Object.EMail,
                    Province = x.Object.Province,
                    District = x.Object.District,
                    Latitude = x.Object.Latitude,
                    Longitude = x.Object.Longitude
                }).Where(x => x.Province == province).Where(x => x.District == district).ToList();
            }
            else if(isMarked && !String.IsNullOrEmpty(province) && String.IsNullOrEmpty(district))
            {
                return (await firebaseClient.Child(nameof(Vetetirener)).OnceAsync<Vetetirener>()).Select(x => new Vetetirener
                {
                    Id = x.Key,
                    Image = x.Object.Image,
                    FullAddress = x.Object.FullAddress,
                    Title = x.Object.Title,
                    Explanation = x.Object.Explanation,
                    EMail = x.Object.EMail,
                    Province = x.Object.Province,
                    District = x.Object.District,
                    Latitude = x.Object.Latitude,
                    Longitude = x.Object.Longitude
                }).Where(x => x.Province == province).ToList();
            }
            else
            {
                return (await firebaseClient.Child(nameof(Vetetirener)).OnceAsync<Vetetirener>()).Select(x => new Vetetirener
                {
                    Id = x.Key,
                    Title = x.Object.Title,
                    Image = x.Object.Image,
                    FullAddress = x.Object.FullAddress,
                    Explanation = x.Object.Explanation,
                    EMail = x.Object.EMail,
                    Province = x.Object.Province,
                    District = x.Object.District,
                    Latitude = x.Object.Latitude,
                    Longitude = x.Object.Longitude
                }).ToList();
            }
        }

        public async Task<List<Vetetirener>> GetAllVetetirenerByProvinceAndDistrict(string province, string district)
        {
            return (await firebaseClient.Child(nameof(Vetetirener)).OnceAsync<Vetetirener>()).Select(x => new Vetetirener
            {
                Id = x.Key,
                Title = x.Object.Title,
                Explanation = x.Object.Explanation,
                EMail = x.Object.EMail,
                Province = x.Object.Province,
                District = x.Object.District,
                Latitude = x.Object.Latitude,
                Longitude = x.Object.Longitude
            }).Where(x => x.Province == province).Where(x => x.District == district).ToList();
        }

        public async Task<Vetetirener> GetVetetirenerById(string id)
        {
            return (await firebaseClient.Child(nameof(Vetetirener) + "/" + id).OnceSingleAsync<Vetetirener>());
        }

        public async Task<Vetetirener> GetVetetirenerByEmail(string email)
        {
            var vetetirener = (await firebaseClient.Child(nameof(Vetetirener)).OnceAsync<Vetetirener>()).Select(x => new Vetetirener
            {
                Id = x.Key,
                Title = x.Object.Title,
                Explanation = x.Object.Explanation,
                District = x.Object.District,
                EMail = x.Object.EMail,
                FullAddress = x.Object.FullAddress,
                Image = x.Object.Image,
                Latitude = x.Object.Latitude,
                Longitude = x.Object.Longitude,
                Province = x.Object.Province
            }).Where(x => x.EMail.ToLower() == email.ToLower()).ToList();


            return vetetirener[0];
        }
    }
}
