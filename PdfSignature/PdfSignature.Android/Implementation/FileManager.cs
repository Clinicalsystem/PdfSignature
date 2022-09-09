using Xamarin.Forms;
using PdfSignature.Implementation;
using System.IO;
using Android.OS;
using System.Threading.Tasks;

[assembly: Dependency(typeof(PdfSignature.Android.Implementation.FileManager))]
namespace PdfSignature.Android.Implementation
{
    public class FileManager : IFileManager
    {
        [System.Obsolete]
        public Task<string> Save(MemoryStream fileStream, string fileName)
        {
            bool isWriteable = Environment.MediaMounted.Equals(Environment.ExternalStorageState);
            string root;
            if (isWriteable)
            {
                root = Environment.GetExternalStoragePublicDirectory(Environment.DirectoryDocuments).AbsolutePath;
                if(!Directory.Exists(root))
                {
                    Directory.CreateDirectory(root);
                }
            }
            else
            {
                root = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
            }
            
            Java.IO.File myDir = new Java.IO.File(root + "/PdfSygnature");
            myDir.Mkdir();
            Java.IO.File file = new Java.IO.File(myDir, fileName);
            string filePath = file.Path;
            //if (file.Exists()) file.Delete();
            Java.IO.FileOutputStream outs = new Java.IO.FileOutputStream(file);
            outs.Write(fileStream.ToArray());
            var ab = file.Path;
            outs.Flush();
            outs.Close();
            return Task.FromResult(filePath);
        }
    }
}