using DowloadXmlPDF.Models;
using DowloadXmlPDF.Models.OF;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using System.Text.Json;

namespace DowloadXmlPDF.Services
{
    public class OpenFactura : IOpenFactura
    {
        public async Task<Response> GetDocument(int Folio, int dte, Type type)
        {
            Response Respt = new Response();
            HttpClient clients = new HttpClient();
            clients.DefaultRequestHeaders.Add("apikey", AppSettings.ApiKey);
            var url = $"{AppSettings.UrlApi}/document/{AppSettings.DataEcommerce.rut}/{dte}/{Folio}/{type.ToString()}";
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
            else if (Response.StatusCode.Equals(HttpStatusCode.Unauthorized))
            {
                Respt = new Response
                {
                    Status = 401,
                    Message = $"Acceso denegado a OpenFactura, es probable que la api suministrada este incorrepta o su suscripción esta suspendida, contacte al soporte de haulmer.",
                    Success = false
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

        public async Task<Response> GetEmitidos(Filters filters)
        {
            #region Fields
            Response Respt = new Response();
            JsonSerializerSettings jsonSerializerSettings = new()
            {
                NullValueHandling = NullValueHandling.Ignore
            };
            HttpClient clients = new HttpClient();
            clients.DefaultRequestHeaders.Add("apikey", AppSettings.ApiKey);
            var url = $"{AppSettings.UrlApi}/document/issued";
            #endregion

            #region Solicitud al api
            string body = JsonConvert.SerializeObject(filters, jsonSerializerSettings);
            StringContent content = new StringContent(body, Encoding.UTF8, "application/json");

            HttpResponseMessage Response = await clients.PostAsync(url, content);
            #endregion

            #region Deserializa response
            if (Response.StatusCode.Equals(HttpStatusCode.OK))
            {
                string jsonstring = await Response.Content.ReadAsStringAsync();

                ResponseDteList responseDteList = JsonConvert.DeserializeObject<ResponseDteList>(jsonstring);



                //if (Object.last_page > Object.current_page)
                //{
                //    do
                //    {
                //        Object.current_page++;
                //        Models.OF.Page page = new Models.OF.Page();
                //        page.eq = Object.current_page.ToString();

                //        filters.Page = page;

                //        body = JsonConvert.SerializeObject(filters);
                //        content = new StringContent(body, Encoding.UTF8, "application/json");
                //        Response = await clients.PostAsync(url, content);
                //        if (Response.StatusCode.Equals(HttpStatusCode.OK))
                //        {
                //            jsonstring = await Response.Content.ReadAsStringAsync();

                //            Object = JsonConvert.DeserializeObject<ResponseDteList>(jsonstring);

                //            ListData.AddRange(Object.Data);
                //        }

                //    } while (Object.current_page == Object.last_page);
                //}

                Respt = new Response
                {
                    Status = 200,
                    Message = $"Datos Obtenidos",
                    Object = responseDteList,
                    Success = true
                };

            }
            else if (Response.StatusCode.Equals(HttpStatusCode.Unauthorized))
            {
                Respt = new Response
                {
                    Status = 401,
                    Message = $"Acceso denegado a OpenFactura, es probable que la api suministrada este incorrepta o su suscripción esta suspendida, contacte al soporte de haulmer.",
                    Success = false
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
            #endregion

            return Respt;


        }
        public async Task<Response> GetEcommerceData(string ApiKey)
        {
            Response Respt = new Response();
            HttpClient clients = new HttpClient();
            clients.DefaultRequestHeaders.Add("apikey", ApiKey);
            var url = $"{AppSettings.UrlApi}/organization";
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
            else if (Response.StatusCode.Equals(HttpStatusCode.Unauthorized))
            {
                Respt = new Response
                {
                    Status = 401,
                    Message = $"Acceso denegado a OpenFactura, es probable que la api suministrada este incorrepta o su suscripción esta suspendida, contacte al soporte de haulmer.",
                    Success = false
                };
            }
            else if (Response.StatusCode.Equals(HttpStatusCode.NotFound) || Response.StatusCode.Equals(HttpStatusCode.GatewayTimeout))
            {
                Respt = new Response
                {
                    Status = 404,
                    Message = $"Error La api de Openfactura no se encuentra disponible. Verifique el estado en status.haulmer.com o contacte a Haulmer.",
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

        public async Task<Response> GetRecibidos(Filters filters)
        {
            #region Fields
            Response Respt = new Response();
            HttpClient clients = new HttpClient();
            JsonSerializerSettings jsonSerializerSettings = new()
            {
                NullValueHandling = NullValueHandling.Ignore
            };
            clients.DefaultRequestHeaders.Add("apikey", AppSettings.ApiKey);
            var url = $"{AppSettings.UrlApi}/document/received";
            #endregion

            #region Solicitud al api
            string body = JsonConvert.SerializeObject(filters, jsonSerializerSettings);
            StringContent content = new StringContent(body, Encoding.UTF8, "application/json");

            HttpResponseMessage Response = await clients.PostAsync(url, content);
            #endregion

            #region Deserializa response
            if (Response.StatusCode.Equals(HttpStatusCode.OK))
            {
                string jsonstring = await Response.Content.ReadAsStringAsync();

                ResponseDteList responseDteList = JsonConvert.DeserializeObject<ResponseDteList>(jsonstring);



                //if (Object.last_page > Object.current_page)
                //{
                //    do
                //    {
                //        Object.current_page++;
                //        Models.OF.Page page = new Models.OF.Page();
                //        page.eq = Object.current_page.ToString();

                //        filters.Page = page;

                //        body = JsonConvert.SerializeObject(filters);
                //        content = new StringContent(body, Encoding.UTF8, "application/json");
                //        Response = await clients.PostAsync(url, content);
                //        if (Response.StatusCode.Equals(HttpStatusCode.OK))
                //        {
                //            jsonstring = await Response.Content.ReadAsStringAsync();

                //            Object = JsonConvert.DeserializeObject<ResponseDteList>(jsonstring);

                //            ListData.Add(Object.Data);
                //        }

                //    } while (Object.current_page == Object.last_page);
                //}

                Respt = new Response
                {
                    Status = 200,
                    Message = $"Datos Obtenidos",
                    Object = responseDteList,
                    Success = true
                };

            }
            else if (Response.StatusCode.Equals(HttpStatusCode.Unauthorized))
            {
                Respt = new Response
                {
                    Status = 401,
                    Message = $"Acceso denegado a OpenFactura, es probable que la api suministrada este incorrepta o su suscripción esta suspendida, contacte al soporte de haulmer.",
                    Success = false
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
            #endregion

            return Respt;
        }
    }

    public enum Type
    {
        pdf,
        xml,
        json
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
