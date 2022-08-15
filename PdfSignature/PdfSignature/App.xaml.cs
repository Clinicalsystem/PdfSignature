using PdfSignature.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: ExportFont("Montserrat-Bold.ttf",Alias="Montserrat-Bold")]
     [assembly: ExportFont("Montserrat-Medium.ttf", Alias = "Montserrat-Medium")]
     [assembly: ExportFont("Montserrat-Regular.ttf", Alias = "Montserrat-Regular")]
     [assembly: ExportFont("Montserrat-SemiBold.ttf", Alias = "Montserrat-SemiBold")]
     [assembly: ExportFont("UIFontIcons.ttf", Alias = "FontIcons")]
namespace PdfSignature
{
    public partial class App : Application
    {
        public static INavigation GlobalNavigation { get; set; }

        private NavigationPage loginPage { get; set; }
        public App()
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Njg2NzMzQDMyMzAyZTMyMmUzMGtBdElubGRvWGZKRjFZdTN4Q1liSzhiV2h0NFVQbTJkOVJIdkFWRmpla3c9");
            InitializeComponent();
            loginPage = new NavigationPage(new LoginPage());

            GlobalNavigation = loginPage.Navigation;

            MainPage = loginPage;
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
