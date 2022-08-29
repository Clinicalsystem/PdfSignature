using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xamarin.Essentials;

namespace PdfSignature.Modelos.Files
{
    public class Document : DocumentFile
    {
        
        public Document(FileResult fileResult = null, string password = "")
        {
            FileName = fileResult.FileName;
            Path = fileResult.FullPath;
            Stream = new StreamReader(fileResult.FileName).BaseStream; 
            Date = DateTime.Now;
            PasswordPdf = password;

        }
        public void ToCovert(DocumentFile documentFile)
        {
            FileName = documentFile.FileName;
            Path = documentFile.Path;
            Stream = documentFile.Stream;
            Date = documentFile.Date;
            PasswordPdf = documentFile.PasswordPdf;
            Id = documentFile.Id;
            FireBaseID = documentFile.FireBaseID;
        }
       

    }
}
