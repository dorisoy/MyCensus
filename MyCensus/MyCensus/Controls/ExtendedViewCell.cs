using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace MyCensus.Controls
{

    /// <summary>
    /// 自定义ExtendedViewCell继承ViewCell,用于ListView的选择效果
    /// </summary>
    public class ExtendedViewCell : ViewCell
    {
        public static readonly BindableProperty SelectedBackgroundColorProperty =
            BindableProperty.Create("SelectedBackgroundColor",
                                    typeof(Color),
                                    typeof(ExtendedViewCell),
                                    Color.Default);

        public Color SelectedBackgroundColor
        {
            get { return (Color)GetValue(SelectedBackgroundColorProperty); }
            set { SetValue(SelectedBackgroundColorProperty, value); }
        }
    }
}
