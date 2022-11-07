using PdfSignature.Modelos;
using PdfSignature.Services;
using PdfSignature.Validators;
using PdfSignature.Validators.Rules;
using Plugin.Fingerprint.Abstractions;
using Plugin.Fingerprint;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace PdfSignature.ViewModels
{
    /// <summary>
    /// ViewModel for login page.
    /// </summary>
    [Preserve(AllMembers = true)]
    public class LoginViewModel : BaseViewModel
    {
        #region Fields

        private ValidatableObject<string> email;

        private bool _isRemember;
        private bool _isHuella;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance for the <see cref="LoginViewModel" /> class.
        /// </summary>
        public LoginViewModel()
        {
            this.InitializeProperties();
            this.AddValidationRules();
        }

        #endregion

        #region Property

        /// <summary>
        /// Gets or sets the property that bounds with an entry that gets the email ID from user in the login page.
        /// </summary>
        public ValidatableObject<string> Email
        {
            get
            {
                return this.email;
            }

            set
            {
                if (this.email == value)
                {
                    return;
                }

                this.SetProperty(ref this.email, value);
                
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
                if (_isHuella == value)
                {
                    return;
                }

                this.SetProperty(ref _isHuella, value);

            }
        }
        public bool IsRemember
        {
            get
            {
                
                return _isRemember;
            }
            set
            {
                if (_isRemember != value)
                {
                    
                    Preferences.Set("IsRemember", value);
                   // _isRemember = value;
                    SetProperty(ref this._isRemember, value);
                }
                    
            }
        }
        #endregion

        #region Methods

        /// <summary>
        /// This method to validate the email
        /// </summary>
        /// <returns>returns bool value</returns>
        public bool IsEmailFieldValid()
        {
            bool isEmailValid = this.Email.Validate();
            return isEmailValid;
        }

        /// <summary>
        /// Initializing the properties.
        /// </summary>
        private void InitializeProperties()
        {
            this.Email = new ValidatableObject<string>();
            this._isRemember = Preferences.Get("IsRemember", false);
            IsHuella = AppSettings.IsHuella;
            if(IsHuella)
            {
                this.Email.Value = AppSettings.UserData.Email;
            }
        }

        /// <summary>
        /// This method contains the validation rules
        /// </summary>
        private void AddValidationRules()
        {
            this.Email.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Email es Requerido" });
            this.Email.Validations.Add(new IsValidEmailRule<string> { ValidationMessage = "Email Invalido" });
        }

        

        #endregion
    }
}
