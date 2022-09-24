﻿using Newtonsoft.Json;
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
                        Message = "No se pudo eliminar el documento eliminado.",
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
                    Message = "Se produjo una excepción, al intentar eliminar el documento eliminado.",
                    Object = ex.Message
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
