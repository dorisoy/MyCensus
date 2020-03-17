using MyCensus.Models;
using MyCensus.Models.Enums;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace MyCensus.Converters
{
    /// <summary>
    /// 生日格式转换
    /// </summary>
    public class BirthDateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is DateTime))
            {
                value = DateTime.Parse("1999-01-01");
                //throw new InvalidOperationException("The target must be a DateTime");
            }

            var date = (DateTime)value;

            var result = date.ToString("yyyy/MM/dd");
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
