using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using VStreamPlayer.Core;

namespace VStreamPlayer.Convert.ValueConverter
{
    [ValueConversion(typeof(bool), typeof(bool))]
    public class PlayAndPauseButtonConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null)
            {
                return false;
            }

            VideoInfo info = (VideoInfo) value;


            if (info.IsPlaying)
            {
                return true;
            }

            return false;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return false;
        }
    }
}
