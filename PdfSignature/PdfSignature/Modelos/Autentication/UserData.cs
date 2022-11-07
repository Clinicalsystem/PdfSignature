using PdfSignature.Modelos.Devices;
using System;
using System.Collections.Generic;
using System.Text;

namespace PdfSignature.Modelos.Autentication
{
    public class UserData :  RegisterUser
    {
        public UserData()
        {
            PdfDevices = new List<PdfDevice>();
        }

        public List<PdfDevice> PdfDevices { get; set; }

        public string LocalId { get;  set; }

        public License License { get; set; }
    }
}
