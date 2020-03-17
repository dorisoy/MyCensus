using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

using System.Linq;
using System.Linq.Expressions;

namespace MyCensus.Controls
{

    /// <summary>
    /// 自定义Input
    /// </summary>
    public class BinEntry : Entry
    {

        //public event EventHandler Clicked;

        public static readonly BindableProperty UnderlineColorProperty = BindableProperty.Create(nameof(UnderlineColor),
            typeof(Xamarin.Forms.Color), typeof(BinPicker), Xamarin.Forms.Color.Transparent, BindingMode.TwoWay);

        public Xamarin.Forms.Color UnderlineColor
        {
            get { return (Xamarin.Forms.Color)this.GetValue(UnderlineColorProperty); }
            set { this.SetValue(UnderlineColorProperty, value); }
        }


        //protected override void OnPropertyChanged(string propertyName = null)
        //{
        //    try
        //    {
        //        base.OnPropertyChanged(propertyName);

        //        this.Clicked -= Handle_Clicked;
        //        this.button.Clicked += Handle_Clicked;
        //    }
        //    catch (Exception ex)
        //    { }
        //}



        public static readonly BindableProperty BorderColorProperty =
            BindableProperty.Create(
                nameof(BorderColor),
                typeof(Color),
                typeof(BinEntry),
                Color.Gray);

        // Gets or sets BorderColor value
        public Color BorderColor
        {
            get { return (Color)GetValue(BorderColorProperty); }
            set { SetValue(BorderColorProperty, value); }
        }


        public static readonly BindableProperty BorderWidthProperty =
               BindableProperty.Create(
                   nameof(BorderWidth),
                   typeof(int),
                   typeof(BinEntry),
                   Device.OnPlatform<int>(1, 2, 2));


        // Gets or sets BorderWidth value
        public int BorderWidth
        {
            get { return (int)GetValue(BorderWidthProperty); }
            set { SetValue(BorderWidthProperty, value); }
        }


        public static readonly BindableProperty IconPaddingProperty =
          BindableProperty.Create(nameof(IconPadding),
              typeof(bool),
              typeof(BinEntry), false);

        public bool IconPadding
        {
            get
            {
                return (bool)GetValue(IconPaddingProperty);
            }
            set
            {
                SetValue(IconPaddingProperty, value);
            }
        }


        public static readonly BindableProperty CornerRadiusProperty =
        BindableProperty.Create(
            nameof(CornerRadius),
            typeof(double),
            typeof(BinEntry),
            Device.OnPlatform<double>(6, 7, 7));


        // Gets or sets CornerRadius value
        public double CornerRadius
        {
            get { return (double)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        public static readonly BindableProperty IsCurvedCornersEnabledProperty =
            BindableProperty.Create(
                nameof(IsCurvedCornersEnabled),
                typeof(bool),
                typeof(BinEntry),
                true);

        // Gets or sets IsCurvedCornersEnabled value
        public bool IsCurvedCornersEnabled
        {
            get { return (bool)GetValue(IsCurvedCornersEnabledProperty); }
            set { SetValue(IsCurvedCornersEnabledProperty, value); }
        }
    }

}
