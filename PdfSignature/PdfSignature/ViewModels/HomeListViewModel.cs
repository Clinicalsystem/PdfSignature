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
using PdfSignature.Data;
using Syncfusion.SfPdfViewer.XForms;
using System.IO;
using System.Net.Http;

namespace PdfSignature.ViewModels
{
    public class HomeListViewModel : BaseViewModel
    {
        #region Fields

        private ObservableCollection<Document> _documentsFavoritos;

        private ObservableCollection<DocumentFile> _documentsRecientes;

        private Command<object> _deleteDocumentCommand;

        private IMessageService _displayAlert;

        private IDataAccess _dataAccess;

        private Command<object> _openDocumentCommand;

        private Command<object> _addFavoritsCommand;
        private Command<object> _deleteFavoritsCommand;
        //private ObservableCollection<Signature> _listSignatures;

        #endregion

        #region Constructor


        public HomeListViewModel()
        {
            _displayAlert = DependencyService.Get<IMessageService>();
            _dataAccess = DependencyService.Get<IDataAccess>();
            
            InitializeProperties();

            #region InitializeCommand
            NewDocumentCommand = new Command(NewDocumentClicked);
            DeleteDocumentCommand = new Command<object>(DeleteDocument);
            AddFavoritsCommand = new Command<object>(AddFovorits);
            DeleteFavoritsCommand = new Command<object>(DeleteFavorits);
            OpenDocumentCommand = new Command<object>(OpenDocument);
            PerfilCommand = new Command<object>(PerfilUser);
            NewDocumentCommand = new Command(NewDocumentClicked);
            #endregion
        }

        #endregion

        #region property


        /// <summary>
        /// Gets or sets the property that is bound with an entry that gets the password from user in the login page.
        /// </summary>
        public ObservableCollection<Document> DocumentsFavoritos
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

        public Command<object> AddFavoritsCommand
        {
            get { return _addFavoritsCommand; }
            set { _addFavoritsCommand = value; }
        }
        public Command<object> DeleteFavoritsCommand
        {
            get { return _deleteFavoritsCommand; }
            set { _deleteFavoritsCommand = value; }
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
            IsLook = true;
            try
            {
                DocumentFile document = (DocumentFile)(obj as Xamarin.Forms.Button).BindingContext;
                if (document != null)
                {
                    StatusMessage = "Esperando, respuesta del usuario.";
                    string[] button = new string[] { "¿Borrar Archivo Reciente?."};
                    var resp = await _displayAlert.ShowAsync(button);
                    switch(resp)
                    {
                        case "Cancelar":
                            break;
                        default:
                            StatusMessage = "Borrando archivo.";
                            var delete = await _dataAccess.Delete(document);
                            if(delete.Success)
                            {
                                DocumentsRecientes.Remove(document);
                                NotifyPropertyChanged("DocumentsRecientes");
                            }
                            break;


                    }
                }
            }
            catch (Exception ex)
            {

                await _displayAlert.ShowAsync($"Se produjo una excepción al intentar borrar el archivo. Code: {ex.GetHashCode()} \n{ex.Message}");
            }
            StatusMessage = string.Empty;
            IsLook = false;
        }

        private async void AddFovorits(object obj)
        {
            IsLook = true;
            try
            {
                DocumentFile document = (DocumentFile)(obj as Button).BindingContext;
                if (document != null)
                {
                    StatusMessage = "Esperando, respuesta del usuario.";
                    document.Date = DateTime.Now;
                    bool questoion = await _displayAlert.QuestionAsync("¿Estas seguro de agregar este documento a favoritos?");
                    
                    if(questoion)
                    {
                        StatusMessage = "Almacenado en la nube.";
                        var response = await ApiServiceFireBase.InsertDocument(document);
                        if(response.Success)
                        {
                            StatusMessage = "Agregando a Favoritos.";
                            await _dataAccess.Delete(document);
                            DocumentsRecientes.Remove(document);
                            NotifyPropertyChanged("DocumentsRecientes");
                            document.FireBaseID = ((FireBaseID)response.Object).Id;
                            DocumentsFavoritos.Add(document);
                            NotifyPropertyChanged("DocumentsFavoritos");
                        }
                        else
                        {
                            StatusMessage = "Esperando, respuesta del usuario.";
                            await _displayAlert.ShowAsync($"{response.Message} Code: {response.Status} \n{response.Object}");    
                        }
                        
                    }
                }
            }
            catch (Exception ex)
            {

                await _displayAlert.ShowAsync(ex.Message);
            }
            StatusMessage = string.Empty;
            IsLook = false;
        }

        private async void DeleteFavorits(object obj)
        {
            IsLook = true;
            try
            {
                Document document = (Document)(obj as Button).BindingContext;
                if (document != null)
                {
                    StatusMessage = "Esperando, respuesta del usuario.";
                   
                    var resp = await _displayAlert.QuestionAsync("¿Estas seguro de eliminar el archivo de Favoritos?");
                    switch (resp)
                    {
                        
                        case false:
                            StatusMessage = "Operación cancelada por el usuario.";
                            break;
                        case true:
                            StatusMessage = "Eliminando archivo de la nube.";
                            var delete = await ApiServiceFireBase.DeleteDocument(document.FireBaseID);
                            if (delete.Success)
                            {
                                StatusMessage = "Removiendo archivo de la lista Recientes.";
                                DocumentsFavoritos.Remove(document);
                                NotifyPropertyChanged("DocumentsFavoritos");
                            }
                            else
                            {
                                StatusMessage = "Esperando, respuesta del usuario.";
                                await _displayAlert.ShowAsync($"{delete.Message} Code:{delete.Status} \n{delete.Object}");
                            }
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                StatusMessage = "Esperando, respuesta del usuario.";
                await _displayAlert.ShowAsync($"Se produjo una excepción al intentar eliminar el archivo. Code: {ex.GetHashCode()} \n{ex.Message}");
            }
            StatusMessage = string.Empty;
            IsLook = false;
        }

        private async void PerfilUser(object obj)
        {
            await App.GlobalNavigation.PushAsync(new PerfilUser(), true);
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
                DocumentsRecientes = new ObservableCollection<DocumentFile>();
                await _displayAlert.ShowAsync(listDoc.Message);
            }
            
            var listFavorits = await ApiServiceFireBase.GetDocumentList();
            if(listFavorits.Success)
            {
                DocumentsFavoritos = new ObservableCollection<Document>((List<Document>)listFavorits.Object);
            }
            else
            {
                DocumentsFavoritos = new ObservableCollection<Document>();
                await _displayAlert.ShowAsync($"{listFavorits.Message} Code: {listFavorits.Status} \n{listFavorits.Object}");
            }

        }

        
        public async void OpenDocument(object obj)
        {
            IsLook = true;
            try
            {
                StatusMessage = "Cargando Archivo.";
                DocumentFile document = (DocumentFile)(obj as Syncfusion.ListView.XForms.ItemTappedEventArgs).ItemData;
                AppSettings.DocumentSelect = document;
                await App.GlobalNavigation.PushAsync(new PdfView(), true);

            }
            catch (Exception ex)
            {
                if(ex.Message.Contains("PdfSignature.Modelos.Files.DocumentFile"))
                {

                    StatusMessage = "Cargando Archivo.";
                    Document document = (Document)(obj as Syncfusion.ListView.XForms.ItemTappedEventArgs).ItemData;
                    AppSettings.DocumentSelect = new DocumentFile 
                    {
                        Date = document.Date,
                        FireBaseID = document.FireBaseID,
                        FileName = document.FileName,
                        PasswordPdf = document.PasswordPdf,
                        PdfBase64 = document.PdfBase64
                    };
                    await App.GlobalNavigation.PushAsync(new PdfView(), true);

                }
                else
                {
                    StatusMessage = "Esperando, respuesta del usuario.";
                    await _displayAlert.ShowAsync($"Se produjo una excepción al intentar abrir el archivo. Code: {ex.GetHashCode()} \n{ex.Message}");
                }
            }
            StatusMessage = string.Empty;
            IsLook = false;
        }

        private async void NewDocumentClicked(object obj)
        {
            IsLook = true;
            try
            {
                StatusMessage = "Esperando, selección de archivo.";
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
                        Path = file.FullPath.Replace("\\", $"/" )

                    };
                    stream = await file.OpenReadAsync();
                }
                else
                {
                    StatusMessage = string.Empty;
                    IsLook = false;
                    return;
                }
                //archivos recientes
               
                var bytes = StreamToByteArray(stream);
                document.PdfBase64 = Convert.ToBase64String(bytes);
                var resp = await _dataAccess.Insert(document);
                if(resp.Success)
                {
                    DocumentsRecientes.Add(document);
                }
                
               


                
                
                AppSettings.DocumentSelect = document;
                await App.GlobalNavigation.PushAsync(new PdfView(), true);

            }
            catch (Exception ex)
            {
                StatusMessage = "Esperando, respuesta del usuario.";
                await _displayAlert.ShowAsync($"Se produjo una excepción al intentar abrir el archivo. Code: {ex.GetHashCode()} \n{ex.Message}");
            }
            StatusMessage = string.Empty;
            IsLook = false;
        }



        #endregion
    }

}
