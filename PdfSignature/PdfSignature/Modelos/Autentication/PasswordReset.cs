using System;
using System.Collections.Generic;
using System.Text;

namespace PdfSignature.Modelos.Autentication
{
    
    public class PasswordReset
    {
        public PasswordReset()
        {
            requestType = "PASSWORD_RESET";
        }
        public string requestType { get; set; }

        public string email { get; set; }
    }
}
