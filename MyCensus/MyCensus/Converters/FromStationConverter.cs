using System;
using System.Globalization;
using Xamarin.Forms;
using static MyCensus.Controls.CustomPin;

namespace MyCensus.Converters
{

    /// <summary>
    /// 地图位置转换
    /// </summary>
    public class FromStationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is AnnotationType))
            {
                throw new InvalidOperationException("The target must be an AnnotationType");
            }

            var type = (AnnotationType)value;

            return type == AnnotationType.From;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
