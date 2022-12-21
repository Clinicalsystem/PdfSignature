using DowloadXmlPDF.Models.OF;
using DowloadXmlPDF.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DowloadXmlPDF
{
    public class AppSettings
    {
        
        private static DataEcommerce _DataEcommerce;
        private static string _ApiKey;
        private static string _UrlApi;
        private static OpenFactura _OpenFactura;

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
                else if(UrlApi == null || UrlApi.Contains("dev"))
                {
                    _ApiKey = "928e15a2d14d4a6292345f04960f4bd3";
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
                else
                {
                    _UrlApi = "https://dev-api.haulmer.com";
                }

                return _UrlApi;
            }
            set
            {
                _UrlApi = value;
                Preferences.Set("UrlApi", JsonConvert.SerializeObject(value));
                //ActualizateEcommerce();
            }
        }

        private static async void ActualizateEcommerce()
        {
            var resp = await _OpenFactura.GetEcommerceData(ApiKey);
            if(resp.Success)
            {
                DataEcommerce = (DataEcommerce)resp.Object;
            }
        }

        public AppSettings(IOpenFactura openFactura)
        {
            _OpenFactura = (OpenFactura)openFactura;
        }


    }
}
