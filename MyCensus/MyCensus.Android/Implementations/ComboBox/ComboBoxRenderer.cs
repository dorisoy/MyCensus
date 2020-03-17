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
using System.ComponentModel;
using Xamarin.Forms;
using Android.Graphics;
using Android.Graphics.Drawables;

using MyCensus.Controls.ComboBox;
using MyCensus.Droid.Implementations.ComboBox;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(ComboBox), typeof(ComboBoxRenderer))]
namespace MyCensus.Droid.Implementations.ComboBox
{

    /// <summary>
    /// 自定义ComboBox 渲染器
    /// </summary>
    public class ComboBoxRenderer : ViewRenderer<MyCensus.Controls.ComboBox.ComboBox, FormsSpinner>
    {
        #region Fields
        private bool _isDisposed;
        #endregion

        public ComboBoxRenderer(Context context)
            : base(context)
        {
            AutoPackage = false;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<MyCensus.Controls.ComboBox.ComboBox> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                if (Control == null)
                {

                    var spinner = CreateNativeControl();
                 
                    spinner.ItemSelected += Spinner_ItemSelected;
                    var items = e.NewElement.ItemsSource;

                    if (items == null)
                    {
                        spinner.Adapter = null;
                    }
                    else
                    {
                        Element.VerticalOptions = LayoutOptions.Center;
                        // spinner.Adapter = new ArrayAdapter(Context, global::Android.Resource.Layout.SimpleSpinnerItem, items);
                        spinner.Adapter = new FormsSpinnerAdapter(Context, items, Element.TextColor.ToAndroid(), Element.FontSize);
                    }

                    SetNativeControl(spinner);
                    Element.SelectedIndexChanged += Element_SelectedIndexChanged;
                    UpdateSelectedIndex();
                    UpdateTextColor();
                    UpdateFontSize();
                }
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == Picker.SelectedIndexProperty.PropertyName)
            {
                UpdateSelectedIndex();
            }
            else if (e.PropertyName == Picker.TextColorProperty.PropertyName)
            {
                UpdateTextColor();
            }
            else if (e.PropertyName == Picker.FontSizeProperty.PropertyName)
            {
                UpdateFontSize();
            }
            base.OnElementPropertyChanged(sender, e);
        }

        private void Element_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            UpdateSelectedIndex();
        }

        private void UpdateSelectedIndex()
        {
            var index = Element.SelectedIndex;
            if (Control.SelectedItemPosition != index)
            {
                Control.SetSelection(index);
            }
        }

        private void Spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            UpdateSelectedItem();
        }

        private void UpdateSelectedItem()
        {
            if (Element.SelectedIndex != Control.SelectedItemPosition)
            {
                Element.SelectedIndex = Control.SelectedItemPosition;
            }
        }

        private void UpdateTextColor()
        {
            var adapter = Control.Adapter as FormsSpinnerAdapter;
            if (adapter != null)
            {
                adapter.TextColor = Element.TextColor.ToAndroid();
            }
        }

        private void UpdateFontSize()
        {
            var adapter = Control.Adapter as FormsSpinnerAdapter;
            if (adapter != null)
            {
                adapter.FontSize = Element.FontSize;
            }
        }

        protected override FormsSpinner CreateNativeControl()
        {
            //var shape = new ShapeDrawable(new Android.Graphics.Drawables.Shapes.RectShape());
            //shape.Paint.Color = Xamarin.Forms.Color.Black.ToAndroid();
            //shape.Paint.SetStyle(Paint.Style.Stroke);

            // Base.Widget.AppCompat.Spinner.Underlined
            // , null, global::Android.Resource.Style.WidgetSpinnerDropDown
            var spinner = new FormsSpinner(Context)
            {
                Focusable = true,
                Clickable = true,
                Tag = this
            };
            return spinner;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && !_isDisposed)
            {
                _isDisposed = true;
            }

            base.Dispose(disposing);
        }
    }
}