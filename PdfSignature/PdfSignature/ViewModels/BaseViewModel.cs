using PdfSignature.Validators;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace PdfSignature.ViewModels
{
    /// <summary>
    /// This viewmodel extends in another viewmodels.
    /// </summary>
    [Preserve(AllMembers = true)]
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
            IsLook = false;
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
        public bool IsLook
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
        private async void BackButtonClicked(object obj)
        {
            if (Device.RuntimePlatform == Device.UWP && Application.Current.MainPage.Navigation.NavigationStack.Count > 1)
            {
                await App.GlobalNavigation.PopToRootAsync();
            }
            else if (Device.RuntimePlatform != Device.UWP && Application.Current.MainPage.Navigation.NavigationStack.Count > 0)
            {
                await App.GlobalNavigation.PopToRootAsync();
            }
        }

        #endregion
    }
}
