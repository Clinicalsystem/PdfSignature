using DowloadXmlPdf.Models;
using DowloadXmlPdf.Models.OF;

namespace DowloadXmlPdf.Services
{
    public interface IOpenFactura
    {
        Task<Response> GetDocument(int Folio, int dte, Type type);

        Task<Response> GetEmitidos(Filters filters);

        Task<Response> GetRecibidos(Filters filters);

        Task<Response> GetEcommerceData(string ApiKey);
    }
}
