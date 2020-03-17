using System;

using Android.Util;
using Android.Content;
using Android.Graphics.Drawables;

using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

using MyCensus.Controls;
using MyCensus.Droid.Implementations;
using Android.Graphics;


[assembly: ExportRenderer(typeof(BinEntry), typeof(CustomEntryRenderer))]

namespace MyCensus.Droid.Implementations
{
    class CustomEntryRenderer : Xamarin.Forms.Platform.Android.EntryRenderer
    {

        private readonly Context context;

        public CustomEntryRenderer(Context context) : base(context)
        {
            this.context = context;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            try
            {
                if (e.NewElement != null)
                {
                    var view = (BinEntry)Element;



                    //if (e.PropertyName == LineEntry.BorderColorProperty.PropertyName)
                    //{
                    //    Control.Background.SetColorFilter(element.BorderColor.ToAndroid(), PorterDuff.Mode.SrcAtop);
                    //}
                    //if (e.PropertyName == LineEntry.IconPaddingProperty.PropertyName)
                    //{
                    //    Control.SetPadding(Control.PaddingLeft + (element.IconPadding ? 68 : 0), Control.PaddingTop, Control.PaddingRight, Control.PaddingBottom);
                    //}


                    if (view.IsCurvedCornersEnabled)
                    {
                        // creating gradient drawable for the curved background
                        var _gradientBackground = new GradientDrawable();
                        _gradientBackground.SetOrientation(GradientDrawable.Orientation.BottomTop);
                        _gradientBackground.SetShape(ShapeType.Rectangle);
                        _gradientBackground.SetColor(view.BackgroundColor.ToAndroid());

                        // Thickness of the stroke line
                        _gradientBackground.SetStroke(view.BorderWidth, view.BorderColor.ToAndroid());

                        // Radius for the curves
                        _gradientBackground.SetCornerRadius(
                            DpToPixels(this.Context,
                                Convert.ToSingle(view.CornerRadius)));

                        // set the background of the label

                        Control.SetBackground(_gradientBackground);



                        //Set padding for the internal text from border
                       Control.SetPadding(
                           (int) DpToPixels(this.Context, Convert.ToSingle(12)),
                            Control.PaddingTop,
                            (int) DpToPixels(this.Context, Convert.ToSingle(12)),
                            Control.PaddingBottom);
                    }
                    else
                    {
                        //var _gradientBackground = new GradientDrawable();
                        //_gradientBackground.SetOrientation(GradientDrawable.Orientation.TopBottom);


                        //_gradientBackground.SetGradientCenter(0, (float)view.Height);

                        //_gradientBackground.SetShape(ShapeType.Line);
                        //_gradientBackground.SetColor(view.BackgroundColor.ToAndroid());

                        //// Thickness of the stroke line
                        //_gradientBackground.SetStroke(view.BorderWidth, view.BorderColor.ToAndroid());

                        //// Radius for the curves
                        //_gradientBackground.SetCornerRadius(
                        //    DpToPixels(this.Context,
                        //        Convert.ToSingle(view.CornerRadius)));

                        //// set the background of the label
                        //Control.SetBackground(_gradientBackground);

                        Control.Background.SetColorFilter(view.BorderColor.ToAndroid(), PorterDuff.Mode.SrcAtop);
                        //Control.SetPadding(Control.PaddingLeft + (element.IconPadding ? 68 : 0), Control.PaddingTop, Control.PaddingRight, Control.PaddingBottom);
                        //Set padding for the internal text from border
                        Control.SetPadding(
                            (int)DpToPixels(this.Context, Convert.ToSingle(12)),
                             Control.PaddingTop,
                             (int)DpToPixels(this.Context, Convert.ToSingle(12)),
                             Control.PaddingBottom);

                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        //protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        //{
        //    base.OnElementPropertyChanged(sender, e);

        //    var element = Element as LineEntry;
        //    if (e.PropertyName == LineEntry.BorderColorProperty.PropertyName)
        //    {
        //        Control.Background.SetColorFilter(element.BorderColor.ToAndroid(), PorterDuff.Mode.SrcAtop);
        //    }
        //    if (e.PropertyName == LineEntry.IconPaddingProperty.PropertyName)
        //    {
        //        Control.SetPadding(Control.PaddingLeft + (element.IconPadding ? 68 : 0), Control.PaddingTop, Control.PaddingRight, Control.PaddingBottom);
        //    }
        //}

        public static float DpToPixels(Context context, float valueInDp)
        {
            DisplayMetrics metrics = context.Resources.DisplayMetrics;
            return TypedValue.ApplyDimension(ComplexUnitType.Dip, valueInDp, metrics);
        }
    }

}