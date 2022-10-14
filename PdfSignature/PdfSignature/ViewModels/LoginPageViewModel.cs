using PdfSignature.Modelos.Autentication;
using PdfSignature.Services;
using PdfSignature.Validators;
using PdfSignature.Validators.Rules;
using PdfSignature.Views;
using PdfSignature.Views.Home;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace PdfSignature.ViewModels
{
    /// <summary>
    /// ViewModel for login page.
    /// </summary>
    [Preserve(AllMembers = true)]
    public class LoginPageViewModel : LoginViewModel
    {
        #region Fields

        private ValidatableObject<string> password;

        private ValidatableObject<string> streamEmpty = new ValidatableObject<string>();

        private IMessageService _displayAlert;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance for the <see cref="LoginPageViewModel" /> class.
        /// </summary>
        public LoginPageViewModel()
        {
            this.InitializeProperties();
            this.AddValidationRules();
            this.LoginCommand = new Command(this.LoginClicked);
            this.SignUpCommand = new Command(this.SignUpClicked);
            this.ForgotPasswordCommand = new Command(this.ForgotPasswordClicked);
            this.SocialMediaLoginCommand = new Command(this.SocialLoggedIn);
            this._displayAlert = DependencyService.Get<IMessageService>();
            this.IsRememberCommand = new Command(this.IsRememberMet) ;
        }

        #endregion

        #region property


        /// <summary>
        /// Gets or sets the property that is bound with an entry that gets the password from user in the login page.
        /// </summary>
        public ValidatableObject<string> Password
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

        public Command IsRememberCommand { get; set; }
        /// <summary>
        /// Gets or sets the command that is executed when the Log In button is clicked.
        /// </summary>
        public Command LoginCommand { get; set; }

        /// <summary>
        /// Gets or sets the command that is executed when the Sign Up button is clicked.
        /// </summary>
        public Command SignUpCommand { get; set; }

        /// <summary>
        /// Gets or sets the command that is executed when the Forgot Password button is clicked.
        /// </summary>
        public Command ForgotPasswordCommand { get; set; }

        /// <summary>
        /// Gets or sets the command that is executed when the social media login button is clicked.
        /// </summary>
        public Command SocialMediaLoginCommand { get; set; }

        #endregion

        #region methods

        private void IsRememberMet()
        {
            if(IsRemember)
            {
                IsRemember = false;
            }
            else
            {
                IsRemember = true;
            }
        }
        /// <summary>
        /// Check the password is null or empty
        /// </summary>
        /// <returns>Returns the fields are valid or not</returns>
        public bool AreFieldsValid()
        {
            bool isEmailValid = this.Email.Validate();
            bool isPasswordValid = this.Password.Validate();
            return isEmailValid && isPasswordValid;
        }

        /// <summary>
        /// Initializing the properties.
        /// </summary>
        private void InitializeProperties()
        {
            this.Password = new ValidatableObject<string>();
        }

        /// <summary>
        /// Validation rule for password
        /// </summary>
        private void AddValidationRules()
        {
            this.Password.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "El Password es Requerido" });
        }

        /// <summary>
        /// Invoked when the Log In button is clicked.
        /// </summary>
        /// <param name="obj">The Object</param>
        private async void LoginClicked(object obj)
        {
            if (this.AreFieldsValid())
            {
               IsLook = true;
                Login user = new Login()
                {
                    email = Email.ToString(),
                    password = Password.ToString()
                };

                var response = await ApiServicesAutentication.Login(user);

                if (response.Success)
                {

                    AppSettings.AuthenticationUser = (ResponseAuthentication)response.Object;
                    await ApiServiceFireBase.GetUser();
                    IsLook = false;
                    await _displayAlert.ShowAsync(response.Message);
                    NavigationPage PdfSignaturePage = new NavigationPage(new HomeList());
                    App.GlobalNavigation = PdfSignaturePage.Navigation;
                    App.Current.MainPage = PdfSignaturePage;
                    return;
                }
                else
                {
                    if (response.Message.Contains("Contraseña"))
                    {
                        Password.IsValid = false;
                    }
                    else
                    {
                        Email.IsValid = false;
                    }
                    IsLook = false;
                    await _displayAlert.ShowAsync(response.Message);
                    return;
                }
                IsLook = false;
            }
        }

        /// <summary>
        /// Invoked when the Sign Up button is clicked.
        /// </summary>
        /// <param name="obj">The Object</param>
        private void SignUpClicked(object obj)
        {
            // Do Something
        }

        /// <summary>
        /// Invoked when the Forgot Password button is clicked.
        /// </summary>
        /// <param name="obj">The Object</param>
        private async void ForgotPasswordClicked(object obj)
        {
            await App.GlobalNavigation.PushAsync(new ForgotPasswordPage(), true);
        }

        /// <summary>
        /// Invoked when social media login button is clicked.
        /// </summary>
        /// <param name="obj">The Object</param>
        private void SocialLoggedIn(object obj)
        {
            // Do something
        }

        #endregion
    }
}