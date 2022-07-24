using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using Android;
using AndroidX.AppCompat.App;
using Xamarin.Essentials;
using Android.Content;
using Android.Gms.Auth.Api.SignIn;
using Android.Gms.Auth.Api;

namespace Pettel.Droid
{
    [Activity(Label = "Pettel", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        const int RequestLocationId = 0;
        readonly string[] LocationPermissions =
        {
            Manifest.Permission.AccessCoarseLocation,
            Manifest.Permission.AccessFineLocation
        };

        protected override void OnStart()
        {
            base.OnStart();
        }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            AppCompatDelegate.DefaultNightMode = AppCompatDelegate.ModeNightNo;

            Rg.Plugins.Popup.Popup.Init(this);
            Xamarin.FormsGoogleMaps.Init(this, savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if (requestCode == 1)
            {
                GoogleSignInResult result = Auth.GoogleSignInApi.GetSignInResultFromIntent(data);
                GoogleManager.Instance.OnAuthCompleted(result);
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            //var status = Permissions.RequestAsync<Permissions.LocationWhenInUse>();
            //if(requestCode == RequestLocationId)
            //{
            //    if((grantResults.Length == 1) && (grantResults[0] == (int)Permission.Granted))
            //    {
            //        //Granted
            //    }
            //    else
            //    {
            //        //Denied
            //    }
            //}
            //else
            //{
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            //}

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}