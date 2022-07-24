using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using Pettel.Models.UserProperties;
using UIKit;

namespace Pettel.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            Rg.Plugins.Popup.Popup.Init();

            global::Xamarin.Forms.Forms.Init();
            Xamarin.FormsGoogleMaps.Init("<Your google maps api key>");
            LoadApplication(new App());

            return base.FinishedLaunching(app, options);
        }
    }
}
