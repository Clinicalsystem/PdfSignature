using Syncfusion.Pdf.Parsing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using Xamarin.Forms;

namespace PdfSignature.ViewModels
{
    
    public class PdfViewModeel : PdfViewerViewModel
    {
        #region Fields

        public static PdfLoadedDocument _signatureDocument { get; set; }

        #endregion

        #region Contructor
        public PdfViewModeel()
        {
            if(_pdfDocumentStream != null)
            {
                _signatureDocument = new PdfLoadedDocument(_pdfDocumentStream);
            }
            
        }
        #endregion

        #region Property
        

        #endregion

        #region Command

        public Command SignatureCommand { get; set; }

        public Command ShareCommand { get; set; }

        public Command SaveDocumentCommand { get; set; }

        #endregion

        #region methods
        private void SaveDocument(object obj)
        {
            try
            {
                _signatureDocument.Save(_pdfDocumentStream);
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
                _signatureDocument.Save(_pdfDocumentStream);
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
                _signatureDocument.Save(_pdfDocumentStream);
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

    }
}
