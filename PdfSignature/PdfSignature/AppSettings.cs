using Newtonsoft.Json;
using PdfSignature.Modelos.Autentication;
using PdfSignature.Modelos.Files;
using System;
using Xamarin.Essentials;

namespace PdfSignature
{
    public class AppSettings
    {

        #region Fields
        public static readonly string ApiFirebase = "https://pdfsignature-8079d-default-rtdb.firebaseio.com/";
        public static readonly string KeyAplication = "AIzaSyCSy16fvx05bc7pYKRlhipo8qZFx6BCL78";
        public static readonly string UrlGoogleApis = "https://identitytoolkit.googleapis.com/v1/";
        public static DocumentFile DocumentSelect;
        private static ResponseAuthentication _AuthenticationUser;
        private static string _path = string.Empty;
        private static UserData _dataUser;
        private static bool _isHuella;
        public static readonly bool IsRemember = Preferences.Get("IsRemember", false);
        #endregion
        public static ResponseAuthentication AuthenticationUser
        {
            get
            {

                if (Preferences.ContainsKey("UserAutentication"))
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
        public static UserData UserData
        {
            get
            {

                if (Preferences.ContainsKey("UserData"))
                {
                    var user = Preferences.Get("UserData", string.Empty);
                    _dataUser = JsonConvert.DeserializeObject<UserData>(user);
                }

                return _dataUser;
            }
            set
            {
                _dataUser = value;
                Preferences.Set("UserData", JsonConvert.SerializeObject(value));
            }
        }

        public static string PdfSavePath
        {
            get
            {
                if (!string.IsNullOrEmpty(_path))
                {
                    return _path;
                }

                if (Preferences.ContainsKey("PdfSavePath"))
                {
                    _path = Preferences.Get("PdfSavePath", string.Empty);
                }

                return _path;
            }
            set
            {
                _path = value;
                Preferences.Set("PdfSavePath", _path);
            }
        }

        public static bool IsHuella 
        {
            get
            {
                
                if (Preferences.ContainsKey("IsHuella"))
                {
                    _isHuella = Preferences.Get("IsHuella", false);
                }

                return _isHuella;
            }
            set
            {
                _isHuella = value;
                Preferences.Set("IsHuella", _isHuella);
            }
        }

        public static string ApiAuthentication(UriApi uriApi)
        {
            switch (uriApi)
            {
                case UriApi.Loging:
                    return $"{UrlGoogleApis}accounts:signInWithPassword?key={KeyAplication}";

                case UriApi.Sign:
                    return $"{UrlGoogleApis}accounts:signUp?key={KeyAplication}";

                case UriApi.Token:
                    return $"{UrlGoogleApis}token?key={KeyAplication}";

                case UriApi.PasswordReset:
                    return $"{UrlGoogleApis}accounts:sendOobCode?key={KeyAplication}";
                case UriApi.ChangePasswor:
                    return $"{UrlGoogleApis}accounts:update?key={KeyAplication}";

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
        PasswordReset,
        ChangePasswor
    }

    public enum StyleText
    {

        ToUppper,
        ToLover,
        ToTitleCase
    }
}
