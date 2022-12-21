using System.Globalization;

namespace DowloadXmlPDF.Converter
{
    public class DteConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string formattedString;
            switch (value)
            {
                case 33:
                    formattedString = string.Format("{0} - Factura", value);
                    break;
                case 34:
                    formattedString = string.Format("{0} - Factura Excenta", value);
                    break;
                case 40:
                    formattedString = string.Format("{0} - Liquidación de Factura", value);
                    break;
                case 39:
                    formattedString = string.Format("{0} - Boleta", value);
                    break;
                case 41:
                    formattedString = string.Format("{0} - Boleta Excenta", value);
                    break;
                case 46:
                    formattedString = string.Format("{0} - Factura de Compra", value);
                    break;
                case 52:
                    formattedString = string.Format("{0} - Guía de Despacho", value);
                    break;
                case 56:
                    formattedString = string.Format("{0} - Nota de Débito", value);
                    break;
                case 61:
                    formattedString = string.Format("{0} - Nota de Crédito", value);
                    break;
                case 110:
                    formattedString = string.Format("{0} - Factura de Exportación", value);
                    break;
                default:
                    formattedString = string.Format("{0}", value);
                    break;
            }
            return formattedString;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var formattedString = value.ToString().Remove(2);
            return formattedString;
        }
    }
}
