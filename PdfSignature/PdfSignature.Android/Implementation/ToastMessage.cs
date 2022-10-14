using Android.App;
using Android.Widget;
using PdfSignature.Android.Implementation;
using PdfSignature.Services;

[assembly: Xamarin.Forms.Dependency(typeof(ToastMessage))]

namespace PdfSignature.Android.Implementation
{
    public class ToastMessage : IToast
    {
        public void Show(string message)
        {
            Toast.MakeText(Application.Context, message, ToastLength.Long).Show();
        }
    }
}