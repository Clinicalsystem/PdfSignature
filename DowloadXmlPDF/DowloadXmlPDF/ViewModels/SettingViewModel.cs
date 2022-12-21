using DowloadXmlPDF.Models.OF;
using DowloadXmlPDF.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DowloadXmlPDF.ViewModels
{
    public class SettingViewModel: BaseViewModel
    {
        private readonly IOpenFactura _openFactura;
        public SettingViewModel(IOpenFactura openFactura)
        {
                _openFactura = openFactura;

                if (Preferences.ContainsKey("Demo"))
                {
                    IsDemo = Preferences.Get("Demo", true);
                    if(_demo)
                    {
                         AppSettings.UrlApi = ApiUrlDemo;
                         GetEcommerce(ApiKeyDemo);
                         
                    //DisplayAlert.Show("Esta usando la Api de la versión demo.");
                    }
                    else
                    {
                    AppSettings.UrlApi = ApiUrl;
                    GetEcommerce(Apikey);
                    
                    }
                
                }
                else
                {
                    IsDemo = true;
                    AppSettings.UrlApi = ApiUrlDemo;
                    GetEcommerce(ApiKeyDemo);
                    
                 //DisplayAlert.Show("Esta usando la Api de la versión demo.");
                }

        }
        #region Fields
        private string _ApiKey;
        
        public string ApiUrlDemo = "https://dev-api.haulmer.com/V2/dte";

        public string ApiUrl = "https://api.haulmer.com/V2/dte";

        public string ApiKeyDemo = "928e15a2d14d4a6292345f04960f4bd3";


        private DataEcommerce _Ecommerce;
        private bool _demo;
        #endregion

        #region Propieties
        public bool IsDemo
        {
            get
            {
                return _demo;
            }
            set
            {
                Preferences.Set("Demo", value);
                SetProperty(ref _demo, value);

            }
        }

        public DataEcommerce Ecommerce
        {
            get
            {
                return _Ecommerce;
            }
            set
            {
                SetProperty(ref _Ecommerce, value);

            }
        }
        

        public string Apikey
        {
            get 
            { 
                return _ApiKey; 
            }
            set 
            {
                AppSettings.ApiKey = _ApiKey;
                
                SetProperty(ref _ApiKey, value);
            
            }
        }

        

        #endregion

        #region Command

        #endregion

        #region Methods
        private async void GetEcommerce(string api)
        {
            var data = await _openFactura.GetEcommerceData(api);
            if(data.Success)
            {
                Ecommerce = (DataEcommerce)data.Object;
            }
            else
            {
              //  _DisplayAler.Show(data.Message);
            }

        }
        #endregion
    }
}
