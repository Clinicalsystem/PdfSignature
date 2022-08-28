using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xamarin.Forms;

namespace PdfSignature.ViewModels
{
    
    public class PdfViewModeel : PdfViewerViewModel
    {
        #region Fields

        private Stream _pdfDocumentStream;

        #endregion

        #region Contructor
        public PdfViewModeel()
        {
        }
        #endregion

        #region Property
        

        #endregion

        #region Command

        public Command SignatureCommand { get; set; }

        public Command OpenFileCommand { get; set; }

        public Command ShareCommand { get; set; }

        public Command SaveDocumentCommand { get; set; }

        #endregion
    }
}
