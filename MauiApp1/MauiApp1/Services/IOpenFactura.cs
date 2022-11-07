using MauiApp1.Models;

namespace MauiApp1.Services
{
    public interface IOpenFactura
    {
        Task<Response> GetSolicitud(int Folio, int dte, TypeDTE type);

        Task<Response> GetEcommerceData();
    }
}
