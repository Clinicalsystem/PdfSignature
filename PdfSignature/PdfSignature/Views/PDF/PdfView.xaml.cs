using PdfSignature.Controls;
using PdfSignature.Implementation;
using PdfSignature.Services;
using PdfSignature.ViewModels;
using Syncfusion.Pdf;
using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Shapes;
using Xamarin.Forms.Xaml;
using static Xamarin.Forms.Internals.GIFBitmap;
using Rect = Xamarin.Forms.Rect;

namespace PdfSignature.Views.PDF
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PdfView : ContentPage
    {
        #region Fields
        private IMessageService _messageService;
        PdfViewModeel model;
        double currentScale = 1;
        double startScale = 1;
        double xOffset = 0;
        double yOffset = 0;
        PinchGestureRecognizer PinchGesture = new PinchGestureRecognizer();
        Rect rect = new Rect();
        private Point originalPoint;
        long touchId = -1;
        #endregion
        public PdfView()
        {
            _messageService = new MessageService(); 
        PinchGesture.PinchUpdated += OnPinchUpdated;
            InitializeComponent();
            CustomPdfBar();
        }

        private void CustomPdfBar()
        {
            pdfViewer.Toolbar.SetToolbarItemVisibility("signature", false);
        }

        private void OnPinchUpdated(object sender, PinchGestureUpdatedEventArgs e)
        {
            if (e.Status == GestureStatus.Started)
            {
                startScale = Content.Scale;
                Content.AnchorX = 0;
                Content.AnchorY = 0;
            }
            if (e.Status == GestureStatus.Running)
            {
                currentScale += (e.Scale - 1) * startScale;
                currentScale = Math.Max(1, currentScale);
                double renderedX = Content.X + xOffset;
                double deltaX = renderedX / Width;
                double deltaWidth = Width / (Content.Width * startScale);
                double originX = (e.ScaleOrigin.X - deltaX) * deltaWidth;
                double renderedY = Content.Y + yOffset;
                double deltaY = renderedY / Height;
                double deltaHeight = Height / (Content.Height * startScale);
                double originY = (e.ScaleOrigin.Y - deltaY) * deltaHeight;
                double targetX = xOffset - (originX * Content.Width) * (currentScale - startScale);
                double targetY = yOffset - (originY * Content.Height) * (currentScale - startScale);
                Content.TranslationX = targetX.Clamp(-Content.Width * (currentScale - 1), 0);
                Content.TranslationY = targetY.Clamp(-Content.Height * (currentScale - 1), 0);
                Content.Scale = currentScale;
            }
            if (e.Status == GestureStatus.Completed)
            {
                xOffset = Content.TranslationX;
                yOffset = Content.TranslationY;
            }
        }

        private void ImageButton_Clicked(object sender, EventArgs e)
        {

        }

        

        private async void pdfViewer_DocumentSaveInitiated(object sender, Syncfusion.SfPdfViewer.XForms.DocumentSaveInitiatedEventArgs args)
        {
            string _Path = string.Empty;
            Stream stream = args.SaveStream;
            
            var document = AppSettings.DocumentSelect;
           // string _Path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "PdfSignature");
            string name = $"{document.FileName.Remove(document.FileName.Length - 4)}_Firmado.pdf";
            if (Device.RuntimePlatform == Device.Android)
            {
                var status = await CheckAndRequestStorageWrite();
                switch (status)
                {
                    case PermissionStatus.Granted:
                        _Path = await DependencyService.Get<IFileManager>().Save(stream as MemoryStream, name);
                        break;
                   
                }
            }
            else if (Device.RuntimePlatform == Device.iOS)
            {

            }
            else
            {
                //byte[] data = ReadFully(stream.BaseStream);
             _Path =  await DependencyService.Get<IFileManager>().Save(stream as MemoryStream, name);
            }
            AppSettings.PdfSavePath = _Path;
            await _messageService.ShowAsync($"Se guardo el archivo correctamente en la ruta: {_Path}");

        }
        private byte[] ReadFully(Stream input)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                input.CopyToAsync(ms);
                return ms.ToArray();
            }
        }
        private async Task<PermissionStatus> CheckAndRequestStorageWrite()
        {
            var status = await Permissions.CheckStatusAsync<Permissions.StorageWrite>();

            if (status == PermissionStatus.Granted)
                return status;

            if (status == PermissionStatus.Denied && Device.RuntimePlatform == Device.Android)
            {
                await _messageService.ShowAsync("El Usuario denego el permiso para guardar archivos en el dispostivo.");
                return status;
            }

            if (Permissions.ShouldShowRationale<Permissions.StorageWrite>())
            {
                await _messageService.ShowAsync("Se requiere su permiso para almacenar los archivos firmados en el dispositivo, por favor conceda el permiso.");
            }

            status = await Permissions.RequestAsync<Permissions.StorageWrite>();

            return status;
        }


         private void OnTouchEffectAction(object sender, TouchActionEventArgs args)
            {
                // Convert Xamarin.Forms point to pixels
                Point pt = args.Location;

            string direc = string.Empty;

                switch (args.Type)
                {
                    case TouchActionType.Pressed:
                        // Find transformed bitmap rectangle
                        rect = new Rect(pt.X, pt.Y, 10, 10);
                        originalPoint = new Point(pt.X, pt.Y);
                        touchId = args.Id;
                   // model.OnTouchEffectCommand.Execute(rect);
                    break;

                    case TouchActionType.Moved:
                    if (touchId == args.Id && args.IsInContact)
                    {
                        // Adjust the matrix for the new position
                        var X = (pt.X - originalPoint.X);
                        var Y = (pt.Y - originalPoint.Y);
                        if (X < 0 && Y < 0) //Diagonal izq arriba
                        {
                            rect.Location = pt;
                            rect.Width = originalPoint.X - pt.X;
                            rect.Height = originalPoint.Y - pt.Y;

                            model.OnTouchEffectCommand.Execute(rect);

                        }
                        if (X < 0 && Y > 0) //Diagonal izq Abajo
                        {
                            rect.Width = (originalPoint.X - pt.X);
                            rect.Height = (originalPoint.Y - pt.Y) * -1;
                            var pont = new Point(pt.X, pt.Y - rect.Height);
                            rect.Location = pont;
                            model.OnTouchEffectCommand.Execute(rect);
                        }

                        else if (X > 0 && Y < 0) //Diagonal Derec arriba
                        {
                            rect.Width = (originalPoint.X - pt.X) * -1;
                            rect.Height = (originalPoint.Y - pt.Y);
                            var pont = new Point(pt.X - rect.Width, pt.Y);
                            rect.Location = pont;
                            model.OnTouchEffectCommand.Execute(rect);

                        }
                        else if (X > 0 && Y > 0) //Diagonal Derec abajo 
                        {
                            rect.Width = (pt.X - originalPoint.X);
                            rect.Height = (pt.Y - originalPoint.Y);
                            model.OnTouchEffectCommand.Execute(rect);
                            
                        }
                    }
                        break;
                case TouchActionType.Entered:
                    break;
                case TouchActionType.Released:
                    
                    model.OnTouchEffectCommand.Execute(rect);
                    model.TouchSignatureCommand.Execute(false);
                    break;

                default:
                        touchId = -1;
                        break;
                }
            }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            model = (PdfViewModeel)this.BindingContext;
        }

        private void SfComboBox_SelectionChanged(object sender, Syncfusion.XForms.ComboBox.SelectionChangedEventArgs e)
        {
            Syncfusion.XForms.ComboBox.SfComboBox sfCombo = sender as Syncfusion.XForms.ComboBox.SfComboBox;
            switch(sfCombo.SelectedIndex)
            {
                case 0:
                    model.CertSelect.Setting.StyleSignature = StyleText.ToUppper;
                    break;
                case 1:
                    model.CertSelect.Setting.StyleSignature = StyleText.ToLover;
                    break;
                case 2:
                    model.CertSelect.Setting.StyleSignature = StyleText.ToTitleCase;
                    break;
                   
                default:
                    break;

            }


        }
    }
}