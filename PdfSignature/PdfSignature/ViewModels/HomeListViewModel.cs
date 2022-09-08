using PdfSignature.Modelos.Autentication;
using PdfSignature.Services;
using PdfSignature.Validators.Rules;
using PdfSignature.Validators;
using PdfSignature.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;
using PdfSignature.Modelos.Files;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using PdfSignature.Views.PDF;
using System.IO;

namespace PdfSignature.ViewModels
{
    public class HomeListViewModel :  BaseViewModel
    {
        #region Fields

        private ObservableCollection<DocumentFile> _documents;

        private IMessageService _displayAlert;

        #endregion

        #region Constructor

       
        public HomeListViewModel()
        {
            this.InitializeProperties();
            this.NewDocumentCommand = new Command(this.NewDocumentClicked);
            this.DeleteDocumentCommand = new Command(this.DeleteDocument);
            this.OpenDocumentCommand = new Command(this.OpenDocumentAsync);
            this.PerfilCommand = new Command(this.PerfilUser);
            this.NewDocumentCommand = new Command(this.NewDocumentClicked);
        }

        #endregion

        #region property


        /// <summary>
        /// Gets or sets the property that is bound with an entry that gets the password from user in the login page.
        /// </summary>
        public ObservableCollection<DocumentFile> Documents
        {
            get
            {
                return _documents;
            }

            set
            {
                if (_documents == value)
                {
                    return;
                }

                this.SetProperty(ref _documents, value);
            }
        }



        #endregion

        #region Command

        public Command DeleteDocumentCommand { get; set; }
        
        public Command OpenDocumentCommand { get; set; }

       
        public Command PerfilCommand { get; set; }

     
        public Command NewDocumentCommand { get; set; }

            

        #endregion

        #region methods

        private void DeleteDocument()
        {
            
        }
        private void PerfilUser()
        {

        }

        public void OpenDocumentAsync()
        {
            
        }

        
        private void InitializeProperties()
        {

            this._documents = new ObservableCollection<DocumentFile>();
        }

        private void PerfilClicked(object obj)
        {
           
        }

        private async void NewDocumentClicked(object obj)
        {
            try
            {
                DocumentFile document;
                Stream stream = null;
                PickOptions pickOptions = new PickOptions
                {
                    PickerTitle = "Seleccione Archivo Pdf",
                    FileTypes = FilePickerFileType.Pdf
                };
                FileResult file = await FilePicker.PickAsync(pickOptions);
                if (file != null)
                {
                   
                    
                    document = new DocumentFile()
                        {
                            FileName = file.FileName,
                            Date = DateTime.Now,
                            Path = file.FullPath

                        };
                    stream = await file.OpenReadAsync();
                }
                else
                {
                    return;
                }
                //archivos recientes
                using (var data = new MemoryStream())
                {
                    await stream.CopyToAsync(data);
                    document.PdfBase64 = Convert.ToBase64String(data.ToArray());
                }
                   
                AppSettings.DocumentSelect = document;
                        await App.GlobalNavigation.PushAsync(new PdfView(), true);
                
            }
            catch (System.Exception)
            {

                return;
            }
        }

       

        #endregion
    }
}
