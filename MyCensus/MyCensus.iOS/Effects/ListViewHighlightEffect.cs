using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;

using Effects.iOS.Effects;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportEffect(typeof(ListViewHighlightEffect), nameof(ListViewHighlightEffect))]
namespace Effects.iOS.Effects
{
    public class ListViewHighlightEffect : PlatformEffect
    {
        protected override void OnAttached()
        {
            var listView = (UIKit.UITableView)Control;

            listView.AllowsSelection = false;
        }

        protected override void OnDetached()
        {
        }
    }
}