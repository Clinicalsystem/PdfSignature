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
        public async Task ShowAsync(string message)
        {
            await App.Current.MainPage.DisplayAlert("PdfSignature", message,"Ok");
        }

       
    }
}
