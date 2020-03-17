using System;
using Android.Content;
using Android.Content.Res;
using Android.Util;
using Android.Graphics.Drawables;
using Android.Graphics;
using Android.OS;
using Android.Support.V4.Content;
using Android.Support.V4.Content.Res;
using MyCensus.Controls;
using MyCensus.Droid.Implementations;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using static Android.Resource;


[assembly: ExportRenderer(typeof(ImageEntry), typeof(ImageEntryRenderer))]
namespace MyCensus.Droid.Implementations
{
    public class ImageEntryRenderer : EntryRenderer
    {
        ImageEntry element;

        public ImageEntryRenderer(Context context) : base(context)
        {

        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null || e.NewElement == null)
                return;

            element = (ImageEntry)this.Element;


            //set border

            if (element.IsCurvedCornersEnabled)
            {
                // creating gradient drawable for the curved background
                var _gradientBackground = new GradientDrawable();
                _gradientBackground.SetShape(ShapeType.Rectangle);
                _gradientBackground.SetColor(element.BackgroundColor.ToAndroid());

                // Thickness of the stroke line
                _gradientBackground.SetStroke(element.BorderWidth, element.BorderColor.ToAndroid());

                // Radius for the curves
                _gradientBackground.SetCornerRadius(
                    DpToPixels(this.Context,
                        Convert.ToSingle(element.CornerRadius)));

                // set the background of the label
                Control.SetBackground(_gradientBackground);
            }

            // Set padding for the internal text from border
            Control.SetPadding(
                (int)DpToPixels(this.Context, Convert.ToSingle(5)),
                Control.PaddingTop,
                (int)DpToPixels(this.Context, Convert.ToSingle(5)),
                Control.PaddingBottom);

            //set image
            var editText = this.Control;
            if (!string.IsNullOrEmpty(element.Image))
            {
                switch (element.ImageAlignment)
                {
                    case ImageAlignment.Left:
                        editText.SetCompoundDrawablesWithIntrinsicBounds(GetDrawable(element.Image), null, null, null);
                        break;
                    case ImageAlignment.Right:
                        editText.SetCompoundDrawablesWithIntrinsicBounds(null, null, GetDrawable(element.Image), null);
                        break;
                }
            }
            editText.CompoundDrawablePadding = 5;
            //editText.SetMinHeight(30);
            Control.Background.SetColorFilter(element.BorderColor.ToAndroid(), PorterDuff.Mode.SrcAtop);
        }


        public static float DpToPixels(Context context, float valueInDp)
        {
            DisplayMetrics metrics = context.Resources.DisplayMetrics;
            return TypedValue.ApplyDimension(ComplexUnitType.Dip, valueInDp, metrics);
        }

        private BitmapDrawable GetDrawable(string imageEntryImage)
        {
            int resID = Resources.GetIdentifier(imageEntryImage, "drawable", this.Context.PackageName);
            if (resID == 0)
                return null;
            else
            {

                var drawable = ContextCompat.GetDrawable(this.Context, resID);
                var bitmap = ((BitmapDrawable)drawable).Bitmap;
                return new BitmapDrawable(Resources, Bitmap.CreateScaledBitmap(bitmap, element.ImageWidth * 2, element.ImageHeight * 2, true));
            }
        }

    }
}