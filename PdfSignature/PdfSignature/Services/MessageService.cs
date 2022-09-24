using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PdfSignature.Services
{
    public class MessageService : IMessageService
    {
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
    }
}
