using PdfSignature.Implementation;
using PdfSignature.UWP.Implementation;
using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Controls;
using Xamarin.Forms;

[assembly: Dependency(typeof(FileManager))]
namespace PdfSignature.UWP.Implementation
{

    public class FileManager : IFileManager
    {
        MemoryStream stream;
        StorageFile file;

        public async Task<Stream> Load(string filePath, string fileName)
        {

            StorageFolder folderPDF = await StorageFolder.GetFolderFromPathAsync(filePath);
            string path = Path.Combine(filePath, fileName);
            Stream fileStream = await folderPDF.OpenStreamForReadAsync(fileName); 
           return fileStream;
            
            
        }

        public async Task<string> Save(MemoryStream fileStream, string fileName)
        {
            stream = fileStream;
            StorageFolder folderOll;
            try
            {
                folderOll = await StorageFolder.GetFolderFromPathAsync(AppSettings.PdfSavePath);
            }
            catch (Exception)
            {

                folderOll = null;
            }

            if (folderOll == null)
            {
                var folderPicker = new Windows.Storage.Pickers.FolderPicker();
                folderPicker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
                folderPicker.FileTypeFilter.Add("*");

                StorageFolder folderNew = await folderPicker.PickSingleFolderAsync();
                if (folderNew != null)
                {
                    Windows.Storage.AccessCache.StorageApplicationPermissions.
                    FutureAccessList.AddOrReplace("PickedFolderToken", folderNew);
                    return await SaveAsync(fileName, folderNew);
                }
            }
            else
            {

                return await SaveAsync(fileName, folderOll);
            }

            return string.Empty;


        }



        private async Task<string> SaveAsync(string fileName, StorageFolder folder)
        {
            #region Fields
            StorageFolder folderPDF;
            StorageFolder folderOll;
            string _path = Path.Combine(folder.Path, "PdfSingature");
            #endregion

            #region Validaciones
            try
            {
                folderOll = await StorageFolder.GetFolderFromPathAsync(_path);
            }
            catch (Exception)
            {
                folderOll = null;

            }

            if (!folder.Path.Contains("PdfSingature") && folderOll == null)
            {
                folderPDF = await folder.CreateFolderAsync("PdfSingature");
                
            }
            else if(folderOll != null)
            {
                folderPDF = folderOll;
            }
            else
            {
                folderPDF = folder;
            }
            #endregion

            #region Crea y Guarda el Archivo
            file = await folderPDF.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
            if (file != null)
            {
                Windows.Storage.Streams.IRandomAccessStream fileStream = await file.OpenAsync(FileAccessMode.ReadWrite);
                Stream st = fileStream.AsStreamForWrite();
                st.SetLength(0);
                st.Write((stream as MemoryStream).ToArray(), 0, (int)stream.Length);
                st.Flush();
                st.Dispose();
                fileStream.Dispose();
            }
            #endregion

            return folderPDF.Path;
        }
    }

}
