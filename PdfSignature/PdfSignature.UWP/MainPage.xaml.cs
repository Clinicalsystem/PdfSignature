using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using System.Reflection;
using Syncfusion.ListView.XForms.UWP;
using Syncfusion.SfPdfViewer.XForms.UWP;
using Syncfusion.SfRangeSlider.XForms.UWP;
using Syncfusion.SfRating.XForms.UWP;
using Syncfusion.XForms.UWP.Border;
using Syncfusion.XForms.UWP.Buttons;
using Syncfusion.XForms.UWP.ComboBox;
using Syncfusion.XForms.UWP.Graphics;
using Syncfusion.XForms.UWP.TabView;
using Syncfusion.XForms.UWP.TextInputLayout;

namespace PdfSignature.UWP
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            this.InitializeComponent();
            SfRatingRenderer.Init();
            SfListViewRenderer.Init();
            SfComboBoxRenderer.Init();
            SfTextInputLayoutRenderer.Init();
            SfSegmentedControlRenderer.Init();
            SfRadioButtonRenderer.Init();
            SfGradientViewRenderer.Init();
            SfTabViewRenderer.Init();
            SfButtonRenderer.Init();
            SfPdfDocumentViewRenderer.Init();
            SfRangeSliderRenderer.Init();
            SfListViewRenderer.Init();

            LoadApplication(new PdfSignature.App());
        }
    }
}
