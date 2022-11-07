using MauiApp1.Models;
using Newtonsoft.Json;
using System.Net;

namespace MauiApp1.Services
{
    public class OpenFactura : IOpenFactura
    {
        public async Task<Response> GetSolicitud(int Folio, int dte, TypeDTE type)
        {
            Response Respt = new Response();
            HttpClient clients = new HttpClient();
            clients.DefaultRequestHeaders.Add("apikey", AppSettings.ApiKey);
            var url = $"{AppSettings.ApiOF}/document/{AppSettings.DataEcommerce.rut}/{dte}/{Folio}/{type.ToString()}";
            HttpResponseMessage Response = await clients.GetAsync(url);

            if (Response.StatusCode.Equals(HttpStatusCode.OK))
            {
                string jsonstring = await Response.Content.ReadAsStringAsync();

                var Object = JsonConvert.DeserializeObject<DTEresponse>(jsonstring);
                Respt = new Response
                {
                    Status = 200,
                    Message = $"Datos Obtenidos",
                    Object = Object,
                    Success = true
                };

            }
            else if (Response.StatusCode.Equals(HttpStatusCode.NotFound) || Response.StatusCode.Equals(HttpStatusCode.GatewayTimeout))
            {
                Respt = new Response
                {
                    Status = 404,
                    Message = $"Error La api de Openfactura no se encuentra disponible.",
                    Success = false
                };
            }
            else if (Response.StatusCode.Equals(HttpStatusCode.BadRequest))
            {
                string jsonstring = await Response.Content.ReadAsStringAsync();
                var Object = JsonConvert.DeserializeObject<ErrorRoot>(jsonstring);
                if (Object.error.code.ToString().Contains("OF-"))
                {

                    Respt = new Response
                    {
                        Status = 401,
                        Message = $"Error {Object.error.code},  {Object.error.message}",
                        Object = Object.error.details,
                        Success = false
                    };
                }
                else
                {
                    Respt = new Response
                    {
                        Status = 402,
                        Message = $"Solicitud fallida {Object.error.message}",
                        Object = Object.error.details,
                        Success = false
                    };
                }
            }
            return Respt;


        }

        public async Task<Response> GetEcommerceData()
        {
            Response Respt = new Response();
            HttpClient clients = new HttpClient();
            clients.DefaultRequestHeaders.Add("apikey", AppSettings.ApiKey);
            var url = $"{AppSettings.ApiOF}/organization";
            HttpResponseMessage Response = await clients.GetAsync(url);

            if (Response.StatusCode.Equals(HttpStatusCode.OK))
            {
                string jsonstring = await Response.Content.ReadAsStringAsync();

                var Object = JsonConvert.DeserializeObject<DataEcommerce>(jsonstring);
                Respt = new Response
                {
                    Status = 200,
                    Message = $"Datos Obtenidos",
                    Object = Object,
                    Success = true
                };

            }
            else if (Response.StatusCode.Equals(HttpStatusCode.NotFound) || Response.StatusCode.Equals(HttpStatusCode.GatewayTimeout))
            {
                Respt = new Response
                {
                    Status = 404,
                    Message = $"Error La api de Openfactura no se encuentra disponible.",
                    Success = false
                };
            }
            else if (Response.StatusCode.Equals(HttpStatusCode.BadRequest))
            {
                string jsonstring = await Response.Content.ReadAsStringAsync();
                var Object = JsonConvert.DeserializeObject<ErrorRoot>(jsonstring);
                if (Object.error.code.ToString().Contains("OF-"))
                {

                    Respt = new Response
                    {
                        Status = 401,
                        Message = $"Error {Object.error.code},  {Object.error.message}",
                        Object = Object.error.details,
                        Success = false
                    };
                }
                else
                {
                    Respt = new Response
                    {
                        Status = 402,
                        Message = $"Solicitud fallida {Object.error.message}",
                        Object = Object.error.details,
                        Success = false
                    };
                }
            }
            return Respt;


        }

       
    }

    public enum TypeDTE
    {
        pdf,
        xml
    }
    #region Error
    public class ErrorRoot
    {
        public Error error { get; set; }
    }
    public class Error
    {
        [JsonProperty("message")]
        public string message { get; set; }

        [JsonProperty("code")]
        public string code { get; set; }

        [JsonProperty("details")]
        public List<Detail> details { get; set; }
    }
    public class Detail
    {
        public string field { get; set; }
        public string issue { get; set; }
    }
    #endregion
}
