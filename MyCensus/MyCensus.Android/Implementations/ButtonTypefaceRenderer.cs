//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//using Android.App;
//using Android.Content;
//using Android.OS;
//using Android.Runtime;
//using Android.Views;
//using Android.Widget;

//namespace MyCensus.Droid.Implementations
//{
//    class ButtonTypefaceRenderer
//    {
//    }
//}

using System;
using Android.Widget;
using Xamarin.Forms;
using MyCensus.Droid.Implementations;
using Android.Graphics;
using System.Diagnostics;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Label), typeof(LabelTypefaceRenderer))]
[assembly: ExportRenderer(typeof(Xamarin.Forms.Button), typeof(ButtonTypefaceRenderer))]
namespace MyCensus.Droid.Implementations
{
    class FontUtils
    {
        public static void ApplyTypeface(TextView view, string fontFamily)
        {
            if (!string.IsNullOrEmpty(fontFamily))
            {
                Typeface typeFace = null;
                try
                {
                    typeFace = Typeface.CreateFromAsset(Xamarin.Forms.Forms.Context.ApplicationContext.Assets, fontFamily);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Could not load font {fontFamily}: {ex}");
                }

                if (typeFace != null)
                {
                    view.Typeface = typeFace;
                }
            }
        }
    }


    //Label
    public class LabelTypefaceRenderer : LabelRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);

            FontUtils.ApplyTypeface(Control, Element.FontFamily);
        }
    }

    public class ButtonTypefaceRenderer : ButtonRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Button> e)
        {
            base.OnElementChanged(e);

            FontUtils.ApplyTypeface(Control, Element.FontFamily);
        }

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            FontUtils.ApplyTypeface(Control, Element.FontFamily);
        }
    }
}