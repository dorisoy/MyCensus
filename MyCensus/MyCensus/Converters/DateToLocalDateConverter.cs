using System;
using System.Globalization;
using Xamarin.Forms;

namespace MyCensus.Converters
{
    /// <summary>
    /// 时间转本地时间
    /// </summary>
    public class DateToLocalDateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is DateTime))
            {
                throw new InvalidOperationException("The target must be a DateTime");
            }

            var date = (DateTime)value;
            return date.ToLocalTime();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
