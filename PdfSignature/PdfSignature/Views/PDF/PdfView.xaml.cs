﻿using PdfSignature.Implementation;
using System;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace PdfSignature.Views.PDF
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PdfView : ContentPage
    {
        #region Fields
        private float _ZoomFactor = 0;
        double currentScale = 1;
        double startScale = 1;
        double xOffset = 0;
        double yOffset = 0;
        PinchGestureRecognizer PinchGesture = new PinchGestureRecognizer();
        #endregion
        public PdfView()
        {
            PinchGesture.PinchUpdated += OnPinchUpdated;
            InitializeComponent();
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
            if (Device.RuntimePlatform == Device.Android || Device.RuntimePlatform == Device.iOS)
            {

            }
            else
            {
                //byte[] data = ReadFully(stream.BaseStream);
             _Path =  await DependencyService.Get<IFileManager>().Save(stream as MemoryStream, name);
            }
            AppSettings.PdfSavePath = _Path;

        }
        private byte[] ReadFully(Stream input)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                input.CopyToAsync(ms);
                return ms.ToArray();
            }
        }
    }
}