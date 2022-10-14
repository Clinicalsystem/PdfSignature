using PdfSignature.Data;
using PdfSignature.Modelos;
using PdfSignature.Modelos.Files;
using PdfSignature.Services;
using Syncfusion.Pdf.Security;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace PdfSignature.ViewModels
{
    /// <summary>
    /// ViewModel for Setting page 
    /// </summary> 
    [Preserve(AllMembers = true)]
    public class PerfilUserViewModel : BaseViewModel
    {
        private IMessageService _displayAlert;
        private IDataAccess _dataAccess;
        private ObservableCollection<Signature> _listSignatures;
        private bool _isVisibleModal;
        private Command<object> _deleteSignature;
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="PerfilUserViewModel" /> class
        /// </summary>
        public PerfilUserViewModel()
        {
            _displayAlert = DependencyService.Get<IMessageService>();
            _dataAccess = DependencyService.Get<IDataAccess>();
            InitializeProperties();
        }

        #endregion

        #region Propieties

        public bool IsVisibleModal
        {
            get
            {
                return _isVisibleModal;
            }
            set
            {
                

                this.SetProperty(ref _isVisibleModal, value);
            }
        }

        public ObservableCollection<Signature> ListSignatures
        {
            get
            {
                return _listSignatures;
            }

            set
            {
                if (_listSignatures == value)
                {
                    return;
                }

                this.SetProperty(ref _listSignatures, value);
            }
        }
        #endregion

        #region Commands

        public Command<object> DeleteSignatureCommand
        {
            get { return _deleteSignature; }
            set { _deleteSignature = value; }
        }

        public Command EditProfileCommand { get; set; }

        /// <summary>
        /// Gets or sets the command is executed when the change password option is clicked.
        /// </summary>
        public Command ChangePasswordCommand { get; set; }

        /// <summary>
        /// Gets or sets the command is executed when the account link option is clicked.
        /// </summary>
        public Command NewSignatureCommand { get; set; }

        /// <summary>
        /// Gets or sets the command is executed when the help option is clicked.
        /// </summary>
        public Command HelpCommand { get; set; }

        /// <summary>
        /// Gets or sets the command is executed when the terms of service option is clicked.
        /// </summary>
        public Command TermsCommand { get; set; }

        /// <summary>
        /// Gets or sets the command is executed when the privacy policy option is clicked.
        /// </summary>
        public Command PolicyCommand { get; set; }

        /// <summary>
        /// Gets or sets the command is executed when the FAQ option is clicked.
        /// </summary>
        public Command SignatureListCommand { get; set; }

        /// <summary>
        /// Gets or sets the command is executed when the logout is clicked.
        /// </summary>
        public Command LogoutCommand { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Invoked when the edit profile option clicked
        /// </summary>
        /// <param name="obj">The object</param>
        private void EditProfileClicked(object obj)
        {
            // Do something
        }

        /// <summary>
        /// Invoked when the change password clicked
        /// </summary>
        /// <param name="obj">The object</param>
        private void ChangePasswordClicked(object obj)
        {
            // Do something
        }
        private async void InitializeProperties()
        {
            EditProfileCommand = new Command(this.EditProfileClicked);
            ChangePasswordCommand = new Command(this.ChangePasswordClicked);
            NewSignatureCommand = new Command(this.NewSignatureClicked);
            HelpCommand = new Command(this.HelpClicked);
            TermsCommand = new Command(this.TermsServiceClicked);
            PolicyCommand = new Command(this.PrivacyPolicyClicked);
            SignatureListCommand = new Command(this.SignatureListClicked);
            LogoutCommand = new Command(this.LogoutClicked);
            DeleteSignatureCommand = new Command<object>(this.DeleteSignatureClicked);
            var listCert = await ApiServiceFireBase.GetSignatureList();
            if (listCert.Success)
            {
                ListSignatures = new ObservableCollection<Signature>((List<Signature>)listCert.Object);
                await _dataAccess.Update((List<Signature>)listCert.Object);
                await _dataAccess.Update((List<Signature>)listCert.Object);
            }
            else
            {
                var resp = await _dataAccess.GetSignatureList();
                if (resp.Success)
                {
                    ListSignatures = new ObservableCollection<Signature>((List<Signature>)resp.Object);
                }
                else
                {
                    await _displayAlert.ShowAsync($"{resp.Message} Code: {resp.Status} \n{resp.Object}");
                }

            }

        }
        /// <summary>
        /// Invoked when the account link clicked
        /// </summary>
        /// <param name="obj">The object</param>
        private async void NewSignatureClicked(object obj)
        {
            try
            {
                #region fields
                List<Signature> listCert = new List<Signature>(ListSignatures);
                StatusMessage = "Esperando, selección de archivo.";
                Signature signatureNew = null;
                string password = string.Empty;
                Stream stream = null;
                #endregion

                #region Select Signature

                FileResult file = await FilePicker.PickAsync();

                #endregion


                if (file != null)
                {
                    #region Validate Signature
                    switch (file.ContentType)
                    {
                        case "application/x-pkcs12":
                            break;
                        default:
                            await _displayAlert.ShowAsync("El archivo seleccionado se encuentra en formato pfx o p12 validos como certificado digital");
                            return;
                    }
                    #endregion

                    #region Open Signature
                    stream = await file.OpenReadAsync();
                    var Bytes = StreamToByteArray(stream);
                    password = await _displayAlert.Info("Se requiere la contraseña de la firma electrónica.");
                    
                    PdfCertificate pdfCert = new PdfCertificate(stream, password);
                    string serial = Encoding.UTF8.GetString(pdfCert.SerialNumber);
                    if (pdfCert == null || pdfCert.IssuerName == null)
                    {
                        await _displayAlert.ShowAsync("La contraseña ingresada es invalida, verfique e intente nuevamente.");
                        return;
                    }

                    var cert = Cryptography.GetTitular(Bytes, password);
                    signatureNew = new Signature
                    {
                        DateRegister = DateTime.Now,
                        Expire = pdfCert.ValidTo,
                        Name = pdfCert.SubjectName,
                        Rut = cert.Rut,
                        CN = cert.CN,
                        Password = password,
                        Emisor = pdfCert.IssuerName,
                        Serial = pdfCert.SerialNumber,
                        Version = pdfCert.Version,
                        FileBase64 = Convert.ToBase64String(Bytes),
                        LoaclId = AppSettings.AuthenticationUser.LocalId,
                        Setting = new SignatureSetting(),

                    };
                    if(listCert.Exists(c=> c.Name == signatureNew.Name && c.Expire == signatureNew.Expire && c.Password == signatureNew.Password))
                    {
                        await _displayAlert.ShowAsync("La firma seleccionada ya se encuentra cargada en nuestro sistema.");
                        return;
                    }
                    #endregion

                    #region Save / Insert Signature
                    var resp = await ApiServiceFireBase.InsertSignature(signatureNew);
                    if (resp.Success)
                    {
                         signatureNew.FireBaselId = signatureNew.Setting.Key = ((FireBaseID)resp.Object).Id;
                         await _dataAccess.Insert(signatureNew);
                        ListSignatures.Add(signatureNew);
                        NotifyPropertyChanged("ListSignatures");
                        _displayAlert.Toast("Firma cargada exitosamente");

                    }
                    else
                    {
                        await _displayAlert.ShowAsync($"{resp.Message} Code:{resp.Status} \n{resp.Object}");
                    }

                    StatusMessage = string.Empty;
                    IsLook = false;
                    return;
                    #endregion

                }
                else
                {
                    StatusMessage = string.Empty;
                    IsLook = false;
                    return;
                }


            }
            catch (Exception Ex)
            {
                await _displayAlert.ShowAsync($"Se produjo una exsepción Code: {Ex.GetHashCode()} \n{Ex.Message}");
                StatusMessage = string.Empty;
                IsLook = false;
                return;
            }
        }

        private async void DeleteSignatureClicked(object obj)
        {
           var password = await _displayAlert.Info("Ingrese su contraseña para eliminar el certificado.");
            if(password == AppSettings.UserData.Password)
            {
                Signature signature = (Signature)(obj as Button).BindingContext;
                response resp = await ApiServiceFireBase.DeleteSignature(signature.FireBaselId);
                if(resp.Success)
                {
                    await _dataAccess.Delete(signature);
                    ListSignatures.Remove(signature);
                    NotifyPropertyChanged("ListSignatures");
                }
            }
            else if(password != null)
            {
                await _displayAlert.ShowAsync("La contraseña ingresada es invalida, verfique e intente nuevamente.");
                return;
            }

        }

        private void TermsServiceClicked(object obj)
        {
            // Do something
        }

        /// <summary>
        /// Invoked when the privacy and policy clicked
        /// </summary>
        /// <param name="obj">The object</param>
        private void PrivacyPolicyClicked(object obj)
        {
            // Do something
        }

       
        private void SignatureListClicked(object obj)
        {
            if(IsVisibleModal)
            {
                IsVisibleModal = false;
            }
            else
            {
                IsVisibleModal = true;
            }
        }
        

        /// <summary>
        /// Invoked when the help option is clicked
        /// </summary>
        /// <param name="obj">The object</param>
        private void HelpClicked(object obj)
        {
            // Do something
        }

        /// <summary>
        /// Invoked when the logout button is clicked
        /// </summary>
        /// <param name="obj">The object</param>
        private void LogoutClicked(object obj)
        {
            ApiServicesAutentication.Logout();
        }

        #endregion
    }
}
