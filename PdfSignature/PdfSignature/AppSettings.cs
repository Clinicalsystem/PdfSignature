using PdfSignature.Modelos.Autentication;
using System;
using System.Collections.Generic;
using System.Text;

namespace PdfSignature
{
    public class AppSettings
    {
       

        public static readonly string ApiFirebase = "https://pdfsignature-8079d-default-rtdb.firebaseio.com/";
        public static readonly string KeyAplication = "AIzaSyCSy16fvx05bc7pYKRlhipo8qZFx6BCL78";

        public static ResponseAuthentication AuthenticationUser { get; set; }

        public static readonly string UrlGoogleApis = "https://identitytoolkit.googleapis.com/v1/";

        public static string ApiAuthentication(UriApi uriApi)
        {
            switch (uriApi)
            {
                case UriApi.Loging:
                    return UrlGoogleApis + "accounts:signInWithPassword?key=" + KeyAplication;

                case UriApi.Sign:
                    return UrlGoogleApis + "accounts:signUp?key=" + KeyAplication;
                case UriApi.Token:
                    return UrlGoogleApis + "token?key=" + KeyAplication;
                default:
                    return String.Empty;

            }

        }

    }
    public enum UriApi
    {
        Token,
        Loging,
        Sign
    }
}
