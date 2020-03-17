using System;
using Android.App;
using MyCensus.Droid.Renderers;
using MyCensus.Views;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(HomePage), typeof(StartPageRenderer))]
namespace MyCensus.Droid.Renderers
{
    public class StartPageRenderer : PageRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Page> e)
        {
            base.OnElementChanged(e);
        }
    }
}
