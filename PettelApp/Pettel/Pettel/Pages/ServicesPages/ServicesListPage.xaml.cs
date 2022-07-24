using Pettel.Pages.PetShop;
using Pettel.Pages.VetetirenerPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Pettel.Pages.ServicesPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ServicesListPage : ContentPage
    {
        public ServicesListPage()
        {
            InitializeComponent();
        }

        private async void Store_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ProductListPage());
        }

        private async void Vet_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new VetetirenerListPage());
        }

    }
}