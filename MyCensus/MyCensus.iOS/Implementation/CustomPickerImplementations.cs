using CoreGraphics;
using Foundation;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using UIKit;


using MyCensus.Controls;
using MyCensus.iOS.Implementation;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;


[assembly: ExportRenderer(typeof(MyCensus.Controls.BinPicker), typeof(CustomPickerRenderer))]
namespace MyCensus.iOS.Implementation
{
    public class CustomPickerRenderer : PickerRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
        {
            base.OnElementChanged(e);

            var picker = e.NewElement as BinPicker;
            if (Control == null || e.NewElement == null) return;
            Control.BackgroundColor = ColorExtensions.ToUIColor(picker.UnderlineColor);
            Control.AttributedPlaceholder = new NSAttributedString(Control.AttributedPlaceholder.Value, foregroundColor: ColorExtensions.ToUIColor(picker.TitleColor));

            if (!String.IsNullOrEmpty(picker.PickerIcon))     // Setting picker icon
                SetBackImage(picker);
        }

        private void SetBackImage(BinPicker picker)
        {
            UIImage img = UIImage.FromBundle(picker.PickerIcon);
            var imageView = new UIImageView(img)
            {
                Frame = new RectangleF(20, 0, 15, 8)
            };
            Control.RightViewMode = UITextFieldViewMode.Always;
            Control.RightView = imageView;
        }
        public UIImage ResizeUIImage(UIImage sourceImage, float widthToScale, float heightToScale)
        {
            var sourceSize = sourceImage.Size;
            var maxResizeFactor = Math.Max(widthToScale / sourceSize.Width, heightToScale / sourceSize.Height);
            if (maxResizeFactor > 1) return sourceImage;
            var width = maxResizeFactor * sourceSize.Width;
            var height = maxResizeFactor * sourceSize.Height;
            UIGraphics.BeginImageContext(new CGSize(width, height));
            sourceImage.Draw(new CGRect(0, 0, width, height));
            var resultImage = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();
            return resultImage;
        }
    }

}
