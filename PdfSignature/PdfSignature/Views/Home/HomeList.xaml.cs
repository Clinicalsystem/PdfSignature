using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.WindowsSpecific;
using Xamarin.Forms.Xaml;
using Application = Xamarin.Forms.Application;

namespace PdfSignature.Views.Home
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomeList : ContentPage
    {
        public HomeList()
        {
            InitializeComponent();
            //Application.Current.On<Windows>().SetImageDirectory("Assets");
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await Task.Run(() =>
                 {
                     Task.Delay(1000).Wait();
                     listRecients.SelectedItems.Clear();
                     listFavorits.SelectedItems.Clear();
                 });
        }
    }
}