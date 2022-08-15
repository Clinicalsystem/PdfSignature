using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PdfSignature.Services
{
    public class ServiceFireBase
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
    }
}
