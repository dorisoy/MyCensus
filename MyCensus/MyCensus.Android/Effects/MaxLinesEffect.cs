using MyCensus.Droid.Effects;
using MyCensus.Effects;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Android.Widget;
using System.ComponentModel;

[assembly: ExportEffect(typeof(MaxLinesEffect), "MaxLinesEffect")]
namespace MyCensus.Droid.Effects
{
    public class MaxLinesEffect : PlatformEffect
    {
        TextView _control;

        protected override void OnAttached()
        {
            _control = Control as TextView;
            SetMaxLines();
        }

        protected override void OnDetached()
        {
        }

        protected override void OnElementPropertyChanged(PropertyChangedEventArgs args)
        {
            if (args.PropertyName == NumberOfLinesEffect.NumberOfLinesProperty.PropertyName)
            {
                SetMaxLines();
            }
        }

        private void SetMaxLines()
        {
            var maxLines = NumberOfLinesEffect.GetNumberOfLines(Element);
            _control?.SetMaxLines(maxLines);
        }
    }
}