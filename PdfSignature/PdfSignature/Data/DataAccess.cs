using PdfSignature.Modelos;
using PdfSignature.Modelos.Files;
using SQLite;
using System.Collections.Generic;
using System.IO;
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
            var item = connection.Table<DocumentFile>().ToList(); ;
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
            var item = connection.Insert(model) ;
            if (item > 0)
            {
                _response = new response()
                {
                    Status = 200,
                    Success = true,
                    Message = $"Se inserto el items.",
                    Object = item
                };
            }
            else
            {
                _response = new response()
                {
                    Status = 400,
                    Success = false,
                    Message = "No se pudo insertar el items",
                    Object = item
                };
            }

            return Task.FromResult(_response);
        }

        public Task<response> Insert<T>(List<T> models)
        {
            response _response = new response();
            var item = connection.InsertAll(models);
            if (item > 0)
            {
                _response = new response()
                {
                    Status = 200,
                    Success = true,
                    Message = $"Se inserto los items.",
                    Object = item
                };
            }
            else
            {
                _response = new response()
                {
                    Status = 400,
                    Success = false,
                    Message = "No se pudo insertar los items",
                    Object = item
                };
            }

            return Task.FromResult(_response);
        }

        public Task<response> Update<T>(T model)
        {
            response _response = new response();
            var item = connection.Update(model);
            if (item > 0)
            {
                _response = new response()
                {
                    Status = 200,
                    Success = true,
                    Message = "El items fue Actualizado.",
                    Object = item
                };
            }
            else
            {
                _response = new response()
                {
                    Status = 400,
                    Success = false,
                    Message = "No se pudo actulizar el items",
                    Object = item
                };
            }

            return Task.FromResult(_response);
        }

        public Task<response> Update<T>(List<T> models)
        {
            response _response = new response();
            var item = connection.UpdateAll(models);
            if (item > 0)
            {
                _response = new response()
                {
                    Status = 200,
                    Success = true,
                    Message = "Los items fueron actualizados.",
                    Object = item
                };
            }
            else
            {
                _response = new response()
                {
                    Status = 400,
                    Success = false,
                    Message = "No se pudo actulizar los items.",
                    Object = item
                };
            }

            return Task.FromResult(_response);
        }
        public void Dispose()
        {
            connection.Dispose();
        }


    }
}
