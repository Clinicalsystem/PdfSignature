using DowloadXmlPDF.Models;
using DowloadXmlPDF.Models.OF;
using DowloadXmlPDF.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using System.Linq;

namespace DowloadXmlPDF.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        #region Fields
        private IOpenFactura _openFactura;
        private ObservableCollection<Data> _dteLists;
        private int _dteTotal;
        private int _dteCargados;
        private bool _IsVisibleDowload;

        private List<Data> ListData { get; set; }
        #endregion
        public HomeViewModel(IOpenFactura openFactura)
        {
            _openFactura = openFactura;

            InicialiceProperties();
            SearchCommand = new Command<object>(SearchDte);
        }
        

        private async void InicialiceProperties()
        {
            FchEmis fch = new FchEmis(); fch.DateIsMayorQue(DateTime.Now.AddDays(-3));
            var resp = await _openFactura.GetEmitidos(new Filters
            {
                FchEmis = fch,
                Page = 1

            }) ;
            if(resp.Success)
            {
                ResponseDteList respDte = (ResponseDteList)resp.Object;
                ListData = new List<Data>(respDte.Data);
                DteLists = new ObservableCollection<Data>(ListData);
                DteTotal = respDte.total;
                IsVisibleDowload = true;
                DteCargados = ListData.Count;
                switch (respDte.last_page)
                {
                    case >1:
                        await Task.Run(async () => {

                            var filters = new Filters
                            {
                                FchEmis = fch,
                                Page = respDte.current_page++
                            };
                            ResponseDteList respDte2 = new ResponseDteList();
                           for(int i = filters.Page; i < respDte.last_page; i++)
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
                            //DisplayAlet.Toas("La lista de dte se ha completado");
                        });
                        //_DisplayAlert.Show($"Se an encontrado la cantidad de {respDte.total} documentos se ha programado una tarea en segundo plano para solicitar todos los documentos, verifica la cantidad de documentos en lista para confirmar la carga total de la solicitud");
                    break;
                    default:
                        break;
                }
            }
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
                if( _dteLists == null )
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
        #endregion

        #region Method
        private void SearchDte(object value)
        {
            string search = value as string;
            if(!string.IsNullOrEmpty(search) && ListData != null)
            {
                var list = ListData.Where(d => d.ToString().ToLower().Contains(search.ToLower())).ToList();

                
                DteLists = new ObservableCollection<Data>(list);
            }
        }
        #endregion
    }
}
