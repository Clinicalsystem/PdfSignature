using MauiApp1.Models;
using MauiApp1.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp1
{
    public class AppSettings
    {
        internal static object ApiOF;
        private static DataEcommerce _DataEcommerce;
        private static string _ApiKey;
        private static string _UrlApi;

        public static DataEcommerce DataEcommerce
        {
            get
            {

                if (!string.IsNullOrEmpty(_ApiKey))
                {
                    var DataEcommerce = Preferences.Get("DataEcommerce", string.Empty);
                    _DataEcommerce = JsonConvert.DeserializeObject<DataEcommerce>(DataEcommerce);
                }

                return _DataEcommerce;
            }
            set
            {
                _DataEcommerce = value;
                Preferences.Set("DataEcommerce", JsonConvert.SerializeObject(value));
            }
        }
        public static string ApiKey
        {
            get
            {

                if (Preferences.ContainsKey("ApiKey"))
                {
                    var apy = Preferences.Get("ApiKey", string.Empty);
                    _ApiKey = JsonConvert.DeserializeObject<string>(apy);
                }

                return _ApiKey;
            }
            set
            {
                _ApiKey = value;
                Preferences.Set("ApiKey", JsonConvert.SerializeObject(value));
            }
        }
        public static string UrlApi
        {
            get
            {

                if (Preferences.ContainsKey("UrlApi"))
                {
                    var api = Preferences.Get("UrlApi", string.Empty);
                    _UrlApi = JsonConvert.DeserializeObject<string>(api);
                }

                return _UrlApi;
            }
            set
            {
                _UrlApi = value;
                Preferences.Set("UrlApi", JsonConvert.SerializeObject(value));
                ActualizateEcommerce();
            }
        }

        private static async void ActualizateEcommerce(IOpenFactura openFactura)
        {
            var resp = await openFactura.GetEcommerceData();
            if(resp.Success)
            {
                DataEcommerce = (DataEcommerce)resp.Object;
            }
        }



    }
}
