using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace PdfSignature.Modelos.Autentication
{
    public class RegisterUser
    {
        public string UserName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        [JsonIgnore]
        public string PasswordVerifie { get; set; }
    }
}
