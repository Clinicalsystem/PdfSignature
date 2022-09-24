using PdfSignature.Data;
using PdfSignature.Modelos;
using PdfSignature.Services;
using PdfSignature.Views;
using PdfSignature.Views.Home;
using Xamarin.Essentials;
using Xamarin.Forms;

[assembly: ExportFont("Montserrat-Bold.ttf", Alias = "Montserrat-Bold")]
[assembly: ExportFont("Montserrat-Medium.ttf", Alias = "Montserrat-Medium")]
[assembly: ExportFont("Montserrat-Regular.ttf", Alias = "Montserrat-Regular")]
[assembly: ExportFont("Montserrat-SemiBold.ttf", Alias = "Montserrat-SemiBold")]
[assembly: ExportFont("UIFontIcons.ttf", Alias = "FontIcons")]
[assembly: ExportFont("Icon.ttf", Alias = "Icon")]
namespace PdfSignature
{
    public partial class App : Application
    {
        #region Fields
        public static string ImageServerPath { get; } = "https://cdn.syncfusion.com/essential-ui-kit-for-xamarin.forms/common/uikitimages/";
        public static INavigation GlobalNavigation { get; set; }

        private NavigationPage loginPage { get; set; }
        private NavigationPage PdfSignaturePage { get; set; }

        private response _response;
        #endregion
        public App()
        {
            #region Services
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Njg2NzMzQDMyMzAyZTMyMmUzMGtBdElubGRvWGZKRjFZdTN4Q1liSzhiV2h0NFVQbTJkOVJIdkFWRmpla3c9");
            DependencyService.Register<IMessageService, MessageService>();
            DependencyService.Register<IDataAccess, DataAccess>();
            #endregion
            InitializeComponent();

            if (Preferences.Get("IsRemember", false))
            {

                if (AppSettings.AuthenticationUser.Registered)
                {
                    PdfSignaturePage = new NavigationPage(new HomeList());
                    GlobalNavigation = PdfSignaturePage.Navigation;
                    MainPage = PdfSignaturePage;
                    return;
                }


            }

            loginPage = new NavigationPage(new LoginPage());
            GlobalNavigation = loginPage.Navigation;
            MainPage = loginPage;

        }

        protected override async void OnStart()
        {
            if (AppSettings.IsRemember == false && Preferences.ContainsKey("UserAutentication"))
            {
                Preferences.Clear("UserAutentication");
                return;
            }
            if (AppSettings.AuthenticationUser != null)
            {

                _response = await ApiServicesAutentication.TokenRefresh();

                if (!_response.Success)
                {
                    loginPage = new NavigationPage(new LoginPage());
                    GlobalNavigation = loginPage.Navigation;
                    MainPage = loginPage;
                    return;
                }
            }
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

    }
}
