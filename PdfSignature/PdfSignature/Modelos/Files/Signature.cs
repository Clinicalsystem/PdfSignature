using Newtonsoft.Json;
using PdfSignature.Services;
using SQLite;
using SQLiteNetExtensions.Attributes;
using Syncfusion.Pdf.Security;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PdfSignature.Modelos.Files
{
    public class Signature
    {
        [PrimaryKey]
        [JsonIgnore]
        public string FireBaselId { get; set; }

        public string LoaclId { get; set; }

        public string Name { get; set; }

        public string Rut { get; set; }

        public bool IsExitRut => string.IsNullOrEmpty(Rut) || Rut.Length < 7 ? false : true;
        public string CN { get; set; }

        public DateTime DateRegister { get; set; }

        public DateTime Expire { get; set; }

        [JsonIgnore]
        [Ignore]
        public TimeSpan DaysOfExpire => DateTime.Now >= Expire ? new TimeSpan(0) : Expire.Subtract(DateTime.Now);

        public string FileBase64 { get; set; }

        public string Password { get; set; }

        public string Emisor { get; set; }

        public byte[] Serial { get; set; }

        public int Version { get; set; }

        [OneToOne(CascadeOperations = CascadeOperation.All)]
        public SignatureSetting Setting { get; set; }


        public PdfCertificate Certificate()
        {
            PdfCertificate pdfCert = null;
            if (!string.IsNullOrEmpty(FileBase64))
            {
                byte[] bytes = Convert.FromBase64String(FileBase64);
                Stream stream = new MemoryStream(bytes);
                pdfCert = new PdfCertificate(stream, Password);
            }
            return pdfCert;
            
        }
    }
}
