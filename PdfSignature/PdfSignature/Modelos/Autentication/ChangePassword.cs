using System;
using System.Collections.Generic;
using System.Text;

namespace PdfSignature.Modelos.Autentication
{
    public class ChangePassword
    {
        public string idToken { get; set; }

        public string password { get; set; }

        public bool returnSecureToken { get; set; }
    }
}
