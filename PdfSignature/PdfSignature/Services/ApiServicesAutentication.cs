using Newtonsoft.Json;
using PdfSignature.Modelos;
using PdfSignature.Modelos.Autentication;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PdfSignature.Services
{
    public class ApiServicesAutentication
    {
        public static async Task<response> Login(Login user)
        {

            try
            {

                HttpClient client = new HttpClient();
                string body = JsonConvert.SerializeObject(user);
                StringContent content = new StringContent(body, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(AppSettings.ApiAuthentication(UriApi.Loging), content);
                if (response.StatusCode.Equals(HttpStatusCode.OK))
                {
                    string jsonResult = await response.Content.ReadAsStringAsync();
                    ResponseAuthentication oResponse = JsonConvert.DeserializeObject<ResponseAuthentication>(jsonResult);

                    AppSettings.AuthenticationUser = oResponse;


                    return new response { Success = true, Message = "Login Exito", Object = oResponse, Status = 200 };
                }
                else
                {
                    string jsonResult = await response.Content.ReadAsStringAsync();
                    var oResponse = JsonConvert.DeserializeObject<RootError>(jsonResult);
                    string Message = string.Empty;
                    if (oResponse.error.message.Contains("INVALID_PASSWORD"))
                    {
                        Message = "Correo o Contraseña Inalida.";
                    }
                    else if (oResponse.error.message.Contains("EMAIL_NOT_FOUND"))
                    {
                        Message = "El correo es invalido.";
                    }
                    else
                    {
                        Message = oResponse.error.message;
                    }
                    return new response { Success = false, Message = Message, Object = oResponse, Status = 400 };
                }
            }
            catch (Exception ex)
            {
                return new response { Success = false, Message = ex.Message, Status = 401 };

            }

        }

        public static async Task<response> PasswordReset(PasswordReset passwordReset)
        {

            try
            {

                HttpClient client = new HttpClient();
                string body = JsonConvert.SerializeObject(passwordReset);
                StringContent content = new StringContent(body, Encoding.UTF8, "application/json");
                

                HttpResponseMessage response = await client.PostAsync(AppSettings.ApiAuthentication(UriApi.PasswordReset), content);
                if (response.StatusCode.Equals(HttpStatusCode.OK))
                {
                    string jsonResult = await response.Content.ReadAsStringAsync();
                    PasswordReset oResponse = JsonConvert.DeserializeObject<PasswordReset>(jsonResult);


                    return new response { Success = true, Message = "Se ha enviado un correo para restablecer su contraseña, revise su bandeja o spam.", Object = oResponse, Status = 200 };
                }
                else
                {
                    string jsonResult = await response.Content.ReadAsStringAsync();
                    var oResponse = JsonConvert.DeserializeObject<RootError>(jsonResult);
                    string Message = string.Empty;
                    if (oResponse.error.message.Contains("EMAIL_NOT_FOUND"))
                    {
                        Message = "No existe registro de usuario correspondiente a este correo. Es posible que el usuario haya sido eliminado o este mal escrito el correo.";
                    }
                    else
                    {
                        Message = oResponse.error.message;
                    }
                    return new response { Success = false, Message = Message, Object = oResponse, Status = 400 };
                }
            }
            catch (Exception ex)
            {
                return new response { Success = false, Message = ex.Message, Status = 401 };

            }

        }

        public static void Logout()
        {

            //using (DataAccess datos = new DataAccess())
            //{
            //    ResponseAuthentication user = datos.GetAutentication("CurrenUser");
            //    datos.DeleteAutentication(user);
            //}
            //AppSettings.AuthenticationUser = null;
            App.Current.MainPage = new Views.LoginPage();
        }

        public static async Task<bool> Register(RegisterUser user)
        {
            bool respuesta;
            try
            {
                Login oUser = new Login()
                {
                    email = user.Email,
                    password = user.Password
                };

                HttpClient client = new HttpClient();
                string body = JsonConvert.SerializeObject(oUser);
                StringContent content = new StringContent(body, Encoding.UTF8, "application/json");
                string ApiUrl = AppSettings.ApiAuthentication(UriApi.Sign);
                HttpResponseMessage response = await client.PostAsync(ApiUrl, content);
                if (response.StatusCode.Equals(HttpStatusCode.OK))
                {
                    string jsonResult = await response.Content.ReadAsStringAsync();
                    ResponseAuthentication responseAuthentication = JsonConvert.DeserializeObject<ResponseAuthentication>(jsonResult);
                    if (responseAuthentication != null)
                    {
                        AppSettings.AuthenticationUser = responseAuthentication;
                        respuesta = await ApiServiceFireBase.RegisterUser(user, responseAuthentication);
                    }
                    else
                    {
                        respuesta = false;
                    }
                }
                else
                {
                    string jsonResult = await response.Content.ReadAsStringAsync();
                    respuesta = false;
                }
            }
            catch (Exception ex)
            {
                string t = ex.Message;
                respuesta = false;
            }

            return respuesta;

        }

        //public static async Task<bool> TokenRefresh(ResponseAuthentication user)
        //{

        //    if (user.DateExpire.AddMilliseconds(user.ExpiresIn) > DateTime.Now)
        //    {

        //        try
        //        {
        //            TokenRefres token = new TokenRefres { RefreshToken = user.RefreshToken };

        //            HttpClient client = new HttpClient();

        //            string body = JsonConvert.SerializeObject(token);
        //            StringContent content = new StringContent(body, Encoding.UTF8, "application/json");
        //            HttpResponseMessage response = await client.PostAsync(AppSettings.ApiAuthentication(UriApi.Token), content);
        //            if (response.StatusCode.Equals(HttpStatusCode.OK))
        //            {
        //                string jsonResult = await response.Content.ReadAsStringAsync();
        //                ResponseTokenRefesh oResponse = JsonConvert.DeserializeObject<ResponseTokenRefesh>(jsonResult);
        //                using (DataAccess datos = new DataAccess())
        //                {
        //                    user.IdToken = oResponse.IdToken;
        //                    AppSettings.AuthenticationUser = user;
        //                    datos.UpdateAutentication(user);
        //                }


        //                return true;
        //            }
        //            else
        //            {
        //                return false;
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            string t = ex.Message;
        //            return false;
        //        }


        //    }
        //    else
        //    {

        //        return false;
        //    }



        //}



    }
    internal class Errors
    {
        public int code { get; set; }
        public string message { get; set; }
        public List<Details> errors { get; set; }
    }

    internal class Details
    {
        public string message { get; set; }
        public string domain { get; set; }
        public string reason { get; set; }
    }

    internal class RootError
    {
        public Errors error { get; set; }
    }
}
