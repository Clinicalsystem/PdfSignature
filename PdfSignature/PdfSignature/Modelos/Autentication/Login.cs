using System;
using System.Collections.Generic;
using System.Text;

namespace PdfSignature.Modelos.Autentication
{
    public class Login
    {
        public Login()
        {
            retourSegureToken = true;
        }
        public string email { get; set; }

        public string password { get; set; }

        public bool retourSegureToken { get; set; }
    }
}
