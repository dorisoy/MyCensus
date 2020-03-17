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

using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Support.V7.Widget;
using Xamarin.Forms;


using MyCensus.Controls.ComboBox;
using MyCensus.Droid.Implementations.ComboBox;
using AColor = Android.Graphics.Color;
using BuildVersionCodes = Android.OS.BuildVersionCodes;



[assembly: ExportRenderer(typeof(ComboBox), typeof(ComboBoxRenderer))]

namespace MyCensus.Droid.Implementations.ComboBox
{

    /// <summary>
    /// 窗体微调适配器
    /// </summary>
    public class FormsSpinner : AppCompatSpinner
    {
        public FormsSpinner(Context context) : base(context)
        {
            ChangeArrowColor();
        }

        void ChangeArrowColor()
        {
            this.Background.SetColorFilter(AColor.Red, PorterDuff.Mode.SrcAtop);
            Drawable spinnerDrawable = Background.GetConstantState().NewDrawable();

            spinnerDrawable.SetColorFilter(AColor.Red, PorterDuff.Mode.SrcAtop);
            if (Build.VERSION.SdkInt >= BuildVersionCodes.JellyBean)
            {
                Background = spinnerDrawable;
            }
            else
            {
                SetBackgroundDrawable(spinnerDrawable);
            }
        }
    }
}