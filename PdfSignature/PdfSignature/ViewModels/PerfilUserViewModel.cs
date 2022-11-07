using PdfSignature.Data;
using PdfSignature.Modelos;
using PdfSignature.Modelos.Files;
using PdfSignature.Services;
using PdfSignature.Views.Perfil;
using Plugin.Fingerprint;
using Plugin.Fingerprint.Abstractions;
using Syncfusion.Pdf.Security;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Credentials;
using Windows.Security.Credentials.UI;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.PlatformConfiguration;

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
        private bool _isHuella;
        private bool _isOn;
        private Command<object> devicePageCommand;
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
        public bool IsHuella
        {
            get
            {
                return _isHuella;
            }
            set
            {
                this.SetProperty(ref _isHuella, value);
            }
        }

        public bool IsOn
        {
            get
            {
                return AppSettings.IsHuella;
            }
            set
            {
                AppSettings.IsHuella = value;   
                this.SetProperty(ref _isOn, value);
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
        public Command<object> DevicePageCommand
        {
            get
            {
                return devicePageCommand ?? (devicePageCommand = new Command<object>(DevicePage));
            }
        }
        public Command EditProfileCommand { get; set; }

        public Command<object> ActiveHuellaCommand { get; set; }
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
        private async void DevicePage(object obj)
        {
            await App.GlobalNavigation.PushAsync(new LinkedDevices(), true);
        }

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
            #region command
            EditProfileCommand = new Command(this.EditProfileClicked);
            ChangePasswordCommand = new Command(this.ChangePasswordClicked);
            NewSignatureCommand = new Command(this.NewSignatureClicked);
            HelpCommand = new Command(this.HelpClicked);
            TermsCommand = new Command(this.TermsServiceClicked);
            PolicyCommand = new Command(this.PrivacyPolicyClicked);
            SignatureListCommand = new Command(this.SignatureListClicked);
            LogoutCommand = new Command(this.LogoutClicked);
            DeleteSignatureCommand = new Command<object>(this.DeleteSignatureClicked);
            ActiveHuellaCommand = new Command<object>(this.ActiveHuella);
            #endregion

            #region List Cert
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
            #endregion

            CheckFingerprint();


        }

        private async void CheckFingerprint()
        {

            if (Device.RuntimePlatform == Device.iOS || Device.RuntimePlatform == Device.Android)
            {
                IsHuella = await CrossFingerprint.Current.IsAvailableAsync(true);

            }
        }
        private async void ActiveHuella(object obj)
        {
            try
            {
                string password = await _displayAlert.Info("Ingrese contraseña de acceso.");
                if (!string.IsNullOrEmpty(password) && password == AppSettings.UserData.Password)
                {

                 #region activar o desactivar huella
                  bool activar = (bool)obj;
                  if (activar)
                  {
                    response aut = await IsAutentic("Se reuiere autenticar su huella para activar esta funcionabilidad.");

                    if (aut.Success)
                    {
                        IsOn = activar;
                           
                        _displayAlert.Toast("Se activo de manera satisfactoria el acceso con huella");
                    }
                    else
                    {
                            IsOn = !IsOn;
                            await _displayAlert.ShowAsync($"No se logro reconocer su huella, no esta configurada en su dispositivo.");
                    }
                  }
                  else
                  {
                        IsOn = activar;
                    _displayAlert.Toast("Se desactivo de manera satisfactoria el acceso con huella");
                  }
                    #endregion

                }
                else
                {
                    IsOn = !IsOn;
                    _displayAlert.Toast("Contraseña invalida");
                }

            }
            catch (Exception ex)
            {

                await _displayAlert.ShowAsync($"Se produjo una excepción Code: {ex.GetHashCode()} \n{ex.Message}"); 
            }
           

        }
        private async Task<response> IsAutentic(string message)
        {
            AuthenticationRequestConfiguration authRequestConfig = new AuthenticationRequestConfiguration("PdfSignature", message);
            var auth = await CrossFingerprint.Current.AuthenticateAsync(authRequestConfig);
            if (auth.Authenticated)
            {
                return new response
                {
                    Status = 200,
                    Success = auth.Authenticated,
                    Object = auth.Status
                };
            }
            else
            {
                return new response
                {
                    Status = 400,
                    Success = auth.Authenticated,
                    Object = auth.Status,
                    Message = auth.ErrorMessage
                };
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
                await _displayAlert.ShowAsync($"Se produjo una excepción Code: {Ex.GetHashCode()} \n{Ex.Message}");
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
