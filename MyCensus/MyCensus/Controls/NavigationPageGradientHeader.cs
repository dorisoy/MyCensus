using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MyCensus.Controls
{

    /// <summary>
    /// 自定义渐变色导航栏
    /// </summary>
    public class NavigationPageGradientHeader : NavigationPage
    {
        public NavigationPageGradientHeader(Xamarin.Forms.Page root) : base(root)
        {
        }

        public static readonly BindableProperty RightColorProperty = BindableProperty.Create(propertyName: nameof(RightColor),
          returnType: typeof(Color),
          declaringType: typeof(NavigationPageGradientHeader),
          defaultValue: Color.Red);

        public static readonly BindableProperty LeftColorProperty =
           BindableProperty.Create(propertyName: nameof(LeftColor),
               returnType: typeof(Color),
               declaringType: typeof(NavigationPageGradientHeader),
               defaultValue: Color.Black);

        public Color RightColor
        {
            get { return (Color)GetValue(RightColorProperty); }
            set { SetValue(RightColorProperty, value); }
        }

        public Color LeftColor
        {
            get { return (Color)GetValue(LeftColorProperty); }
            set { SetValue(LeftColorProperty, value); }
        }
    }
}
