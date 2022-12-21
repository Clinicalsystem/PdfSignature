﻿using MauiApp1.Models;
using MauiApp1.Models.OF;

namespace MauiApp1.Services
{
    public interface IOpenFactura
    {
        Task<Response> GetDocument(int Folio, int dte, Type type);

        Task<Response> GetEmitidos(Filters filters);

        Task<Response> GetRecibidos(Filters filters);

        Task<Response> GetEcommerceData(string ApiKey);
    }
}
