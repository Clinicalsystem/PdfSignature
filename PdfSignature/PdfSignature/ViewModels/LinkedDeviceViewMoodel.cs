using PdfSignature.Data;
using PdfSignature.Modelos.Devices;
using PdfSignature.Services;
using Plugin.DeviceInfo;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace PdfSignature.ViewModels
{
    public class LinkedDeviceViewMoodel : BaseViewModel
    {
        #region Fields
        private ObservableCollection<PdfDevice> _pdfDevices;
        private IMessageService _displayAlert;
        private IDataAccess _dataAccess;

        #endregion

        #region Contrunctor
        public LinkedDeviceViewMoodel()
        {

            InitializeProperties();
        }


        #endregion

        #region Propieties
        public ObservableCollection<PdfDevice> PdfDevices
        {
            get
            {
                return _pdfDevices;
            }
            set
            {
                if (value == _pdfDevices)
                {
                    return;
                }
                this.SetProperty(ref _pdfDevices, value);
            }
        }
        #endregion

        #region Command
        public Command<object> DeleteCommand { get; set; }
        #endregion

        #region methods

        private void InitializeProperties()
        {
            PdfDevices = new ObservableCollection<PdfDevice>(AppSettings.UserData.PdfDevices);
            _displayAlert = DependencyService.Get<IMessageService>();
            _dataAccess = DependencyService.Get<IDataAccess>();

            this.DeleteCommand = new Command<object>(DeleteDevice);
        }
        private async void DeleteDevice(object obj)
        {
            try
            {
                PdfDevice deleteDevice = (PdfDevice)(obj as Button).BindingContext;
                if (deleteDevice != null)
                {
                    var user = AppSettings.UserData;
                    int ind = PdfDevices.IndexOf(deleteDevice);

                    var it = PdfDevices.Remove(deleteDevice);

                    user.PdfDevices = new List<PdfDevice>(PdfDevices);
                    var resp = await ApiServiceFireBase.UpdateUser(user);
                    AppSettings.UserData = user;
                    if (resp && deleteDevice.Id == CrossDeviceInfo.Current.Id)
                    {
                        await _dataAccess.DeleteDataUSer(user.LocalId);
                        await _displayAlert.Show("Este dispositivo fue desvinculado de esta cuenta, la sesión ha caducado.");
                        ApiServicesAutentication.Logout();
                        return;
                    }
                    else
                    {
                        PdfDevices = new ObservableCollection<PdfDevice>(user.PdfDevices);
                        _displayAlert.Toast($"El dispositivo {deleteDevice.DeviceName} fue desvinculado de esta cuenta.");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                await _displayAlert.Show($"Se produjo una excepción Code: {ex.GetHashCode()} \n{ex.Message}");
            }
        }
        #endregion
    }
}
