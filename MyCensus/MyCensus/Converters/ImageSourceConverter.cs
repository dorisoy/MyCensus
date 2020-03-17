using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.Reflection;
using Xamarin.Forms;

namespace MyCensus.Converters
{

    /// <summary>
    /// 资源图片地址加载转换
    /// </summary>
    public class ImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is string))
            {
                return default(ImageSource);
            }

            var path = value.ToString();

            if ( Device.RuntimePlatform == Device.iOS || Device.RuntimePlatform == Device.Android)
            {
                return ImageSource.FromResource(path);
            }
            else
            {
                var assembly = typeof(App).GetTypeInfo().Assembly;
                return ImageSource.FromResource(path, assembly);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
