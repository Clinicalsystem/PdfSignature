using Newtonsoft.Json;
using PdfSignature.Modelos.Autentication;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

namespace PdfSignature
{
    public class AppSettings
    {

        #region Fields
        public static readonly string ApiFirebase = "https://pdfsignature-8079d-default-rtdb.firebaseio.com/";
        public static readonly string KeyAplication = "AIzaSyCSy16fvx05bc7pYKRlhipo8qZFx6BCL78";
        public static readonly string UrlGoogleApis = "https://identitytoolkit.googleapis.com/v1/";
        private static ResponseAuthentication _AuthenticationUser;
        public static readonly bool IsRemember = Preferences.Get("IsRemember", false);
        #endregion
        public static ResponseAuthentication AuthenticationUser 
        {
            get 
            {
               
                    if(Preferences.ContainsKey("UserAutentication"))
                    {
                        var user = Preferences.Get("UserAutentication", string.Empty);
                        _AuthenticationUser = JsonConvert.DeserializeObject<ResponseAuthentication>(user);
                    }
                   
                return _AuthenticationUser;
            }
            set 
            { 
                _AuthenticationUser = value;
                Preferences.Set("UserAutentication", JsonConvert.SerializeObject(value));
            }
        }

        public static string ApiAuthentication(UriApi uriApi)
        {
            switch (uriApi)
            {
                case UriApi.Loging:
                    return  $"{UrlGoogleApis}accounts:signInWithPassword?key={KeyAplication}";

                case UriApi.Sign:
                    return $"{UrlGoogleApis}accounts:signUp?key={KeyAplication}";

                case UriApi.Token:
                    return $"{UrlGoogleApis}token?key={KeyAplication}";

                case UriApi.PasswordReset:
                    return $"{UrlGoogleApis}accounts:sendOobCode?key={KeyAplication}";

                default:
                    return String.Empty;

            }

        }

    }

    public enum UriApi
    {
        Token,
        Loging,
        Sign,
        PasswordReset
    }
}
