using System;
using System.Globalization;
using Xamarin.Forms;

namespace MyCensus.Converters
{
    /// <summary>
    /// 字符转布尔(为空判断)
    /// </summary>
    public class StringToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value == null)
            {
                return false;
            }

            string stringValue = value.ToString();

            if (string.IsNullOrEmpty(stringValue))
                return false;
            else
                return true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
