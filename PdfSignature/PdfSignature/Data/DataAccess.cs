using SQLite;
using System.IO;
using Xamarin.Essentials;

namespace PdfSignature.Data
{
    public class DataAccess
    {
        private static SQLiteConnection connection;

        public DataAccess()
        {
            connection = new SQLiteConnection(Path.Combine(FileSystem.AppDataDirectory, "DataBase.db3"));




        }
    }
}
