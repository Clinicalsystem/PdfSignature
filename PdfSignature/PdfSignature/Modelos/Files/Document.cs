using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xamarin.Essentials;

namespace PdfSignature.Modelos.Files
{
    public class Document 
    {

        public string FireBaseID { get; set; }
        public string FileName { get; set; }

        public string PdfBase64 { get; set; }

        public DateTime Date { get; set; }

        public string PasswordPdf { get; set; }

        public override string ToString()
        {
            return String.Format("{0} : {1}", FireBaseID, FileName);
        }

    }
}
