using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace NeuroshimaDB.Services
{
    public class DangerToColorValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return
                (string)value == "Brak niebezpieczeństw (przynajmniej tych z podręcznika)." ?
                Color.FromHex("#a4ffcf") :
                Color.FromHex("#ffa4d4");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}