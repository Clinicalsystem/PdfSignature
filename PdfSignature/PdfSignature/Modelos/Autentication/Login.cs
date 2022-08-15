using System;
using System.Collections.Generic;
using System.Text;

namespace PdfSignature.Modelos.Autentication
{
    public class Login
    {
        public Login()
        {
            RetourSegureToken = true;
        }
        public string Email { get; set; }

        public string Password { get; set; }

        public bool RetourSegureToken { get; set; }
    }
}
