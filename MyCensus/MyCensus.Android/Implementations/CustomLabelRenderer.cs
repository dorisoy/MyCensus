using Android.Content;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace MyCensus.Droid.Implementations
{
    /// <summary>
    /// 
    /// </summary>
    public class CustomLabelRenderer : LabelRenderer
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public CustomLabelRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);
        }
    }
}