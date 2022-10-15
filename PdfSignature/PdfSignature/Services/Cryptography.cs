using System.IO;
using System.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Syncfusion.Pdf.Security;

namespace PdfSignature.Services
{
    public class Cryptography
    {

        public static TitularCertificate GetTitular(Stream stream, string password)
        {
            X509Certificate2 objCert = new X509Certificate2(StreamToByteArray(stream), password, X509KeyStorageFlags.UserKeySet);
            foreach (var ct in objCert.Extensions)
            {

                if (ct.Oid.FriendlyName == "Subject Alternative Name")
                {
                    return new TitularCertificate
                    {
                        Rut = Encoding.UTF8.GetString(ct.RawData, 18, 10),
                        CN = objCert.Issuer
                    };
                }
            }

            return new TitularCertificate { CN = string.Empty, Rut = string.Empty };
        }
        public static TitularCertificate GetTitular(byte[] bytes, string password)
        {
            X509Certificate2 objCert = new X509Certificate2(bytes, password, X509KeyStorageFlags.UserKeySet);
            foreach(var ct in objCert.Extensions)
            {

                if (ct.Oid.FriendlyName == "Subject Alternative Name")
                 {
                        return new TitularCertificate
                        {
                            Rut = Encoding.UTF8.GetString(ct.RawData, 18, 10),
                            CN = objCert.Issuer
                        };
                 }
            }

            return new TitularCertificate { CN= string.Empty, Rut = string.Empty };
        }
       
        public static byte[] StreamToByteArray(Stream stream)
        {
            stream.Position = 0L;
            byte[] array = new byte[stream.Length];
            stream.Read(array, 0, array.Length);
            return array;
        }

    }
    public class TitularCertificate
    {
        public string Rut { get; set; }

        public string CN { get; set; }
    }

}
