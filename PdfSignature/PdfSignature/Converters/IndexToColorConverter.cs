using Syncfusion.ListView.XForms;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace PdfSignature.Converters
{
    public class IndexToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var listview = parameter as SfListView;
            var index = listview.DataSource.DisplayItems.IndexOf(value);

            if (index % 2 == 0)
                return Color.White;
            return Color.FromHex("E7E6E6");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
