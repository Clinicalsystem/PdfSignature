using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PdfSignature.Views.Home
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomeList : ContentPage
    {
        public HomeList()
        {
            InitializeComponent();
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
           await  Task.Run(() =>
                {
                Task.Delay(1000).Wait();
                listRecients.SelectedItems.Clear();
                listFavorits.SelectedItems.Clear();
                });
        }
    }
}