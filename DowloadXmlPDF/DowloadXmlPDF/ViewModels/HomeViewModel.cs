using DowloadXmlPdf.Models.OF;
using DowloadXmlPdf.Services;
using DowloadXmlPdf.Views.Setting;
using System.Collections.ObjectModel;

namespace DowloadXmlPdf.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        #region Fields
        private IOpenFactura _openFactura;
        private IMessageService _Message;
        private ObservableCollection<Data> _dteLists;
        private int _dteTotal;
        private int _dteCargados;
        private bool _IsVisibleDowload;

        private List<Data> ListData { get; set; }
        #endregion
        public HomeViewModel(IOpenFactura openFactura, IMessageService messageService)
        {
            _openFactura = openFactura;
            _Message = messageService;
            InicialiceProperties();
            SearchCommand = new Command<object>(SearchDte);
        }


        


        #region Propieties

        public bool IsVisibleDowload
        {
            get
            {
                return _IsVisibleDowload;
            }
            set
            {
                SetProperty(ref _IsVisibleDowload, value);
            }
        }
        public ObservableCollection<Data> DteLists
        {
            get
            {
                if (_dteLists == null)
                    _dteLists = new ObservableCollection<Data>();
                return _dteLists;
            }
            set
            {
                SetProperty(ref _dteLists, value);
            }
        }
        public int DteCargados
        {
            get
            {
                return _dteCargados;
            }
            set
            {
                SetProperty(ref _dteCargados, value);
            }
        }
        public int DteTotal
        {
            get
            {
                return _dteTotal;


            }
            set
            {
                SetProperty(ref _dteTotal, value);
            }
        }
        #endregion

        #region Command
        public Command<object> SearchCommand { get; set; }

        public Command<object>DowloadCommand { get; set; }
        #endregion

        #region Method
        private async void InicialiceProperties()
        {
            FchEmis fch = new FchEmis(); fch.DateIsMayorQue(DateTime.Now.AddDays(-3));
            var resp = await _openFactura.GetEmitidos(new Filters
            {
                FchEmis = fch,
                Page = 1

            });
            if (resp.Success)
            {
                ResponseDteList respDte = (ResponseDteList)resp.Object;
                ListData = new List<Data>(respDte.Data);
                DteLists = new ObservableCollection<Data>(ListData);
                DteTotal = respDte.total;
                IsVisibleDowload = true;
                DteCargados = ListData.Count;
                switch (respDte.last_page)
                {
                    case > 1:
                        await Task.Run(async () =>
                        {

                            var filters = new Filters
                            {
                                FchEmis = fch,
                                Page = respDte.current_page++
                            };
                            ResponseDteList respDte2 = new ResponseDteList();
                            for (int i = filters.Page; i < respDte.last_page; i++)
                            {
                                filters.Page = i;
                                var respNew = await _openFactura.GetEmitidos(filters);
                                if (respNew.Success)
                                {
                                    respDte2 = (ResponseDteList)respNew.Object;

                                    ListData.AddRange(respDte2.Data);
                                    DteLists = new ObservableCollection<Data>(ListData);
                                    DteCargados = ListData.Count;
                                    DteTotal = respDte2.total;
                                }
                            }
                            IsVisibleDowload = true;
                            _Message.ToastMessage("La lista de dte se ha completado");
                        });
                        break;
                    default:
                        break;
                }

            }
            else
            {
                _Message.ToastMessage(resp.Message);
            }
            DowloadCommand = new Command<object>(DowloadDTE);

        }

        private async void DowloadDTE(object obj)
        {
            try
            {
                Services.Type Dte = Services.Type.pdf;
                if(obj != null)
                {
                    switch(obj.ToString()) 
                    {
                        case "xml":
                            Dte = Services.Type.xml;
                            break;
                        case "pdf":
                            Dte = Services.Type.pdf;
                            break;
                        case "json":
                            Dte = Services.Type.json;
                            break;
                        default:
                            return;
                    }
                }
                else
                {
                    _Message.ToastMessage("Sin parametro de descarga");
                }

                var listDowload = ListData.Where(x => x.IsSelected == true).ToList();
                foreach( var item in listDowload) 
                {
                    var response = await _openFactura.GetDocument(item.Folio, item.TipoDTE, Dte);
                    if(response.Success)
                    {
                        DTEresponse dT = (DTEresponse)response.Object;

                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void SearchDte(object value)
        {
            string search = value as string;
            if (!string.IsNullOrEmpty(search) && ListData != null)
            {
                var list = ListData.Where(d => d.ToString().ToLower().Contains(search.ToLower())).ToList();


                DteLists = new ObservableCollection<Data>(list);
            }
        }
        #endregion
    }
}
