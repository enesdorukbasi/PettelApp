using Pettel.Models;
using Pettel.Repositories;
using Pettel.Repositories.PetPropertyRepositories;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Pettel.Pages.PetShop
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProductEditPage : ContentPage
    {
        ProductCategoriesRepository _productCategoriesRepository = new ProductCategoriesRepository();
        PetTypeRepository _petTypeRepository = new PetTypeRepository();
        ProductRepository _productRepository = new ProductRepository();
        MediaFile mediaFile1;
        MediaFile mediaFile2;
        MediaFile mediaFile3;

        Product currentProduct = new Product();

        public ProductEditPage(Product product)
        {
            InitializeComponent();
            currentProduct = product;

            pckMainCategory.ItemsSource = _productCategoriesRepository.GetAllMainCategories();
            pckPetType.ItemsSource = _petTypeRepository.GetAll();

            txtProductName.Text = product.ProductName;
            txtExplanation.Text = product.Explanation;
            txtTags.Text = product.Tags;
            txtPrice.Text = product.Price.ToString();
            txtNumber.Text = product.Number.ToString();
            pckPetType.SelectedItem = product.PetType;
            pckMainCategory.SelectedItem = product.MainCategoryName;
            ProductImage1.Source = product.Image1;
            ProductImage2.Source = product.Image2;
            ProductImage3.Source = product.Image3;
        }

        private async void GoToHomePage()
        {
            await Navigation.PopModalAsync();
        }

        private async void ProductImageTap1_Tapped(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();

            try
            {
                mediaFile1 = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions
                {
                    PhotoSize = PhotoSize.Custom
                });
                if (mediaFile1 == null)
                {
                    return;
                }

                ProductImage1.Source = ImageSource.FromStream(() =>
                {
                    return mediaFile1.GetStream();
                });
            }
            catch { }
        }

        private async void ProductImageTap2_Tapped(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();

            try
            {
                mediaFile2 = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions
                {
                    PhotoSize = PhotoSize.Custom
                });
                if (mediaFile2 == null)
                {
                    return;
                }

                ProductImage2.Source = ImageSource.FromStream(() =>
                {
                    return mediaFile2.GetStream();
                });
            }
            catch { }
        }

        private async void ProductImageTap3_Tapped(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();

            try
            {
                mediaFile3 = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions
                {
                    PhotoSize = PhotoSize.Custom
                });
                if (mediaFile3 == null)
                {
                    return;
                }

                ProductImage3.Source = ImageSource.FromStream(() =>
                {
                    return mediaFile3.GetStream();
                });
            }
            catch { }
        }

        private async void btnNext_Clicked(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(txtProductName.Text) && !String.IsNullOrEmpty(txtExplanation.Text) &&
                !String.IsNullOrEmpty(txtPrice.Text.ToString()) && !String.IsNullOrEmpty(txtNumber.Text.ToString()) &&
                !String.IsNullOrEmpty(pckPetType.SelectedItem.ToString()) && !String.IsNullOrEmpty(pckMainCategory.SelectedItem.ToString()))
            {
                StackEntry.IsVisible = false;
                StackImg.IsVisible = true;
            }
            else
            {
                await DisplayAlert("Uyarı!", "Tüm alanlar doldurulmadan ilerleyemezsiniz.", "Tamam");
            }
        }

        private void btnPrev_Clicked(object sender, EventArgs e)
        {
            StackEntry.IsVisible = true;
            StackImg.IsVisible = false;
        }

        private async void btnSave_Clicked(object sender, EventArgs e)
        {
            string productName, explanation, tags, petType, mainCategory, eMail, image1, image2, image3;
            decimal price;
            int number;

            productName = txtProductName.Text;
            explanation = txtExplanation.Text;
            tags = txtTags.Text + ", " + productName;
            petType = pckPetType.SelectedItem.ToString();
            mainCategory = pckMainCategory.SelectedItem.ToString();
            price = decimal.Parse(txtPrice.Text);
            number = Int32.Parse(txtNumber.Text);
            eMail = Preferences.Get("currentUserEmail", string.Empty);

            if (String.IsNullOrEmpty(eMail))
            {
                await DisplayAlert("Hata!", "Bir hatayla karşılaşıldı, uygulamayı kapatıp yeniden açmanız önerilir.", "Tamam");

                Application.Current.Quit();
            }

            Product product = new Product();
            product.Id = currentProduct.Id;
            product.ProductName = productName;
            product.Explanation = explanation;
            product.Tags = tags;
            product.PetType = petType;
            product.MainCategoryName = mainCategory;
            product.Price = price;
            product.Number = number;
            product.EMail = eMail;

            if (mediaFile1 == null && mediaFile2 == null && mediaFile3 == null)
            {
                await DisplayAlert("Uyarı!", "En az 1 adet ürün görseli eklemeniz gerekmektedir.", "Tamam");
                return;
            }

            if (mediaFile1 != null)
            {
                image1 = await _productRepository.UploadImage(mediaFile1.GetStream(), Path.GetFileName(mediaFile1.Path));
                product.Image1 = image1;
            }
            if (mediaFile2 != null)
            {
                image2 = await _productRepository.UploadImage(mediaFile2.GetStream(), Path.GetFileName(mediaFile2.Path));
                product.Image2 = image2;
            }
            if (mediaFile3 != null)
            {
                image3 = await _productRepository.UploadImage(mediaFile3.GetStream(), Path.GetFileName(mediaFile3.Path));
                product.Image3 = image3;
            }

            var isSaved = await _productRepository.Update(product);

            if (isSaved)
            {
                await DisplayAlert("Paylaşma Tamamlandı", "Ürün paylaşıldı.", "Tamam");
                mediaFile1 = null;
                image1 = null;
                mediaFile2 = null;
                image2 = null;
                mediaFile3 = null;
                image3 = null;
                try
                {
                    GoToHomePage();
                }
                catch { }
            }
            else
            {
                await DisplayAlert("Hata!", "Bir hata ile karşılaşıldı.", "Tamam");
            }
        }

    }
}