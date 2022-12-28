using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

namespace DowloadXmlPdf.Services
{
    public class MessageService : IMessageService
    {
        
        public async Task<string> Info(string messagestring)
        {
            return await App.Current.MainPage.DisplayPromptAsync("DowloadXmlPDF", messagestring, "Ok", "Cancelar");
        }

        public async Task<bool> QuestionAsync(string message, string aceptar, string cancelar)
        {
            return await App.Current.MainPage.DisplayAlert("PdfSignature", message, aceptar, cancelar);
        }

        public async Task Show(string message)
        {
            await App.Current.MainPage.DisplayAlert("PdfSignature", message, "Ok");
        }

        public async Task<string> ShowAsync(string[] message)
        {
            return await App.Current.MainPage.DisplayActionSheet("DowloadXmlPDF", "Ok", "Cancelar", message);
        }

        public async void ToastMessage(string messagestring)
        {
            var toas = Toast.Make(messagestring, ToastDuration.Long, 14);
            await toas.Show();
        }
    }
}
