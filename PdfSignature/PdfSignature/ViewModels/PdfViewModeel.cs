using ImageMagick;
using PdfSignature.Data;
using PdfSignature.Implementation;
using PdfSignature.Modelos.Files;
using PdfSignature.Services;
using Syncfusion.Drawing;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Parsing;
using Syncfusion.Pdf.Security;
using Syncfusion.SfPdfViewer.XForms;
using Syncfusion.XForms.ProgressBar;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Shapes;
using Path = System.IO.Path;
using Rect = Xamarin.Forms.Rect;
using TextAlignment = ImageMagick.TextAlignment;

namespace PdfSignature.ViewModels
{

    public class PdfViewModeel : PdfViewerViewModel
    {
        #region Fields

        public static PdfLoadedDocument Document { get; set; }
        private Rect CorrdenadasDoc;
        Command<object> saveCommand;
        private StepStatus _stepCert;
        private StepStatus _stepContraseña;
        private StepStatus _stepSave;
        private bool _isVisibleModal;
        private IMessageService _displayAlert;
        private IDataAccess _dataAccess;
        private ObservableCollection<Signature> _listSignatures;
        private RectangleGeometry _rect;
        private bool _isTouchSignature;
        private bool _isNext;
        private Signature _certSelect;
        private string _sourse;
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

        public string SourseImg
        {
            get
            {
                return _sourse;
            }
            set
            {
                _sourse = value;
                NotifyPropertyChanged("SourseImg");
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

        private async void InicializePropieties()
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
            _rect = new RectangleGeometry
            {
                Rect = new Rect()
            };
            this.SignatureCommand = new Command<object>(this.SignatureDocument);
            this.ShareCommand = new Command(this.ShareDocument);
            this.OnTouchEffectCommand = new Command<object>(OnTouchEffectAction);
            this.SaveCommand = new Command<object>(SaveDocument);
            this.TouchSignatureCommand = new Command<object>(TouchSignature);
            this.SelectCertCommand = new Command<object>(SelectCert);
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
        private string CreateImage(SignatureSetting setting)
        {
            MagickImage image = new MagickImage(MagickColor.FromRgb(255, 255, 255), 2000, 1500);

            var Y = 60;
            new Drawables()
                .FontPointSize(50)
                .Font("Arial")
                .StrokeColor(MagickColors.Black)
                .FillColor(MagickColors.Black)
                .TextAlignment(TextAlignment.Left)
                .Text(300, Y, $"FIRMADO POR: {_certSelect.Name}.")
                .Draw(image);
            if (setting.IsRut)
            {
                Y += 60;
                new Drawables()
                 .FontPointSize(50)
                 .Font("Arial")
                 .StrokeColor(MagickColors.Black)
                 .FillColor(MagickColors.Black)
                 .TextAlignment(TextAlignment.Left)
                 .Text(300, Y, $"RUT: {_certSelect.Rut}.")
                 .Draw(image);
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
                 .Text(300, Y, $"FIRMA CERT POR: {_certSelect.Emisor}.")
                 .Draw(image);
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
                 .Text(300, Y, $"Nombre de Reconocimiento (DN): {_certSelect.CN}.")
                 .Draw(image);
            }
            Y += 60;
            new Drawables()
                 .FontPointSize(50)
                 .Font("Arial")
                 .StrokeColor(MagickColors.Black)
                 .FillColor(MagickColors.Black)
                 .TextAlignment(TextAlignment.Left)
                 .Text(300, Y, $"FECHA: {_certSelect.Setting.Date}.")
                 .Draw(image);

            if (setting.IsReason)
            {
                Y += 60;
                new Drawables()
                 .FontPointSize(50)
                 .Font("Arial")
                 .StrokeColor(MagickColors.Black)
                 .FillColor(MagickColors.Black)
                 .TextAlignment(TextAlignment.Left)
                 .Text(300, Y, $"MOTIVO: {_certSelect.Setting.MyReason.ToUpper()}")
                 .Draw(image);
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
                 .Text(300, Y, $"UBICACIÓN: {_certSelect.Setting.Location}.")
                 .Draw(image);
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
                 .Text(300, Y, $"ORGANIZACIÓN: {_certSelect.Setting.Company}")
                 .Draw(image);
            }
            if (setting.IsImagePersonal && !string.IsNullOrEmpty(setting.ImagePersonal) && !string.IsNullOrWhiteSpace(setting.ImagePersonal))
            {
                using (var imagePersonal = new MagickImage(setting.ImageStream))
                {
                    imagePersonal.Resize(290, 0);
                    imagePersonal.ColorFuzz = new Percentage(50);
                    var i = (Y - imagePersonal.Height) * 0.60;
                    image.Composite(imagePersonal, 0, (int)i, CompositeOperator.Over);
                };

            }



            image.Format = MagickFormat.Png;
            image.Trim();
            var path = Path.Combine(FileSystem.AppDataDirectory, $"{_certSelect.Name}.png");
            image.Write(path, MagickFormat.Png);




            return path;

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
                    SourseImg = CreateImage(CertSelect.Setting);
                    StepFirma = StepStatus.InProgress;
                    IsNext = false;

                }
            }

            catch (Exception)
            {


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

        private void SettingSignature(Rect rect)
        {
            CorrdenadasDoc = new Rect(rect.Location, rect.Size);
            IsVisibleModal = true;
        }

        private async void SignatureDocument(object pdfViewer)
        {
            try
            {
                var pdf = pdfViewer as SfPdfViewer;

                var nro = pdf.PageNumber;
                //var page = Document.Pages[nro];
                //var Size = page.Size;
                // Rectangle bounds = new Rectangle((Size.Width /2) - 100, Size.Height - 100, 200, 50);
                //Creates a new rectangle shape annotation
                // ShapeAnnotation shapeAnnotation = new ShapeAnnotation(ShapeAnnotationType.Rectangle, nro, bounds);

                //Sets the stroke color for the rectangle shape annotation 
                // shapeAnnotation.Name = "Signature";
                //  pdf.AddAnnotation(shapeAnnotation);

                //var page = Document.Pages[nro];
                //var Size = page.Size;
                ////Creates a rectangle

                //RectangleF rectangle = new RectangleF((Size.Width / 2) - 100, Size.Height - 100, 200, 50);

                //Creates a new popup annotation.


                //Gets the page

                PdfLoadedPage page = Document.Pages[nro - 1] as PdfLoadedPage;

                PdfCertificate pdfCert = ListSignatures.FirstOrDefault(x => x.LoaclId == AppSettings.AuthenticationUser.LocalId).Certificate();
                //Creates a signature field

                RectangleF rectangleF = new RectangleF((page.Size.Width / 2) - 50, page.Size.Height - 250, 200, 100);

                //PdfBitmap image = new PdfBitmap(GetImageSignature());
                PdfStandardFont font = new PdfStandardFont(PdfFontFamily.Helvetica, 15);


                Syncfusion.Pdf.Security.PdfSignature signature = new Syncfusion.Pdf.Security.PdfSignature(Document, Document.Pages[nro - 1], pdfCert, "DigitalSignature");

                signature.EnableValidationAppearance = true;
                //Set bounds to the signature.
                signature.Bounds = rectangleF;

                //Load image from file.
                PdfImage image = PdfImage.FromStream(GetImageSignature());
                //Create a font to draw text.

                signature.Appearance.Normal.Graphics.DrawImage(image, (page.Size.Width / 2) - 50, page.Size.Height - 250, 200, 100);
                signature.Appearance.Normal.Graphics.DrawString("Digitally Signed by Syncfusion", font, PdfBrushes.Black, 60, 17);
                signature.Appearance.Normal.Graphics.DrawString("Reason: Testing signature", font, PdfBrushes.Black, 60, 39);
                signature.Appearance.Normal.Graphics.DrawString("Location: USA", font, PdfBrushes.Black, 60, 60);



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


        private Stream GetImageSignature()
        {


            var bytes = Convert.FromBase64String("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");

            Stream image = new MemoryStream(bytes);

            return image;
        }
        #endregion


        private void OnTouchEffectAction(object args)
        {
            Rect rect = (Rect)args;

            Dimension = new RectangleGeometry { Rect = rect };

        }
    }
}
