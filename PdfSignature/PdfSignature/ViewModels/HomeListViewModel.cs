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
using PdfSignature.Data;
using Syncfusion.SfPdfViewer.XForms;

namespace PdfSignature.ViewModels
{
    public class HomeListViewModel : BaseViewModel
    {
        #region Fields

        private ObservableCollection<DocumentFile> _documentsFavoritos;

        private ObservableCollection<DocumentFile> _documentsRecientes;

        private Command<object> _deleteDocumentCommand;

        private IMessageService _displayAlert;


        private IDataAccess _dataAccess;
        private Command<object> _openDocumentCommand;

        #endregion

        #region Constructor


        public HomeListViewModel()
        {
             _displayAlert = DependencyService.Get<IMessageService>();
             _dataAccess = DependencyService.Get<IDataAccess>();
             InitializeProperties();
             NewDocumentCommand = new Command(NewDocumentClicked);
             DeleteDocumentCommand = new Command<object>(DeleteDocument);
             OpenDocumentCommand = new Command<object>(OpenDocument);
             PerfilCommand = new Command(PerfilUser);
             NewDocumentCommand = new Command(NewDocumentClicked);
             
        }

        #endregion

        #region property


        /// <summary>
        /// Gets or sets the property that is bound with an entry that gets the password from user in the login page.
        /// </summary>
        public ObservableCollection<DocumentFile> DocumentsFavoritos
        {
            get
            {
                return _documentsFavoritos;
            }

            set
            {
                if (_documentsFavoritos == value)
                {
                    return;
                }

                this.SetProperty(ref _documentsFavoritos, value);
            }
        }

        public ObservableCollection<DocumentFile> DocumentsRecientes
        {
            get
            {
                return _documentsRecientes;
            }

            set
            {
                if (_documentsRecientes == value)
                {
                    return;
                }

                this.SetProperty(ref _documentsRecientes, value);
            }
        }

        #endregion

        #region Command

        
        public Command<object> DeleteDocumentCommand
        {
            get { return _deleteDocumentCommand; }
            set { _deleteDocumentCommand = value; }
        }

        public Command<object> OpenDocumentCommand
        {
            get { return _openDocumentCommand; }
            set { _openDocumentCommand = value; }
        }


        public Command PerfilCommand { get; set; }


        public Command NewDocumentCommand { get; set; }



        #endregion

        #region methods

        private async void DeleteDocument(object obj)
        {
            try
            {
                DocumentFile document = (DocumentFile)(obj as Xamarin.Forms.Button).BindingContext;
                if (document != null)
                {
                    string[] button = new string[] { "Borrar solo del PDdfSignature.", "Borrar incluso del dispositivo." };
                    var resp = await _displayAlert.ShowAsync(button);
                    switch(resp)
                    {
                        case "Borrar solo del PDdfSignature.":
                           var delete = _dataAccess.Delete(document);
                            break;

                        case "Borrar incluso del dispositivo.":
                            var delete1 = await _dataAccess.Delete(document);
                            if(delete1.Success)
                            {
                                //Falto borrar del disco.
                                //var ss = File.Exists(document.Path);
                                //File.Delete(document.Path);
                            }
                            break;


                    }
                }
            }
            catch (Exception ex)
            {

              await  _displayAlert.ShowAsync(ex.Message);
            }
        }
        private void PerfilUser()
        {

        }

        public void OpenDocument(object obj)
        {
            try
            {
                return;
            }
            catch (Exception)
            {

                throw;
            }
        }


        private async void InitializeProperties()
        {
            var listDoc = await _dataAccess.GetDocumentList();
            if (listDoc.Success)
            {
                DocumentsRecientes = new ObservableCollection<DocumentFile>((List<DocumentFile>)listDoc.Object);
            }
            else
            {
                await _displayAlert.ShowAsync(listDoc.Message);
            }
            // var listFavorits = await ApiServiceFireBase.GetFavoritsList();

            this._documentsFavoritos = new ObservableCollection<DocumentFile>();
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
                    var resp = await _dataAccess.Insert(document);
                    if(resp.Success)
                    {
                        document.Id = (int)resp.Object;
                        DocumentsRecientes.Add(document);
                    }
                    
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
