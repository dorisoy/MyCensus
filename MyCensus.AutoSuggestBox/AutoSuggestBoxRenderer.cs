using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace MyCensus.Droid.AutoSuggestBox
{
    internal class AutoSuggestBoxRenderer : ViewRenderer<AutoSuggestBox, NativeAutoSuggestBox>
    {
        public AutoSuggestBoxRenderer(Android.Content.Context context) : base(context) { }

        protected override void OnElementChanged(ElementChangedEventArgs<AutoSuggestBox> e)
        {
            base.OnElementChanged(e);
            if (Control == null)
            {
                SetNativeControl(e.NewElement?.NativeAutoSuggestBox);
            }
        }

    }
}
