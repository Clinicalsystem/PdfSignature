using PdfSignature.Modelos.Autentication;
using PdfSignature.Services;
using PdfSignature.Validators;
using PdfSignature.Validators.Rules;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace PdfSignature.ViewModels
{
    /// <summary>
    /// ViewModel for sign-up page.
    /// </summary>
    [Preserve(AllMembers = true)]
    public class SignUpPageViewModel : LoginViewModel
    {
        #region Fields

        private ValidatableObject<string> name;

        private ValidatablePair<string> password;

        private ValidatableObject<string> stringEmpty;

        private IMessageService _displayAlert;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance for the <see cref="SignUpPageViewModel" /> class.
        /// </summary>
        public SignUpPageViewModel()
        {
            this.InitializeProperties();
            this.AddValidationRules();
            this.LoginCommand = new Command(this.LoginClicked);
            this.SignUpCommand = new Command(this.SignUpClicked);
            this._displayAlert = DependencyService.Get<IMessageService>();
        }
        #endregion

        #region Property

        /// <summary>
        /// Gets or sets the property that bounds with an entry that gets the name from user in the Sign Up page.
        /// </summary>
        public ValidatableObject<string> Name
        {
            get
            {
                return this.name;
            }

            set
            {
                if (this.name == value)
                {
                    return;
                }

                this.SetProperty(ref this.name, value);
            }
        }

        /// <summary>
        /// Gets or sets the property that bounds with an entry that gets the password from users in the Sign Up page.
        /// </summary>
        public ValidatablePair<string> Password
        {
            get
            {
                return this.password;
            }

            set
            {
                if (this.password == value)
                {
                    return;
                }

                this.SetProperty(ref this.password, value);
            }
        }
        #endregion

        #region Command

        /// <summary>
        /// Gets or sets the command that is executed when the Log In button is clicked.
        /// </summary>
        public Command LoginCommand { get; set; }

        /// <summary>
        /// Gets or sets the command that is executed when the Sign Up button is clicked.
        /// </summary>
        public Command SignUpCommand { get; set; }
        #endregion

        #region Methods

        /// <summary>
        /// Initialize whether fieldsvalue are true or false.
        /// </summary>
        /// <returns>true or false </returns>
        public bool AreFieldsValid()
        {
            bool isEmail = this.Email.Validate();
            bool isNameValid = this.Name.Validate();
            bool isPasswordValid = this.Password.Validate();
            return isPasswordValid && isNameValid && isEmail;
        }

        /// <summary>
        /// Initializing the properties.
        /// </summary>
        private void InitializeProperties()
        {
            this.Name = new ValidatableObject<string>();
            this.Password = new ValidatablePair<string>();
        }

        /// <summary>
        /// this method contains the validation rules
        /// </summary>
        private void AddValidationRules()
        {
            this.Name.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "El nombre es requerido" });
            this.Password.Item1.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "El password requerido" });
            this.Password.Item2.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Reingrese el password" });
        }

        /// <summary>
        /// Invoked when the Log in button is clicked.
        /// </summary>
        /// <param name="obj">The Object</param>
        private void LoginClicked(object obj)
        {
            // Do something
        }

        /// <summary>
        /// Invoked when the Sign Up button is clicked.
        /// </summary>
        /// <param name="obj">The Object</param>
        private async void SignUpClicked(object obj)
        {
            if (this.AreFieldsValid())
            {
                var User = new RegisterUser()
                {
                    Email = Email.ToString(),
                    Password = Password.Item1.ToString(),
                    UserName = Name.ToString()
                };

                if (await ApiServicesAutentication.Register(User))
                {
                    Email.Value = string.Empty;
                    Password.Item1.Value = string.Empty;
                    Password.Item2.Value = string.Empty;
                    IsCompletet = true;
                    Name.Value = string.Empty;
                    await _displayAlert.ShowAsync("Registro Completado");
                    
                    
                }
                
            }
            IsCompletet = true;
            Name.Value = string.Empty;

        }

        #endregion
    }
}