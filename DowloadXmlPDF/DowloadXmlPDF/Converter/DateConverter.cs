using System.Globalization;

namespace DowloadXmlPDF.Converter
{
    public class DateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string date = value as string;
            char Chr = '-';
            string[] Date = date.Split(Chr);
            return string.Format("{2}-{1}-{0}", Date);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
