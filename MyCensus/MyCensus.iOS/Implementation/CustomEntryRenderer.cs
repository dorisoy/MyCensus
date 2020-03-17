using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using Xamarin.Forms.Platform.iOS;
using CoreAnimation;
using Xamarin.Forms;
using CoreGraphics;

using MyCensus.iOS.Implementation;
using MyCensus.Controls;

[assembly: ExportRenderer(typeof(BinEntry), typeof(CustomEntryRenderer))]

namespace MyCensus.iOS.Implementation
{
    class CustomEntryRenderer : EntryRenderer
    {
        private CALayer _line;

        //protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        //{
        //    base.OnElementChanged(e);
        //    _line = null;

        //    if (Control == null || e.NewElement == null)
        //        return;

        //    Control.BorderStyle = UITextBorderStyle.None;
        //    var entry = e.NewElement as CustomEntry;

        //    /* Spliting the color */
        //    string color = entry.UnderlineColor.Split('#')[1];
        //    string[] split = new string[color.Length / 2 + (color.Length % 2 == 0 ? 0 : 1)];
        //    for (int i = 0; i < split.Length; i++)
        //    {
        //        split[i] = color.Substring(i * 2, i * 2 + 2 > color.Length ? 1 : 2);
        //    }

        //    /* Creating a new frame  */
        //    _line = new CALayer
        //    {
        //        BorderColor = UIColor.FromRGB(Convert.ToInt32(split[0]), Convert.ToInt32(split[1]), Convert.ToInt32(split[2])).CGColor,
        //        BackgroundColor = UIColor.FromRGB(Convert.ToInt32(split[0]), Convert.ToInt32(split[1]), Convert.ToInt32(split[2])).CGColor,
        //        Frame = new CGRect(0, Frame.Height / 1.2, Frame.Width, 1f)
        //    };

        //    Control.Layer.AddSublayer(_line);
        //}

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                var view = (BinEntry)Element;

                Control.LeftView = new UIView(new CGRect(0f, 0f, 9f, 20f));
                Control.LeftViewMode = UITextFieldViewMode.Always;
                Control.KeyboardAppearance = UIKeyboardAppearance.Dark;
                Control.ReturnKeyType = UIReturnKeyType.Done;
                Control.Layer.CornerRadius = Convert.ToSingle(view.CornerRadius);
                Control.Layer.BorderColor = view.BorderColor.ToCGColor();
                Control.Layer.BorderWidth = view.BorderWidth;
                Control.ClipsToBounds = true;
            }
        }
    }

}
