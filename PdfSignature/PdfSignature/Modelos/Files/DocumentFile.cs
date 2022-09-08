using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PdfSignature.Modelos.Files
{
    public class DocumentFile
    {
        [PrimaryKey]
        public int Id { get; set; }

        public string FireBaseID { get; set; }    
        public string FileName { get; set; }

        public string PdfBase64 { get; set; }

        public string Path { get; set; }

        public DateTime Date { get; set; }

        public string PasswordPdf { get; set; }

        public override string ToString()
        {
            return String.Format("{0} : {1}", FireBaseID, FileName);
        }
    }
}
