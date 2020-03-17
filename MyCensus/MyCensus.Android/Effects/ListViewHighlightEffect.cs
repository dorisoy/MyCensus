using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MyCensus.Droid.Effects;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;


[assembly: ExportEffect(typeof(ListViewHighlightEffect), nameof(ListViewHighlightEffect))]
namespace MyCensus.Droid.Effects
{
    public class ListViewHighlightEffect : PlatformEffect
    {
        protected override void OnAttached()
        {
            var listView = (Android.Widget.ListView)Control;

            listView.ChoiceMode = ChoiceMode.None;
        }

        protected override void OnDetached()
        {
        }
    }
}