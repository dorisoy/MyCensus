using System;
using System.Globalization;
using Xamarin.Forms;

namespace MyCensus.Converters
{
    /// <summary>
    /// 日期格式转换
    /// </summary>
    public class DateTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is DateTime))
            {
                throw new InvalidOperationException("The target must be a DateTime");
            }

            var date = (DateTime)value;
            bool converToLocal = (string)parameter == "ToLocal";

            //var result = converToLocal? date.ToLocalTime().ToString("dddd, MMMM dd"): date.ToString("dddd, MMMM dd");
            return date.ToString("yyyy/MM/dd", new System.Globalization.CultureInfo("zh-cn"));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
