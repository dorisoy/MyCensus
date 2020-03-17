using Android.Graphics;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;

using MyCensus.Droid.Effects;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportEffect(typeof(ContentPageTitleFontEffect), "ContentPageTitleFontEffect")]
namespace MyCensus.Droid.Effects
{
    public class ContentPageTitleFontEffect : PlatformEffect
    {
        private const string FontFamilyTitleName = "Montserrat-Regular.ttf";

        //  private readonly Context context;
        //public CustomNavigationPageRenderer(Context context) : base(context)
        //{
        //    this.context = context;
        //}
        protected override void OnAttached()
        {
            var typeface = Android.Graphics.Typeface.CreateFromAsset(Forms.Context.Assets, FontFamilyTitleName);

            var context = (Android.App.Activity)Forms.Context;
            var toolbar = context.Resources.GetIdentifier("toolbar", "id", context.PackageName);
            var view = context.FindViewById(toolbar);

            SetTypefaceToAllTextViews(view, typeface);
        }

        private void SetTypefaceToAllTextViews(Android.Views.View view, Typeface typeface)
        {
            if (view is ViewGroup)
            {
                var g = view as ViewGroup;
                for (var i = 0; i < g.ChildCount; i++)
                {
                    SetTypefaceToAllTextViews(g.GetChildAt(i), typeface);
                }
            }
            else if (view is TextView)
            {
                var tv = view as TextView;
                tv.Typeface = typeface;
            }
        }

        protected override void OnDetached()
        {
        }
    }
}