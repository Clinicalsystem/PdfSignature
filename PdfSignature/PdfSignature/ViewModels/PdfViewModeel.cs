using ImageMagick;
using PdfSignature.Data;
using PdfSignature.Implementation;
using PdfSignature.Modelos.Files;
using PdfSignature.Services;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Parsing;
using Syncfusion.Pdf.Security;
using Syncfusion.SfPdfViewer.XForms;
using Syncfusion.XForms.ProgressBar;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Shapes;
using Path = System.IO.Path;
using Point = Xamarin.Forms.Point;
using Rect = Xamarin.Forms.Rect;
using RectangleF = Syncfusion.Drawing.RectangleF;
using TextAlignment = ImageMagick.TextAlignment;

namespace PdfSignature.ViewModels
{

    public class PdfViewModeel : PdfViewerViewModel
    {
        #region Fields

        public static PdfLoadedDocument Document { get; set; }
        TextInfo _TextInfo = new CultureInfo("en-US", false).TextInfo; //CultureInfo.CurrentCulture.TextInfo.ToTitleCase
        Command<object> saveCommand;
        public Rect RectSignature { get; set; }
        private StepStatus _stepCert;
        private StepStatus _stepSave;
        private bool _isVisibleModal;
        private IMessageService _displayAlert;
        private IDataAccess _dataAccess;
        private ObservableCollection<Signature> _listSignatures;
        private RectangleGeometry _rect;
        private bool _isTouchSignature;
        private bool _isNext;
        private Signature _certSelect;
        private ImageSource _SourceImg;
        #endregion

        #region Contructor
        public PdfViewModeel()
        {
            if (_pdfDocumentStream != null)
            {
                Document = new PdfLoadedDocument(_pdfDocumentStream);
            }
            _isVisibleModal = false;
            _displayAlert = DependencyService.Get<IMessageService>();
            _dataAccess = DependencyService.Get<IDataAccess>();
            InicializePropieties();
        }

        public Rect CorrdenadasDoc { get; private set; }


        #endregion

        #region Property

        public bool IsVisibleModal
        {
            get
            {
                return _isVisibleModal;
            }
            set
            {
                _isVisibleModal = value;
                NotifyPropertyChanged("IsVisibleModal");
            }
        }

        public bool IsTouchSignature
        {
            get
            {
                return _isTouchSignature;
            }
            set
            {
                _isTouchSignature = value;
                NotifyPropertyChanged("IsTouchSignature");
            }
        }

        public bool IsNext
        {
            get
            {
                return _isNext;
            }
            set
            {
                _isNext = value;
                NotifyPropertyChanged("IsNext");
            }
        }

        public Signature CertSelect
        {
            get
            {
                return _certSelect;
            }
            set
            {
                _certSelect = value;
                NotifyPropertyChanged("CertSelect");
            }
        }
        public ImageSource SourceImg
        {
            get
            {
                return _SourceImg;
            }
            set
            {
                _SourceImg = value;
                NotifyPropertyChanged("SourceImg");
            }
        }

        public List<string> StylesSignature
        {
            get
            {
                return new List<string>() { "Texto en mayúscula", "Texto en minúscula", "Texto estilo titulo" };

            }
        }


        public StepStatus StepCertificado
        {
            get
            {
                return _stepCert;
            }
            set
            {
                _stepCert = value;
                NotifyPropertyChanged("StepCertificado");
            }
        }
        public StepStatus StepFirma
        {
            get
            {
                return _stepSave;
            }
            set
            {
                _stepSave = value;
                NotifyPropertyChanged("StepFirma");

            }
        }
        public StepStatus StepSave
        {
            get
            {
                return _stepSave;
            }
            set
            {
                _stepSave = value;
                NotifyPropertyChanged("StepSave");

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
                _listSignatures = value;
                NotifyPropertyChanged("ListSignatures");

            }
        }

        public RectangleGeometry Dimension
        {
            get
            {
                return _rect;
            }

            set
            {
                if (_rect == value)
                {
                    return;
                }
                _rect = value;
                NotifyPropertyChanged("Dimension");

            }
        }
        #endregion

        #region Command

        public Command<object> SignatureCommand { get; set; }

        public Command<object> SelectCertCommand { get; set; }

        public Command<object> ChangeSettingCommand { get; set; }

        public Command<object> SaveSettingCommand { get; set; } //SaveSettingCommand

        public Command<object> SelectImageCommand { get; set; } //SelectImageCommand
        public Command ShareCommand { get; set; }

        public Command<object> OnTouchEffectCommand { get; set; }

        public Command<object> TouchSignatureCommand { get; set; }

        public Command<object> SaveCommand
        {
            get { return saveCommand; }
            protected set { saveCommand = value; }
        }

        #endregion

        #region methods

        private void InicializePropieties()
        {
            StepCertificado = StepStatus.InProgress;
            _isNext = true;
            _certSelect = new Signature
            {
                Name = "<Nombre del propietario de la firma>",
                DateRegister = DateTime.Now,
                Expire = DateTime.Now,
                Emisor = "<Nombre del emisor de la firma>",
                Version = 0

            };
            this.SignatureCommand = new Command<object>(this.SignatureDocument);
            this.ShareCommand = new Command(this.ShareDocument);
            this.OnTouchEffectCommand = new Command<object>(OnTouchEffectAction);
            this.SaveCommand = new Command<object>(SaveDocument);
            this.TouchSignatureCommand = new Command<object>(TouchSignature);
            this.SelectCertCommand = new Command<object>(SelectCert);
            this.ChangeSettingCommand = new Command<object>(ChangeSettingSingnature);
            this.SelectImageCommand = new Command<object>(SelectImage);
            this.SaveSettingCommand = new Command<object>(SaveSetting);


        }

        private async void SaveSetting(object obj)
        {
            try
            {
                var response = await ApiServiceFireBase.UpdateSignature(CertSelect);
                if (response.Success)
                {
                    var up = _dataAccess.Update(CertSelect);
                    _displayAlert.Toast(response.Message);
                }
                else
                {
                    await _displayAlert.ShowAsync(response.Message);
                }
            }
            catch (Exception ex)
            {

                await _displayAlert.ShowAsync(ex.Message);
            }
        }

        private Stream CreateImage(SignatureSetting setting)
        {
            Stream stream = new MemoryStream();
            var color = MagickColor.FromRgb(255, 255, 255);
            color.A = 0;
            MagickImage Text = new MagickImage(color, 2000, 1500);

            var Y = 60;
            new Drawables()
                .FontPointSize(50)
                .Font("Arial")
                .StrokeColor(MagickColors.Black)
                .FillColor(MagickColors.Black)
                .TextAlignment(TextAlignment.Left)
                .Text(0, Y, MyStyleText($"FIRMADO POR: {_certSelect.Name}.", setting.StyleSignature))
                .Draw(Text);
            if (setting.IsRut)
            {
                Y += 60;
                new Drawables()
                 .FontPointSize(50)
                 .Font("Arial")
                 .StrokeColor(MagickColors.Black)
                 .FillColor(MagickColors.Black)
                 .TextAlignment(TextAlignment.Left)
                 .Text(0, Y, MyStyleText($"RUT: {_certSelect.Rut}.", setting.StyleSignature))
                 .Draw(Text);
            }
            if (setting.IsEmisor)
            {
                Y += 60;
                new Drawables()
                 .FontPointSize(50)
                 .Font("Arial")
                 .StrokeColor(MagickColors.Black)
                 .FillColor(MagickColors.Black)
                 .TextAlignment(TextAlignment.Left)
                 .Text(0, Y, MyStyleText($"FIRMA CERT POR: {_certSelect.Emisor}.", setting.StyleSignature))
                 .Draw(Text);
            }
            if (setting.IsCN)
            {
                Y += 60;
                new Drawables()
                 .FontPointSize(50)
                 .Font("Arial")
                 .StrokeColor(MagickColors.Black)
                 .FillColor(MagickColors.Black)
                 .TextAlignment(TextAlignment.Left)
                 .Text(0, Y, MyStyleText($"Nombre de Reconocimiento (DN): {_certSelect.CN}.", setting.StyleSignature))
                 .Draw(Text);
            }
            Y += 60;
            new Drawables()
                 .FontPointSize(50)
                 .Font("Arial")
                 .StrokeColor(MagickColors.Black)
                 .FillColor(MagickColors.Black)
                 .TextAlignment(TextAlignment.Left)
                 .Text(0, Y, MyStyleText($"FECHA: {_certSelect.Setting.Date}.", setting.StyleSignature))
                 .Draw(Text);

            if (setting.IsReason)
            {
                Y += 60;
                new Drawables()
                 .FontPointSize(50)
                 .Font("Arial")
                 .StrokeColor(MagickColors.Black)
                 .FillColor(MagickColors.Black)
                 .TextAlignment(TextAlignment.Left)
                 .Text(0, Y, MyStyleText($"MOTIVO: {_certSelect.Setting.MyReason.ToUpper()}", setting.StyleSignature))
                 .Draw(Text);
            }
            if (setting.IsLocation)
            {
                Y += 60;
                new Drawables()
                 .FontPointSize(50)
                 .Font("Arial")
                 .StrokeColor(MagickColors.Black)
                 .FillColor(MagickColors.Black)
                 .TextAlignment(TextAlignment.Left)
                 .Text(0, Y, MyStyleText($"UBICACIÓN: {_certSelect.Setting.Location}.", setting.StyleSignature))
                 .Draw(Text);
            }
            if (setting.IsCompany)
            {
                Y += 60;
                new Drawables()
                 .FontPointSize(50)
                 .Font("Arial")
                 .StrokeColor(MagickColors.Black)
                 .FillColor(MagickColors.Black)
                 .TextAlignment(TextAlignment.Left)
                 .Text(0, Y, MyStyleText($"ORGANIZACIÓN: {_certSelect.Setting.Company}", setting.StyleSignature))
                 .Draw(Text);
            }
            Text.Trim();
            Text.Format = MagickFormat.Png;



            using (var imageSignature = new MagickImage(color, Text.Width + Y, Text.Height))
            {



                if (setting.IsImagePersonal && !string.IsNullOrEmpty(setting.ImagePersonal) && !string.IsNullOrWhiteSpace(setting.ImagePersonal))
                {
                    using (var imagePersonal = new MagickImage(setting.ImagePersonalStream()))
                    {
                        imagePersonal.Trim();
                        imagePersonal.Resize(Y - 5, 0);

                        // imagePersonal.ColorFuzz = new Percentage(50);
                        var i = (Y - imagePersonal.Height) * 0.60;
                        imageSignature.Composite(imagePersonal, Gravity.West, CompositeOperator.Over);

                    };

                }



                if (setting.IsWaterMark && !string.IsNullOrEmpty(setting.WaterMark) && !string.IsNullOrWhiteSpace(setting.WaterMark))
                {
                    using (var waterMark = new MagickImage(setting.WaterMarkStream()))
                    {
                        waterMark.Trim();
                        waterMark.Resize(0, Y - 10);
                        waterMark.Evaluate(Channels.Alpha, EvaluateOperator.Divide, 4);
                        imageSignature.Composite(waterMark, Gravity.Center, CompositeOperator.Over);
                    };

                }

                imageSignature.Composite(Text, Gravity.East, CompositeOperator.Over);

                imageSignature.Format = MagickFormat.Png;

                imageSignature.Trim();
                imageSignature.Write(stream, MagickFormat.Png);
            }



            return stream;

        }

        private string MyStyleText(string text, StyleText styleText)
        {
            var str = _TextInfo.ToTitleCase(text.ToLower());
            switch (styleText)
            {
                case StyleText.ToUppper:
                    return _TextInfo.ToUpper(text);
                case StyleText.ToLover:
                    return _TextInfo.ToLower(text);
                case StyleText.ToTitleCase:
                    return _TextInfo.ToTitleCase(text.ToLower());
                default:
                    return text;
            }
        }

        private void ChangeSettingSingnature(object ojb)
        {
            var stream = CreateImage(CertSelect.Setting);
            SourceImg = ImageSource.FromStream(() => { return stream; });


        }

        private byte[] ToByteArray(Stream input)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                input.CopyToAsync(ms);
                return ms.ToArray();
            }
        }

        private void SelectCert(object obj)
        {
            try
            {
                Syncfusion.ListView.XForms.ItemTappedEventArgs SfListView = obj as Syncfusion.ListView.XForms.ItemTappedEventArgs;
                if (SfListView != null)
                {
                    CertSelect = (Signature)SfListView.ItemData;
                    var cert = CertSelect.Certificate();
                    StepFirma = StepStatus.InProgress;
                    var stream = CreateImage(CertSelect.Setting);
                    SourceImg = ImageSource.FromStream(() => { return stream; });


                    IsNext = false;

                }
            }

            catch (Exception ex)
            {
                _displayAlert.ShowAsync(ex.Message);

            }
        }

        private async void SelectImage(object obj)
        {
            try
            {
                #region fields
                Stream stream = null;
                #endregion

                #region Select file
                var customFileType = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
                    {
                         { DevicePlatform.iOS, new[] { ".jpg", ".png", ".heif" } },
                         { DevicePlatform.Android, new[] { ".jpg", ".png", ".heif" } },
                         { DevicePlatform.UWP, new[] { ".jpg", ".png", ".heif" } },
                    });
                var options = new PickOptions
                {
                    PickerTitle = "Seleccione una imagen para insertar en su firma",
                    FileTypes = customFileType,
                };

                FileResult file = await FilePicker.PickAsync(options);

                #endregion


                if (file != null)
                {
                    MagickFormat magickFormat = new MagickFormat();
                    #region Validate Signature
                    switch (file.ContentType)
                    {
                        case "image/png":
                            magickFormat = MagickFormat.Png;
                            break;
                        case "image/jpeg":
                            magickFormat = MagickFormat.Jpeg;
                            break;
                        default:
                            await _displayAlert.ShowAsync("El archivo seleccionado no se encuentra en formato jpg, .png ó heif validos como una imagen.");
                            return;
                    }
                    #endregion

                    #region Save 
                    stream = await file.OpenReadAsync();
                    using (var magickImage = new MagickImage(stream))
                    {
                        using (MemoryStream streamResize = new MemoryStream())
                        {
                            magickImage.Resize(560, 0);
                            magickImage.Format = magickFormat;
                            magickImage.Write(streamResize, magickFormat);
                            magickImage.Dispose();
                            if (obj.ToString().Contains("ImsgePersonal"))
                            {
                                CertSelect.Setting.ImagePersonal = Convert.ToBase64String(streamResize.ToArray());
                            }
                            else if (obj.ToString().Contains("IsWaterMark"))
                            {
                                CertSelect.Setting.WaterMark = Convert.ToBase64String(streamResize.ToArray());
                            }

                        };



                    };



                    return;
                    #endregion

                }
                else
                {

                    return;
                }


            }
            catch (Exception Ex)
            {
                await _displayAlert.ShowAsync($"Se produjo una exsepción Code: {Ex.GetHashCode()} \n{Ex.Message}");

                return;
            }


        }
        private void SaveDocument(object obj)
        {
            try
            {
                MemoryStream stream = new MemoryStream();
                Document.Save(stream);
                var document = AppSettings.DocumentSelect;
                string _Path = $"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}/PdfSignature";
                string name = $"{document.FileName.Remove(document.FileName.Length - 4)}_Firmado.pdf";

                _Path = Path.Combine(_Path, name);
                if (!Directory.Exists(_Path))
                {
                    Directory.CreateDirectory(_Path);
                }

                File.WriteAllBytes(_Path, stream.GetBuffer());

            }
            catch (Exception)
            {

                throw;
            }
        }

        private void ShareDocument(object obj)
        {
            try
            {
                MemoryStream stream = new MemoryStream();
                Document.Save(stream);
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void TouchSignature(object obj)
        {
            string value = obj.ToString().ToLower();
            if (value.Contains("false"))
            {
                SettingSignature(Dimension.Rect);
                IsTouchSignature = false;
                Dimension = new RectangleGeometry();
                return;
            }
            IsTouchSignature = true;
        }

        private async void SettingSignature(Rect rect)
        {
            CorrdenadasDoc = new Rect(rect.Location, rect.Size);
            var resp = await _dataAccess.GetSignatureList();
            if (resp.Success)
            {
                ListSignatures = new ObservableCollection<Signature>((List<Signature>)resp.Object);
            }
            else
            {
                await _displayAlert.ShowAsync($"{resp.Message} Code: {resp.Status} \n{resp.Object}");
            }
            IsVisibleModal = true;
        }

        private async void SignatureDocument(object pdfViewer)
        {
            try
            {
                var pdf = pdfViewer as SfPdfViewer;

                var pageNumber = pdf.PageNumber;

                Point clientPoint = new Point((int)RectSignature.Location.X, (int)RectSignature.Location.Y);
                var x2 = RectSignature.Location.X + RectSignature.Size.Width;
                var y2 = RectSignature.Location.Y + RectSignature.Size.Height;
                Point clientPoint2 = new Point((int)x2 , (int)y2);
                Point pagePoint = pdf.ConvertClientPointToPagePoint(clientPoint, pageNumber);
                Point pagePoint2 = pdf.ConvertClientPointToPagePoint(clientPoint2, pageNumber);
                int width = (int)(pagePoint2.X - pagePoint.X);
                int Height = (int)(pagePoint2.Y - pagePoint.Y);

                PdfLoadedPage page = Document.Pages[pageNumber - 1] as PdfLoadedPage;

                PdfGraphics graphics = page.Graphics;

                PdfCertificate pdfCert = CertSelect.Certificate();
                //Creates a signature field

                RectangleF rectangleF = new RectangleF((float)pagePoint.X, (float)pagePoint.Y, width, Height);

                //PdfBitmap image = new PdfBitmap(GetImageSignature());
                PdfStandardFont font = new PdfStandardFont(PdfFontFamily.Helvetica, 15);


                Syncfusion.Pdf.Security.PdfSignature signature = new Syncfusion.Pdf.Security.PdfSignature(Document, Document.Pages[pageNumber - 1], pdfCert, "PdfSignature");

                //Set bounds to the signature.
                signature.Bounds = rectangleF;

                //Load image from file.
                PdfImage image = PdfImage.FromStream(CreateImage(CertSelect.Setting));
                //Create a font to draw text.

                // signature.Appearance.Normal.Graphics.DrawImage(image, rectangleF);
                graphics.DrawImage(image, rectangleF);

                //Saves and closes the document

                string _Path = string.Empty;
                Stream stream = new MemoryStream();
                Document.Save(stream);

                var documentSelect = AppSettings.DocumentSelect;
                // string _Path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "PdfSignature");
                string name = $"{documentSelect.FileName.Remove(documentSelect.FileName.Length - 4)}_Firmado.pdf";
                if (Device.RuntimePlatform == Device.Android)
                {
                    var status = await CheckAndRequestStorageWrite();
                    switch (status)
                    {
                        case PermissionStatus.Granted:
                            _Path = await DependencyService.Get<IFileManager>().Save(stream as MemoryStream, name);
                            break;

                    }
                }
                else if (Device.RuntimePlatform == Device.iOS)
                {

                }
                else
                {
                    //byte[] data = ReadFully(stream.BaseStream);
                    _Path = await DependencyService.Get<IFileManager>().Save(stream as MemoryStream, name);
                }
                AppSettings.PdfSavePath = _Path;
                await _displayAlert.ShowAsync($"Se guardo el archivo correctamente en la ruta: {_Path}");


            }
            catch (Exception)
            {

                throw;
            }
        }

        private async Task<PermissionStatus> CheckAndRequestStorageWrite()
        {
            var status = await Permissions.CheckStatusAsync<Permissions.StorageWrite>();

            if (status == PermissionStatus.Granted)
                return status;

            if (status == PermissionStatus.Denied && Device.RuntimePlatform == Device.Android)
            {
                await _displayAlert.ShowAsync("El Usuario denego el permiso para guardar archivos en el dispostivo.");
                return status;
            }

            if (Permissions.ShouldShowRationale<Permissions.StorageWrite>())
            {
                await _displayAlert.ShowAsync("Se requiere su permiso para almacenar los archivos firmados en el dispositivo, por favor conceda el permiso.");
            }

            status = await Permissions.RequestAsync<Permissions.StorageWrite>();

            return status;
        }


        #endregion


        private void OnTouchEffectAction(object args)
        {
            Rect rect = (Rect)args;

            Dimension = new RectangleGeometry { Rect = rect };

        }
    }


}
