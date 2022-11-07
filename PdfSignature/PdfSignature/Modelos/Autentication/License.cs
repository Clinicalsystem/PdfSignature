using PdfSignature.Validators.Rules;
using System;
using System.Collections.Generic;
using System.Text;

namespace PdfSignature.Modelos.Autentication
{
    public class License
    {
        public string Name { get; set; }

        public DateTime Expire { get; set; }

        public int DeviceNumber { get; set; }
        
        public bool IsValid => Expire < DateTime.Now ? false : true;
        public int SignatureNumber { get; set; }


    }
}
