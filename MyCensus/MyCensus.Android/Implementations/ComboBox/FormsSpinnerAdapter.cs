
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using Java.Lang;
using System.Collections;
using Xamarin.Forms;

using MyCensus.Controls.ComboBox;
using MyCensus.Droid.Implementations.ComboBox;

using AColor = Android.Graphics.Color;
using AView = Android.Views.View;

[assembly: ExportRenderer(typeof(ComboBox), typeof(ComboBoxRenderer))]

namespace MyCensus.Droid.Implementations.ComboBox
{

    /// <summary>
    /// 窗体微调适配器
    /// </summary>
    public class FormsSpinnerAdapter : BaseAdapter
    {
        private readonly IList _items;
        private AColor _textColor;
        private int _fontSize;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="items">绑定数据</param>
        /// <param name="textColor"></param>
        /// <param name="fontSize"></param>
        public FormsSpinnerAdapter(Context context, IList items, AColor textColor,int fontSize)
        {
            _items = items;
            _textColor = textColor;
            Context = context;
            _fontSize = fontSize;
        }

        public Context Context { get; private set; }

        public AColor TextColor
        {
            get
            {
                return _textColor;
            }
            set
            {
                if (_textColor != value)
                {
                    _textColor = value;
                    // Redraw all cells
                    NotifyDataSetChanged();
                }
            }
        }


        public int FontSize
        {
            get
            {
                return _fontSize;
            }
            set
            {
                if (_fontSize != value)
                {
                    _fontSize = value;
                    // Redraw all cells
                    NotifyDataSetChanged();
                }
            }
        }


        public override int Count => _items.Count;

        public override Object GetItem(int position)
        {
            return null;
        }

        public override long GetItemId(int position)
        {
            return 0;
        }

        public override AView GetView(int position, AView convertView, ViewGroup parent)
        {
            TextView view = new TextView(Context);

            //view.SetHeight(30);

            // view.SetBackgroundColor(bgColor);
            view.SetTextColor(_textColor);

            // view.SetTypeface(Typeface.Monospace, TypefaceStyle.Bold);
            //view.SetHeight(30);

            view.Text = _items[position].ToString();

            //view
            view.TextSize = _fontSize;

            view.SetPadding(10, 10, 10, 10);

            return view;

            //var dataTemplate = new DataTemplate(() =>
            //{
            //    var viewHolder = new ContentView();
            //    viewHolder.HorizontalOptions = LayoutOptions.Fill;
            //    viewHolder.VerticalOptions = LayoutOptions.Start;
            //    viewHolder.HeightRequest = 20;
            //    viewHolder.BackgroundColor = Color.Gray;

            //    var grid = new Grid();
            //    //grid.BackgroundColor = bgColor;
            //    var label = new Label { FontAttributes = FontAttributes.Bold };
            //    //label.SetBinding(Label.TextProperty, "Name");
            //    label.Text = _items[position].ToString();
            //    label.FontSize = _fontSize;
            //    label.HeightRequest = 20;
            //    grid.Children.Add(label);

            //    viewHolder.Content = grid;
            //    return viewHolder;
            //});

            //var view = dataTemplate.CreateContent() as VisualElement;
            //var renderer = Xamarin.Forms.Platform.Android.Platform.CreateRendererWithContext(view, Context);
            //Xamarin.Forms.Platform.Android.Platform.SetRenderer(view, renderer);

            //renderer.Element.Layout(new Rectangle(0, 0, 200, 40));
            //renderer.UpdateLayout();
            //var nativeView = renderer.View;
            //return renderer.View;
        }
    }
}