using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using VStreamPlayer.Core;

namespace VStreamPlayer.Convert.ValueConverter
{
    public class PlaybackOrderValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is PlaybackOrder order)
            {
                switch (order)
                {
                    case PlaybackOrder.RandomPlay:
                        return "Random play";

                    case PlaybackOrder.SingleLoop:
                        return "Single loop";

                    case PlaybackOrder.SequentialPlay:
                        return "Sequential play";
                }

                return default;
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
