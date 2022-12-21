﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp1.Converter
{
    public class SummaryConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int itms = (int)value;    
            var formattedString = string.Format("$ {0}", itms.ToString("F2"));
            return formattedString;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var formattedString = value.ToString().Remove(0,2);
            return formattedString;
        }
    }
}