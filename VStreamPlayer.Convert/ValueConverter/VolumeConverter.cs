using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using VStreamPlayer.Core;

namespace VStreamPlayer.Convert.ValueConverter
{
    public class VolumeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double dValue)
            {
                dValue *= 100;



                return (int) dValue;
            }
            else
            {

                return default;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return "null";
        }
    }
}
