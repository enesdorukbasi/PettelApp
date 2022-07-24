using Pettel.Models.UserProperties;
using Pettel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Pettel.Pages.Interfaces
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BottomTabs : Shell
    {

        public BottomTabs()
        {
            InitializeComponent();
        }

    }
}