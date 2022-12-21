using System.Globalization;

namespace DowloadXmlPDF.Converter
{
    public class HeightRequestConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double Height = 300;
            if ((double)value != -1)
                Height = (double)value - (double)int.Parse((string)parameter);

            return Height;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
