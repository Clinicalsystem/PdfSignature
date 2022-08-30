using System;
using System.Collections.Generic;
using System.Text;

namespace PdfSignature.Modelos.Autentication
{
    public class Login
    {
        public Login()
        {
            returnSecureToken = true;
        }
        public string email { get; set; }

        public string password { get; set; }

        public bool returnSecureToken { get; set; }
    }
}
