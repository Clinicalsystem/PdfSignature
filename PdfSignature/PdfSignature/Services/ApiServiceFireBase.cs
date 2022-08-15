using Newtonsoft.Json;
using PdfSignature.Modelos.Autentication;
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
    }
}
