using Microsoft.AspNetCore.Http.Features;
using PdfSignature.ViewModels;
using Plugin.Fingerprint;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace PdfSignature.Views
{
    /// <summary>
    /// Page to show the setting.
    /// </summary>
    [Preserve(AllMembers = true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PerfilUser : ContentPage
    {
        private PerfilUserViewModel viewModel;
        public PerfilUser()
        {
            this.InitializeComponent();
            
        }

        private void SfSwitch_StateChanged(object sender, Syncfusion.XForms.Buttons.SwitchStateChangedEventArgs e)
        {
            bool sfSwitch = (bool)e.NewValue;
            
                viewModel.ActiveHuellaCommand.Execute(sfSwitch);
           

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            viewModel = (PerfilUserViewModel)this.BindingContext;
        }

    }
}