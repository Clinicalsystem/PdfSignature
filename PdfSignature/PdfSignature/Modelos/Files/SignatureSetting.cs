using Newtonsoft.Json;
using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Drawing;
using System.Linq;
using Xamarin.Forms;
using System.Security.Principal;

namespace PdfSignature.Modelos.Files
{
    public class SignatureSetting
    {
        [PrimaryKey]
        [JsonIgnore]
        public string Key { get; set; }

        [JsonIgnore]
        public DateTime Date => DateTime.Now;
        public bool IsImagePersonal { get; set; }

        public string ImagePersonal { get; set; }

        public bool IsWaterMark { get; set; }

        public string WaterMark { get; set; }

        public bool IsEmisor { get; set; }

        public bool IsLocation { get; set; }

        public bool IsRut { get; set; }

        public bool IsCN { get; set; }

        public string Location { get; set; }

        public bool IsCompany { get; set; }

        public string Company { get; set; }

        public bool IsReason { get; set; }

        public StyleText StyleSignature { get; set; } 

        [Ignore]
        public Reason Reason { get; set; }

        public int IndexReason { get; set; }

        [JsonIgnore]
        public string MyReason => Reason.reason.ElementAt(IndexReason);


        [ForeignKey(typeof(Signature))]
        public string SignatureId { get; set; }

        [OneToOne]
        public Signature Signature { get; set; }

        public SignatureSetting()
        {
            IsEmisor = true;
            IsReason = true;
            Reason = new Reason();

            
        }
        
        public Stream ImagePersonalStream()
        {
            Stream stream = null;   
            if (!string.IsNullOrEmpty(ImagePersonal))
            {
                byte[] bytes = Convert.FromBase64String(ImagePersonal);
                stream = new MemoryStream(bytes);
            }
            return stream;  
        }
       
        public Stream WaterMarkStream()
        {
            Stream stream = null;

            if (!string.IsNullOrEmpty(WaterMark))
            {
                byte[] bytes = Convert.FromBase64String(WaterMark);
                stream = new MemoryStream(bytes);
            }
            return stream;
        }
    }

    public class Reason
    {
        public List<string> reason { get; set; }

        public Reason()
        {
            reason = new List<string>()
            {
                "Soy el propietario de este documento.",
                "He revisado este documento.",
                "Estoy deacuerdo con el contenido de este documento.",
                "Mi firma en este documento representa mi aceptación de los términos definidos"

            };
        }
    }
}
