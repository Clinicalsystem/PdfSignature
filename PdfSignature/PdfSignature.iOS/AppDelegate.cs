using Syncfusion.SfRating.XForms.iOS;
using Syncfusion.ListView.XForms.iOS;
using Syncfusion.XForms.iOS.ComboBox;
using Syncfusion.XForms.iOS.TextInputLayout;
using Syncfusion.XForms.iOS.Core;
using Syncfusion.XForms.iOS.Border;
using Syncfusion.XForms.iOS.Graphics;
using Syncfusion.XForms.iOS.TabView;
using Syncfusion.XForms.iOS.Buttons;
using Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using UIKit;

using Syncfusion.SfPdfViewer.XForms.iOS; 

using Syncfusion.SfRangeSlider.XForms.iOS;
using PdfSignature.Services;
using PdfSignature.iOS.Implementation;
using PdfSignature.Implementation;
using Syncfusion.XForms.iOS.Cards;

namespace PdfSignature.iOS
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
            global::Xamarin.Forms.Forms.Init();
            Core.Init();
            SfRatingRenderer.Init();
            SfListViewRenderer.Init();
            SfComboBoxRenderer.Init();
            SfTextInputLayoutRenderer.Init();
            SfAvatarViewRenderer.Init();
            SfSegmentedControlRenderer.Init();
            SfRadioButtonRenderer.Init();
            SfGradientViewRenderer.Init();
            SfBorderRenderer.Init();
            SfTabViewRenderer.Init();
            SfButtonRenderer.Init();
			SfPdfDocumentViewRenderer.Init(); 
			SfRangeSliderRenderer.Init();
            SfCardViewRenderer.Init();
            SfListViewRenderer.Init();
            Xamarin.Forms.DependencyService.Register<IToast, ToastMessage>();
           // Xamarin.Forms.DependencyService.Register<IFileManager, FileManager>();
            LoadApplication(new App());

            return base.FinishedLaunching(app, options);
        }
    }
}
