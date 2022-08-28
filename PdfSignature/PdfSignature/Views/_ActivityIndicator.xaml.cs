using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PdfSignature.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class _ActivityIndicator : ContentView
    {
       
        public static readonly BindableProperty IsRunningProperty = BindableProperty.Create(nameof(IsRunning), typeof(bool), typeof(_ActivityIndicator), false);
        public static readonly BindableProperty SourceProperty = BindableProperty.Create(nameof(Source), typeof(ImageSource), typeof(_ActivityIndicator), default(ImageSource));

        public _ActivityIndicator()
        {
            InitializeComponent();
            BindingContext = this;
        }



        public bool IsRunning
        {
            get => (bool)GetValue(IsRunningProperty);
            set => SetValue(IsRunningProperty, value);
        }

        public ImageSource Source
        {
            get => (ImageSource)GetValue(SourceProperty);
            set => SetValue(SourceProperty, value);
        }
    
    }
}