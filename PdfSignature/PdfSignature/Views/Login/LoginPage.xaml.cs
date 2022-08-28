using PdfSignature.Services;
using PdfSignature.ViewModels;
using Syncfusion.SfPdfViewer.XForms;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace PdfSignature.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [Preserve(AllMembers = true)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when tab view selection items are changed.
        /// </summary>
        private void tabView_SelectionChanged(object sender, Syncfusion.XForms.TabView.SelectionChangedEventArgs e)
        {

            if (Device.RuntimePlatform == Device.Android || Device.RuntimePlatform == Device.iOS)
            {
                if (e.Name == "Sign Up")
                {
                    this.frame.HeightRequest = 460;
                }
                else if (e.Name == "Login")
                {
                    this.frame.HeightRequest = 390;
                }
            }
        }
       

        private void NameEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(BaseViewModel.IsCompletet)
            {
                tabView.SelectedIndex = 0;
            }

        }

        
        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
            if(propertyName == "IsBusy")
            {
                //activityIndicator.IsRunning = IsBusy;
            }
        }

           
        

        private void Login_Clicked(object sender, System.EventArgs e)
        {
            
        }
        //correo@correo.cl

    }
}