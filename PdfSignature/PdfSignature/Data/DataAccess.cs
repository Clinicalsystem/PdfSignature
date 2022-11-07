using PdfSignature.Modelos;
using PdfSignature.Modelos.Autentication;
using PdfSignature.Modelos.Files;
using SQLite;
using SQLiteNetExtensions.Extensions;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace PdfSignature.Data
{
    public class DataAccess : IDataAccess
    {
        private static SQLiteConnection connection;

        public DataAccess()
        {
            connection = new SQLiteConnection(Path.Combine(FileSystem.AppDataDirectory, "DataBase.db3"));

            #region CreateTables
            connection.CreateTable<DocumentFile>();
            connection.CreateTable<Signature>();
            connection.CreateTable<SignatureSetting>();
            #endregion


        }

        public Task<response> Delete<T>(T model)
        {
           response _response = new response();
           var item = connection.Delete(model);
            if(item > 0)
            {
                _response = new response()
                {
                    Status= 200,
                    Success = true,
                    Message = "El items fue eliminado.",
                    Object = item
                };
            }
            else
            {
                _response = new response()
                {
                    Status = 400,
                    Success = false,
                    Message = "El items no existe en la database.",
                    Object = item
                };
            }

            return Task.FromResult(_response);
        }
        public Task<response> Delete<T>(List<T> models)
        {
            response _response = new response();
            int count = 0;
            foreach(var itm in models)
            {
                connection.Delete(itm);
                count++;
            }
            if (count > 0)
            {
                _response = new response()
                {
                    Status = 200,
                    Success = true,
                    Message = "El items fue eliminado.",
                    Object = count
                };
            }
            else
            {
                _response = new response()
                {
                    Status = 400,
                    Success = false,
                    Message = "No se puede procesar el requerimiento.",
                    Object = count
                };
            }

            return Task.FromResult(_response);
        }

        public Task<response> GetDocument(int id)
        {
            response _response = new response();
            var item = connection.Table<DocumentFile>().FirstOrDefault(m => m.Id == id);
            if (item != null)
            {
                _response = new response()
                {
                    Status = 200,
                    Success = true,
                    Message = $"Retorna item {item.ToString()}.",
                    Object = item
                };
            }
            else
            {
                _response = new response()
                {
                    Status = 400,
                    Success = false,
                    Message = "El items no existe en la database.",
                    Object = item
                };
            }
            return Task.FromResult(_response);
        }

        public Task<response> GetDocument(string FbID)
        {
            response _response = new response();
            var item = connection.Table<DocumentFile>().FirstOrDefault(m => m.FireBaseID == FbID);
            if (item != null)
            {
                _response = new response()
                {
                    Status = 200,
                    Success = true,
                    Message = $"Retorna item {item.ToString()}.",
                    Object = item
                };
            }
            else
            {
                _response = new response()
                {
                    Status = 400,
                    Success = false,
                    Message = "El items no existe en la database.",
                    Object = item
                };
            }
            return Task.FromResult(_response);
        }

        public Task<response> GetDocumentList()
        {
            response _response = new response();
            var item = connection.GetAllWithChildren<DocumentFile>().Where(s => s.LocalId == AppSettings.AuthenticationUser.LocalId).ToList();
            if (item != null)
            {
                _response = new response()
                {
                    Status = 200,
                    Success = true,
                    Message = $"Retorna lista de items.",
                    Object = item
                };
            }
            else
            {
                _response = new response()
                {
                    Status = 400,
                    Success = false,
                    Message = "No se pudo obtener la lista de items",
                    Object = item
                };
            }

            return Task.FromResult(_response);
        }

        public Task<response> GetSignatureList()
        {
            response _response = new response();
            var item = connection.GetAllWithChildren<Signature>().Where(s => s.LoaclId ==  AppSettings.AuthenticationUser.LocalId).ToList();
            if (item != null)
            {
                
                _response = new response()
                {
                    Status = 200,
                    Success = true,
                    Message = $"Retorna lista de items.",
                    Object = item
                };
            }
            else
            {
                _response = new response()
                {
                    Status = 400,
                    Success = false,
                    Message = "No se pudo obtener la lista de items",
                    Object = item
                };
            }

            return Task.FromResult(_response);
        }
        public Task<response> Insert<T>(T model)
        {
            response _response = new response();
            
            connection.InsertWithChildren(model) ;

            _response = new response()
            {
                Status = 200,
                Success = true,
                Message = $"Se inserto el items.",
                Object = model
            };

            return Task.FromResult(_response);
        }

        public  Task<response> Insert<T>(List<T> models)
        {
            response _response = new response();
            connection.InsertOrReplaceAllWithChildren(models);
                _response = new response()
                {
                    Status = 200,
                    Success = true,
                    Message = $"Se inserto los items.",
                    Object = models
                };
           

            return Task.FromResult(_response);
        }

        public Task<response> Update<T>(T models)
        {
            response _response;
            connection.InsertOrReplaceWithChildren(models);
            _response = new response()
            {
                Status = 200,
                Success = true,
                Message = $"Se inserto los items.",
                Object = models
            };


            return Task.FromResult(_response);
        }

        public Task<response> Update<T>(List<T> models)
        {
            response _response = new response();
            connection.InsertOrReplaceAllWithChildren(models);
            _response = new response()
            {
                Status = 200,
                Success = true,
                Message = "Los items fueron actualizados.",
                Object = models
            };


            return Task.FromResult(_response);
        }
        
        public void Dispose()
        {
            connection.Dispose();
        }

        public async Task<response> DeleteDataUSer(string localId)
        {
            var itemSig = connection.GetAllWithChildren<Signature>().Where(s => s.LoaclId == localId).ToList();
            var sig = await Delete(itemSig);

            var itemDoc = connection.GetAllWithChildren<DocumentFile>().Where(s => s.LocalId == AppSettings.AuthenticationUser.LocalId).ToList();
            var doc = await Delete(itemDoc);

            if(doc.Success && sig.Success)
            {
                return new response
                {
                    Success = true,
                    Status = 200,
                    Message = "Todos los datos del usuario eliminados",
                    Object = null
                };
            }
            else if(doc.Success && !sig.Success)
            {
                return new response
                {
                    Success = true,
                    Status = 201,
                    Message = $"Se eliminaros los documentos frecuentes pero no se pudo eliminar los certificados asociados a este usuario. Error:{sig.Status} /n {sig.Message}",
                    Object = null
                };
            }
            else if (!doc.Success && sig.Success)
            {
                return new response
                {
                    Success = true,
                    Status = 202,
                    Message = $"Se eliminaros las firmas pero no se pudo eliminar los documentos frecuentes asociados a este usuario. Error:{doc.Status} \n {doc.Message}",
                    Object = null
                };
            }
            else
            {
                return new response
                {
                    Success = false,
                    Status = 400,
                    Message = $"No se pudo eliminar la información asociada a este usuario. Error: DF{doc.Status}, FE{sig.Status} \n DF: {doc.Message} \n FE: {sig.Message}",
                    Object = null
                };
            }

        }
    }
}
