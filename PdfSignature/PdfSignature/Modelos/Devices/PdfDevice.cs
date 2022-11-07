using Plugin.DeviceInfo.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace PdfSignature.Modelos.Devices
{
    public class PdfDevice
    {
        public PdfDevice()
        {

        }
        public PdfDevice(IDeviceInfo device)
        {
            AppBuild = device.AppBuild;
            AppVersion= device.AppVersion;
            DeviceName= device.DeviceName;
            Id= device.Id;
            Idiom = device.Idiom;
            IsDevice = device.IsDevice;
            Model = device.Model;
            Platform = device.Platform;
            OS = device.Platform.ToString();
            Version = device.Version;
            VersionNumber = device.VersionNumber;

        }
        public string AppBuild { get; set; }

        public string AppVersion { get; set; }

        public string DeviceName { get; set; }

        public string Id { get; set; }

        public Idiom Idiom { get; set; }

        public bool IsDevice { get; set; }

        public string Model { get; set; }

        public Platform Platform { get; set; }

        public string OS { get; set; }

        public string Version { get; set; }

        public object VersionNumber { get; set; }
    }
}
