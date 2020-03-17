using FFImageLoading.Transformations;
using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace MyCensus.Controls
{
    public partial class ShopImageControl
    {
        public static readonly BindableProperty ProfileImageProperty =
            BindableProperty.Create("ProfileImage", typeof(ImageSource), typeof(ShopImageControl), null);

        //StreamImageSource
        public ImageSource ProfileImage
        {
            get { return (ImageSource)GetValue(ProfileImageProperty); }
            set { SetValue(ProfileImageProperty, value); }
        }

        public static readonly BindableProperty UpdatePhotoCommandProperty =
            BindableProperty.Create("UpdatePhotoCommand", typeof(ICommand), typeof(ShopImageControl), null);

        public ICommand UpdatePhotoCommand
        {
            get { return (ICommand)GetValue(UpdatePhotoCommandProperty); }
            set { SetValue(UpdatePhotoCommandProperty, value); }
        }

        public static readonly BindableProperty BorderColorProperty =
           BindableProperty.Create("BorderColor", typeof(string), typeof(ShopImageControl), propertyChanged: OnBorderColorChanged);

        private static void OnBorderColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ShopImageControl control = bindable as ShopImageControl;
            control?.SetImageBorder(newValue as string);
        }

        public string BorderColor
        {
            get { return (string)GetValue(BorderColorProperty); }
            set { SetValue(BorderColorProperty, value); }
        }

        public ShopImageControl()
        {
            InitializeComponent();
        }

        private void SetImageBorder(string borderColor)
        {
            var transformation = new CircleTransformation
            {
                BorderHexColor = borderColor
            };

            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    transformation.BorderSize = 30;
                    break;
                default:
                    transformation.BorderSize = 20;
                    break;
            }

            Photo.Transformations.Add(transformation);
        }
    }
}
