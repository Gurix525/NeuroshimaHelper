using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace NeuroshimaDB.Services
{
    public class SizeToColorValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value switch
            {
                0 => Color.FromHex("#fff0f8"),
                1 => Color.FromHex("#f0f8ff"),
                _ => Color.FromHex("#f8fff0")
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}