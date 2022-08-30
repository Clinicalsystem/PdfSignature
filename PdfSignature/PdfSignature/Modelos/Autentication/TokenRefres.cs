using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace PdfSignature.Modelos.Autentication
{
    public class TokenRefres
    {
        [JsonProperty("grant_type")]
        public string Grant_type => "refresh_token";

        [JsonProperty("refreshToken")]
        public string RefreshToken { get; set; }
    }
}
