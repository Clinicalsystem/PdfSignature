using Syncfusion.Pdf.Parsing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace PdfSignature.ViewModels
{
    
    public class PdfViewModeel : PdfViewerViewModel
    {
        #region Fields

        public static PdfLoadedDocument _signatureDocument { get; set; }
        Command<object> saveCommand;
        #endregion

        #region Contructor
        public PdfViewModeel()
        {
            if(_pdfDocumentStream != null)
            {
                _signatureDocument = new PdfLoadedDocument(_pdfDocumentStream);
            }
            this.SignatureCommand = new Command(this.SignatureDocument);
            this.ShareCommand = new Command(this.ShareDocument);
            this.SaveCommand = new Command<object>(this.SaveDocument);
        }
        #endregion

        #region Property

        #endregion

        #region Command

        public Command SignatureCommand { get; set; }

        public Command ShareCommand { get; set; }

        public Command<object> SaveCommand
        {
            get { return saveCommand; }
            protected set { saveCommand = value; }
        }

        #endregion

        #region methods
        private void SaveDocument(object obj)
        {
            try
            {
                MemoryStream stream = new MemoryStream();
                _signatureDocument.Save(stream);
                var document = AppSettings.DocumentSelect;
                string _Path = $"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}/PdfSignature";
                string name = $"{document.FileName.Remove(document.FileName.Length - 4)}_Firmado.pdf";
                
                _Path =Path.Combine(_Path, name);
                if(!Directory.Exists(_Path))
                {
                    Directory.CreateDirectory(_Path);   
                }

                File.WriteAllBytes(_Path, stream.GetBuffer());

            }
            catch (Exception)
            {

                throw;
            }
        }

        private void ShareDocument(object obj)
        {
            try
            {
                MemoryStream stream = new MemoryStream();
                _signatureDocument.Save(stream);
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void SignatureDocument(object obj)
        {
            try
            {
                //_signatureDocument.Save(_pdfDocumentStream);
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

    }
}
