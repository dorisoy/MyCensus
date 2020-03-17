using System;
using System.Globalization;
using Xamarin.Forms;

namespace MyCensus.Converters
{

    /// <summary>
    /// 为空验证判断
    /// </summary>
    public class InverseNullToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
