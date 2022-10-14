using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PdfSignature.Services
{
    public class MessageService : IMessageService
    {
        private readonly IToast _toast;
        public MessageService()
        {
            _toast = DependencyService.Get<IToast>();
        }
        public async Task<string> Info(string messagestring)
        {
            
            return await App.Current.MainPage.DisplayPromptAsync("PdfSignature", messagestring, "Ok", "Cancelar","*******");
        }

        public async Task<bool> QuestionAsync(string message, string aceptar, string cancelar)
        {
            return await App.Current.MainPage.DisplayAlert("PdfSignature", message, aceptar, cancelar);
        }

        public async Task ShowAsync(string message)
        {
            await App.Current.MainPage.DisplayAlert("PdfSignature", message,"Ok");
        }

        public async Task<string> ShowAsync(string[] message)
        {
           return await App.Current.MainPage.DisplayActionSheet("PdfSignature", "Ok", "Cancelar", message);
        }

        public void Toast(string messagestring)
        {
            _toast.Show(messagestring);
        }
    }
}
