using DowloadXmlPDF.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace DowloadXmlPDF.ViewModels
{
    /// <summary>
    /// This viewmodel extends in another viewmodels.
    /// </summary>

    [DataContract]
    public class BaseViewModel : INotifyPropertyChanged
    {
        #region Fields

        private Command<object> backButtonCommand;
        private string _menssage;
        private bool _isLook;

        private static bool isCompletet { get; set; }

        #endregion

        #region Event handler

        /// <summary>
        /// Occurs when the property is changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        public BaseViewModel()
        {
            IsBusy = false;
        }
        #region Properties

        public byte[] StreamToByteArray(Stream stream)
        {
            stream.Position = 0L;
            byte[] array = new byte[stream.Length];
            stream.Read(array, 0, array.Length);
            return array;
        }
        public static bool IsCompletet
        {
            get
            {
                return isCompletet;
            }

            set
            {
                if (isCompletet == value)
                {
                    return;
                }

                isCompletet = value;
            }
        }
        /// <summary>
        /// Turns the activity indicator on or off, to lock the user's screen.
        /// </summary>
        public bool IsBusy
        {
            get
            {
                return _isLook;
            }

            set
            {
                this.SetProperty(ref this._isLook, value);
            }
        }
        /// <summary>
        /// Indicates the current message of a task status.
        /// </summary>
        public string StatusMessage
        {
            get
            {
                return _menssage;
            }

            set
            {
                this.SetProperty(ref this._menssage, value);
            }
        }

        #endregion

        #region Commands

        /// <summary>
        /// Gets the command that will be executed when an item is selected.
        /// </summary>
        public Command<object> BackButtonCommand
        {
            get
            {
                return this.backButtonCommand ?? (this.backButtonCommand = new Command<object>(this.BackButtonClicked));
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// The PropertyChanged event occurs when changing the value of property.
        /// </summary>
        /// <param name="propertyName">The PropertyName</param>
        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(storage, value))
            {
                return false;
            }

            storage = value;
            this.NotifyPropertyChanged(propertyName);

            return true;
        }

        /// <summary>
        /// Invoked when an back button is clicked.
        /// </summary>
        /// <param name="obj">The Object</param>
        private void BackButtonClicked(object obj)
        {

        }

        #endregion
    }
}
