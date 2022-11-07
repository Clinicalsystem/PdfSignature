using PdfSignature.Modelos;
using PdfSignature.Modelos.Autentication;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PdfSignature.Data
{
    public interface IDataAccess
    {
        Task<response> Insert<T>(T model);
        Task<response> Insert<T>(List<T> models);

        Task<response> Update<T>(T model);
        Task<response> Update<T>(List<T> models);
        Task<response> GetDocument(int id);
        Task<response> GetDocument(string FbID);

        Task<response> GetDocumentList();

        Task<response> GetSignatureList();

        Task<response> Delete<T>(T model);

        Task<response> DeleteDataUSer(string localId);

    }
}
