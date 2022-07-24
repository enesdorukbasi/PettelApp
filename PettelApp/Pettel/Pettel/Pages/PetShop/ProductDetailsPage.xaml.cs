using Pettel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Pettel.Pages.PetShop
{
    public class Images
    {
        public string Title { get; set; }
        public string Url { get; set; }
    }

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProductDetailsPage : ContentPage
    {
        Product currentProduct = new Product();

        public ProductDetailsPage(Product product)
        {
            InitializeComponent();

            currentProduct = product;

            List<Images> Images = new List<Images>();
            if(product.Image1 != null)
            {
                Images.Add(new Images() { Title = "Image1", Url = product.Image1 });
            }
            if(product.Image2 != null)
            {
                Images.Add(new Images() { Title = "Image2", Url = product.Image2 });
            }
            if (product.Image3 != null)
            {
                Images.Add(new Images() { Title = "Image3", Url = product.Image3 });
            }
            ProductImages.ItemsSource = Images;

            lblProductName.Text = product.ProductName;
            lblExplanation.Text = product.Explanation;
            lblPetType.Text = product.PetType;
            lblMainCategoryName.Text = product.MainCategoryName;
            lblNumber.Text = product.Number.ToString();
            lblPrice.Text = product.Price.ToString();
        }
    }
}