using MyCensus.Models;
using MyCensus.Models.Enums;
using MyCensus.Utils;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace MyCensus.Converters
{
    /// <summary>
    ///  持续时间转换(间隔h/s/m)
    /// </summary>
    public class DurationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is int))
            {
                throw new InvalidOperationException("The target must be a int (time in seconds)");
            }
            var totalSeconds = (int)value;
            var timeSpan = TimeSpan.FromSeconds(totalSeconds);
            var result = timeSpan.Humanize(precision:2);
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
