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
    public class ProductRepository
    {
        FirebaseClient firebaseClient = new FirebaseClient("<Your db url>");
        FirebaseStorage firebaseStorage = new FirebaseStorage("<Your storage url>");

        public async Task<bool> Save(Product product)
        {
            var data = await firebaseClient.Child(nameof(Product)).PostAsync(JsonConvert.SerializeObject(product));

            if (!string.IsNullOrEmpty(data.Key))
            {
                Preferences.Set("ProductKey", data.Key);
                return true;
            }
            return false;
        }

        public async Task<string> UploadImage(Stream img, string fileName)
        {
            var image = await firebaseStorage.Child("ProductImages").Child(fileName).PutAsync(img);
            return image;
        }


        public async Task<List<Product>> GetAll()
        {
            return (await firebaseClient.Child(nameof(Product)).OnceAsync<Product>()).Select(x => new Product
            {
                Id = x.Key,
                Image1 = x.Object.Image1,
                Image2 = x.Object.Image2,
                Image3 = x.Object.Image3,
                ProductName = x.Object.ProductName,
                Explanation = x.Object.Explanation,
                Price = x.Object.Price,
                Number = x.Object.Number,
                PetType = x.Object.PetType,
                MainCategoryName = x.Object.MainCategoryName,
                Tags = x.Object.Tags,
                EMail = x.Object.EMail
            }).Where(x => x.Number > 0).ToList();
        }

        public async Task<List<Product>> GetAllByProductNameOrTags(string search)
        {
            return (await firebaseClient.Child(nameof(Product)).OnceAsync<Product>()).Select(x => new Product
            {
                Id = x.Key,
                Image1 = x.Object.Image1,
                Image2 = x.Object.Image2,
                Image3 = x.Object.Image3,
                ProductName = x.Object.ProductName,
                Explanation = x.Object.Explanation,
                Price = x.Object.Price,
                Number = x.Object.Number,
                PetType = x.Object.PetType,
                MainCategoryName = x.Object.MainCategoryName,
                Tags = x.Object.Tags,
                EMail = x.Object.EMail
            }).Where(x => x.Number > 0).Where(x => x.ProductName.ToLower().Contains(search.ToLower()) || x.Tags.ToLower().Contains(search.ToLower())).ToList();
        }

        public async Task<List<Product>> GetAllByEMail(string email)
        {
            return (await firebaseClient.Child(nameof(Product)).OnceAsync<Product>()).Select(x => new Product
            {
                Id = x.Key,
                Image1 = x.Object.Image1,
                Image2 = x.Object.Image2,
                Image3 = x.Object.Image3,
                ProductName = x.Object.ProductName,
                Explanation = x.Object.Explanation,
                Price = x.Object.Price,
                Number = x.Object.Number,
                PetType = x.Object.PetType,
                MainCategoryName = x.Object.MainCategoryName,
                Tags = x.Object.Tags,
                EMail = x.Object.EMail
            }).Where(x => x.EMail.ToLower() == email.ToLower()).ToList();
        }


        public async Task<List<Product>> GetAllByEmailAndProductNameOrTags(string email, string search)
        {
            return (await firebaseClient.Child(nameof(Product)).OnceAsync<Product>()).Select(x => new Product
            {
                Id = x.Key,
                Image1 = x.Object.Image1,
                Image2 = x.Object.Image2,
                Image3 = x.Object.Image3,
                ProductName = x.Object.ProductName,
                Explanation = x.Object.Explanation,
                Price = x.Object.Price,
                Number = x.Object.Number,
                PetType = x.Object.PetType,
                MainCategoryName = x.Object.MainCategoryName,
                Tags = x.Object.Tags,
                EMail = x.Object.EMail
            }).Where(x => x.EMail.ToLower() == email.ToLower()).Where(x => x.ProductName.ToLower().Contains(search.ToLower()) || x.Tags.ToLower().Contains(search.ToLower())).ToList();
        }


        public async Task<Product> GetById(string id)
        {
            return (await firebaseClient.Child(nameof(Product) + "/" + id).OnceSingleAsync<Product>());
        }

        public async Task<bool> Delete(string id)
        {
            await firebaseClient.Child(nameof(Product) + "/" + id).DeleteAsync();
            return true;
        }
        public async Task<bool> Update(Product product)
        {
            await firebaseClient.Child(nameof(Product) + "/" + product.Id).PutAsync(JsonConvert.SerializeObject(product));
            return true;
        }
    }
}
