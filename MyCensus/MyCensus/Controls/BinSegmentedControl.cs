using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;

namespace MyCensus.Controls
{
    //public class BinSegmentedControl
    //{
    //}


    public class BinSegmentedControl : View, IViewContainer<BinSegmentedControlOption>
    {
        public IList<BinSegmentedControlOption> Children { get; set; }

        public BinSegmentedControl()
        {
            Children = new List<BinSegmentedControlOption>();
        }

        public static readonly BindableProperty TintColorProperty = BindableProperty.Create("TintColor", typeof(Color), typeof(BinSegmentedControl), Color.Blue);

        public Color TintColor
        {
            get { return (Color)GetValue(TintColorProperty); }
            set { SetValue(TintColorProperty, value); }
        }

        public static readonly BindableProperty DisabledColorProperty = BindableProperty.Create("DisabledColor", typeof(Color), typeof(BinSegmentedControl), Color.Gray);

        public Color DisabledColor
        {
            get { return (Color)GetValue(DisabledColorProperty); }
            set { SetValue(DisabledColorProperty, value); }
        }

        public static readonly BindableProperty SelectedTextColorProperty = BindableProperty.Create("SelectedTextColor", typeof(Color), typeof(BinSegmentedControl), Color.White);

        public Color SelectedTextColor
        {
            get { return (Color)GetValue(SelectedTextColorProperty); }
            set { SetValue(SelectedTextColorProperty, value); }
        }

        public static readonly BindableProperty SelectedSegmentProperty = BindableProperty.Create("SelectedSegment", typeof(int), typeof(BinSegmentedControl), 0);

        public int SelectedSegment
        {
            get
            {
                return (int)GetValue(SelectedSegmentProperty);
            }
            set
            {
                SetValue(SelectedSegmentProperty, value);
            }
        }

        public event EventHandler<ValueChangedEventArgs> ValueChanged;

        [EditorBrowsable(EditorBrowsableState.Never)]
        public void SendValueChanged()
        {
            ValueChanged?.Invoke(this, new ValueChangedEventArgs { NewValue = this.SelectedSegment });
        }
    }

    public class BinSegmentedControlOption : View
    {
        public static readonly BindableProperty TextProperty = BindableProperty.Create("Text", typeof(string), typeof(BinSegmentedControlOption), string.Empty);

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
    }

    public class ValueChangedEventArgs : EventArgs
    {
        public int NewValue { get; set; }
    }
}
