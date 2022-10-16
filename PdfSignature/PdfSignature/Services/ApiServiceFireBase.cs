using Newtonsoft.Json;
using PdfSignature.Modelos;
using PdfSignature.Modelos.Autentication;
using PdfSignature.Modelos.Files;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PdfSignature.Services
{
    public class ApiServiceFireBase
    {
        public static async Task<bool> CheckConnection()
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                return false;
            }

            bool isReachable = await CrossConnectivity.Current.IsReachable("google.com", 5000);
            if (!isReachable)
            {
                return false;
            }
            await ApiServicesAutentication.TokenRefresh();
            return true;
        }

        public static async Task<bool> RegisterUser(RegisterUser user, ResponseAuthentication Response)
        {
            try
            {

                HttpClient client = new HttpClient();
                string body = JsonConvert.SerializeObject(user);
                StringContent content = new StringContent(body, Encoding.UTF8, "application/json");
                string ApiUrl = string.Concat(AppSettings.ApiFirebase, $"Users/{Response.LocalId}.json?auth={AppSettings.AuthenticationUser.IdToken}");

                HttpResponseMessage response = await client.PutAsync(ApiUrl, content);
                if (response.StatusCode.Equals(HttpStatusCode.OK))
                {
                    
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                string t = ex.Message;
                return false;
            }

        }

        public static async Task<response> GetUser()
        {
                
            try
            {
                HttpClient client = new HttpClient();
                string apiUri = string.Concat(AppSettings.ApiFirebase, $"Users/{AppSettings.AuthenticationUser.LocalId}.json?auth={AppSettings.AuthenticationUser.IdToken}");

                HttpResponseMessage response = await client.GetAsync(apiUri);
                string jsonstring = await response.Content.ReadAsStringAsync();
                if (response.StatusCode.Equals(HttpStatusCode.OK))
                {
                    

                    if (jsonstring != "null")
                    {
                        var user = JsonConvert.DeserializeObject<RegisterUser>(jsonstring);
                        AppSettings.UserData = user;
                    }
                    return new response
                    {
                        Status = 200,
                        Success = true,
                        Object = JsonConvert.DeserializeObject<RegisterUser>(jsonstring),
                        Message = "Datos obtenidos exitosamente"

                        
                    };
                }
                else
                {
                    return new response
                    {
                        Status = (int)response.StatusCode,
                        Success = false,
                        Message = "No se pudo obtener los datos del usuario.",
                        Object = JsonConvert.DeserializeObject<object>(jsonstring),
                    };
                }
            }
            catch (Exception ex)
            {
                return new response
                {
                    Message = "Se produjo una excepción, al intentar obtener los datos.",
                    Success = false,
                    Object = ex.Message,
                    Status = ex.GetHashCode()
                };
            }

        }

        public static async Task<bool> UpdateUser(RegisterUser user)
        {

            try
            {
                HttpClient client = new HttpClient();
                string body = JsonConvert.SerializeObject(user);
                StringContent content = new StringContent(body, Encoding.UTF8, "application/json");

                string apiUri = string.Concat(AppSettings.ApiFirebase, $"Users/{AppSettings.AuthenticationUser.LocalId}.json?auth={AppSettings.AuthenticationUser.IdToken}");

                HttpResponseMessage response = await client.PutAsync(apiUri, content);

                if (response.StatusCode.Equals(HttpStatusCode.OK))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                string t = ex.Message;
                return false;
            }

        }

        #region DocumentFavorits
        public static async Task<response> InsertDocument(DocumentFile document)
        {
            await CheckConnection();
            try
            {
                HttpClient client = new HttpClient();
                string body = JsonConvert.SerializeObject(document);
                StringContent content = new StringContent(body, Encoding.UTF8, "application/json");


                string apiUri = string.Concat(AppSettings.ApiFirebase, $"DocumentFavorits/{AppSettings.AuthenticationUser.LocalId}.json?auth={AppSettings.AuthenticationUser.IdToken}");
                HttpResponseMessage response = await client.PostAsync(apiUri, content);
                string jsonString = await response.Content.ReadAsStringAsync();
                if (response.StatusCode.Equals(HttpStatusCode.OK))
                {
                    return new response 
                    { 
                        Message = "Se agrego correctamente.",
                        Success = true,
                        Object = JsonConvert.DeserializeObject<FireBaseID>(jsonString),
                        Status = (int)response.StatusCode
                    };
                }
                else
                {
                    return new response
                    {
                        Message = "No se pudo guardar el archivo.",
                        Success = false,
                        Object = JsonConvert.DeserializeObject<object>(jsonString),
                        Status = (int)response.StatusCode
                    };
                }
            }
            catch (Exception ex)
            {
                return new response
                {
                    Message = "Se produjo una excepción, al itentar guardar el archivo.",
                    Success = false,
                    Object = ex.Message,
                    Status = (int)ex.GetHashCode()
                };
            }

        }

        public static async Task<response> GetDocumentList()
        {
            await CheckConnection();
            Dictionary<string, Document> keyValuePairs = new Dictionary<string, Document>();
            List<Document> documentFiles = new List<Document>();
            try
            {
                HttpClient client = new HttpClient();
                string apiUri = string.Concat(AppSettings.ApiFirebase, $"DocumentFavorits/{AppSettings.AuthenticationUser.LocalId}.json?auth={AppSettings.AuthenticationUser.IdToken}");

                HttpResponseMessage response = await client.GetAsync(apiUri);
                string jsonString = await response.Content.ReadAsStringAsync();
                if (response.StatusCode.Equals(HttpStatusCode.OK))
                {

                    
                        keyValuePairs = JsonConvert.DeserializeObject<Dictionary<string, Document>>(jsonString);
                    if(keyValuePairs != null)
                    {
                        foreach (KeyValuePair<string, Document> item in keyValuePairs)
                        {

                            documentFiles.Add(new Document()
                            {
                                FireBaseID = item.Key,
                                FileName = item.Value.FileName,
                                Date = item.Value.Date,
                                PasswordPdf = item.Value.PasswordPdf,
                                PdfBase64 = item.Value.PdfBase64
                            });


                        }
                    }
                        
                    return new response
                    {
                        Status = (int)response.StatusCode,
                        Success = true,
                        Message = "Datos obtenidos exitosamente.",
                        Object = documentFiles
                    };
                    

                    
                }
                else
                {
                    return new response
                    {
                        Status = (int)response.StatusCode,
                        Success = false,
                        Message = "No se pudo obtener la lista de archivos recientes.",
                        Object = JsonConvert.DeserializeObject<object>(jsonString),
                    };
                }
            }
            catch (Exception ex)
            {
                return new response
                {
                    Message = "Se produjo una excepción, al intentar obtener los datos.",
                    Success = false,
                    Object = ex.Message,
                    Status = ex.GetHashCode()
                };
            }
        }

        public static async Task<response> DeleteDocument(string FireBaseId)
        {

            try
            {
                HttpClient client = new HttpClient();
                string apiUri = string.Concat(AppSettings.ApiFirebase, $"DocumentFavorits/{AppSettings.AuthenticationUser.LocalId}/{FireBaseId}.json?auth={AppSettings.AuthenticationUser.IdToken}");
                HttpResponseMessage response = await client.DeleteAsync(apiUri);
                if (response.StatusCode.Equals(HttpStatusCode.OK))
                { 
                    return new response
                    {
                        Status = (int)response.StatusCode,
                        Success = true,
                        Message = "Documento eliminado exitosamente.",
                        Object = FireBaseId
                    };
                   
                }
                else
                {
                    return new response
                    {
                        Status = (int)response.StatusCode,
                        Success = false,
                        Message = "No se pudo eliminar el documento.",
                        Object = FireBaseId
                    };
                }
            }
            catch (Exception ex)
            {
                return new response
                {
                    Status = ex.GetHashCode(),
                    Success = false,
                    Message = "Se produjo una excepción, al intentar eliminar el documento.",
                    Object = ex.Message
                };
            }
        }
        #endregion

        #region Signature

        public static async Task<response> InsertSignature(Signature signature)
        {
            try
            {
                HttpClient client = new HttpClient();
                string body = JsonConvert.SerializeObject(signature);
                StringContent content = new StringContent(body, Encoding.UTF8, "application/json");


                string apiUri = string.Concat(AppSettings.ApiFirebase, $"Signature/{AppSettings.AuthenticationUser.LocalId}.json?auth={AppSettings.AuthenticationUser.IdToken}");
                HttpResponseMessage response = await client.PostAsync(apiUri, content);
                string jsonString = await response.Content.ReadAsStringAsync();
                if (response.StatusCode.Equals(HttpStatusCode.OK))
                {
                    return new response
                    {
                        Message = "Se guardo correctamente.",
                        Success = true,
                        Object = JsonConvert.DeserializeObject<FireBaseID>(jsonString),
                        Status = (int)response.StatusCode
                    };
                }
                else
                {
                    return new response
                    {
                        Message = "No se pudo guardar el archivo.",
                        Success = false,
                        Object = JsonConvert.DeserializeObject<object>(jsonString),
                        Status = (int)response.StatusCode
                    };
                }
            }
            catch (Exception ex)
            {
                return new response
                {
                    Message = "Se produjo una excepción, al itentar guardar el archivo.",
                    Success = false,
                    Object = ex.Message,
                    Status = (int)ex.GetHashCode()
                };
            }
        }

        public static async Task<response> GetSignatureList()
        {
            await CheckConnection();
            Dictionary<string, Signature> keyValuePairs = new Dictionary<string, Signature>();
            List<Signature> signatureFiles = new List<Signature>();
            try
            {
                HttpClient client = new HttpClient();
                string apiUri = string.Concat(AppSettings.ApiFirebase, $"Signature/{AppSettings.AuthenticationUser.LocalId}.json?auth={AppSettings.AuthenticationUser.IdToken}");

                HttpResponseMessage response = await client.GetAsync(apiUri);
                string jsonString = await response.Content.ReadAsStringAsync();
                if (response.StatusCode.Equals(HttpStatusCode.OK))
                {


                    keyValuePairs = JsonConvert.DeserializeObject<Dictionary<string, Signature>>(jsonString);
                    if (keyValuePairs != null)
                    {
                        foreach (KeyValuePair<string, Signature> item in keyValuePairs)
                        {

                            var sig = new Signature()
                            {
                                FireBaselId = item.Key,
                                Name = item.Value.Name,
                                DateRegister = item.Value.DateRegister,
                                Expire = item.Value.Expire,
                                FileBase64 = item.Value.FileBase64,
                                Password = item.Value.Password,
                                LoaclId = AppSettings.AuthenticationUser.LocalId,
                                Emisor = item.Value.Emisor,
                                Serial = item.Value.Serial,
                                Setting = item.Value.Setting,
                                Version = item.Value.Version,
                            };
                            sig.Setting.Key = item.Key;
                            signatureFiles.Add(sig);


                        }
                    }

                    return new response
                    {
                        Status = (int)response.StatusCode,
                        Success = true,
                        Message = "Datos obtenidos exitosamente.",
                        Object = signatureFiles
                    };



                }
                else
                {
                    return new response
                    {
                        Status = (int)response.StatusCode,
                        Success = false,
                        Message = "No se pudo obtener la lista de archivos recientes.",
                        Object = JsonConvert.DeserializeObject<object>(jsonString),
                    };
                }
            }
            catch (Exception ex)
            {
                return new response
                {
                    Message = "Se produjo una excepción, al intentar obtener los datos.",
                    Success = false,
                    Object = ex.Message,
                    Status = ex.GetHashCode()
                };
            }
        }

        public static async Task<response> DeleteSignature(string FireBaseId)
        {

            try
            {
                HttpClient client = new HttpClient();
                string apiUri = string.Concat(AppSettings.ApiFirebase, $"Signature/{AppSettings.AuthenticationUser.LocalId}/{FireBaseId}.json?auth={AppSettings.AuthenticationUser.IdToken}");
                HttpResponseMessage response = await client.DeleteAsync(apiUri);
                if (response.StatusCode.Equals(HttpStatusCode.OK))
                {
                    return new response
                    {
                        Status = (int)response.StatusCode,
                        Success = true,
                        Message = "Certificado eliminado exitosamente.",
                        Object = FireBaseId
                    };

                }
                else
                {
                    return new response
                    {
                        Status = (int)response.StatusCode,
                        Success = false,
                        Message = "No se pudo eliminar el certificado.",
                        Object = FireBaseId
                    };
                }
            }
            catch (Exception ex)
            {
                return new response
                {
                    Status = ex.GetHashCode(),
                    Success = false,
                    Message = "Se produjo una excepción, al intentar eliminar el certificado.",
                    Object = ex.Message
                };
            }
        }

        public static async Task<response> UpdateSignature(Signature signature)
        {
            try
            {
                HttpClient client = new HttpClient();
                string body = JsonConvert.SerializeObject(signature);
                StringContent content = new StringContent(body, Encoding.UTF8, "application/json");


                string apiUri = string.Concat(AppSettings.ApiFirebase, $"Signature/{AppSettings.AuthenticationUser.LocalId}/{signature.FireBaselId}.json?auth={AppSettings.AuthenticationUser.IdToken}");
                HttpResponseMessage response = await client.PutAsync(apiUri, content);
                string jsonString = await response.Content.ReadAsStringAsync();
                if (response.StatusCode.Equals(HttpStatusCode.OK))
                {
                    return new response
                    {
                        Message = "Se guardo correctamente.",
                        Success = true,
                        Object = JsonConvert.DeserializeObject<FireBaseID>(jsonString),
                        Status = (int)response.StatusCode
                    };
                }
                else
                {
                    return new response
                    {
                        Message = "No se pudo guardar el archivo.",
                        Success = false,
                        Object = JsonConvert.DeserializeObject<object>(jsonString),
                        Status = (int)response.StatusCode
                    };
                }
            }
            catch (Exception ex)
            {
                return new response
                {
                    Message = "Se produjo una excepción, al itentar guardar el archivo.",
                    Success = false,
                    Object = ex.Message,
                    Status = (int)ex.GetHashCode()
                };
            }
        }
        #endregion
    }
    internal class FireBaseID
    {
        [JsonProperty("name")]
        public string Id { get; set; }
    }
}
