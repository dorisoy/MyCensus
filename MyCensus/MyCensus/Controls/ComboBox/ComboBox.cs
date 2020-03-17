using System;
using System.Collections;
using System.Collections.Specialized;
using Xamarin.Forms;
using Xamarin.Forms.Internals;


namespace MyCensus.Controls.ComboBox
{
    public class SelectedIndexChangedEventArgs : EventArgs
    {
        public SelectedIndexChangedEventArgs(int oldIndex, int newIndex)
        {
            OldIndex = oldIndex;
            NewIndex = newIndex;
        }
        public int OldIndex { get; set; }
        public int NewIndex { get; set; }
    }

    /// <summary>
    /// 自定义ComboBox
    /// </summary>
    public class ComboBox : View
    {
        #region Dependency property

        public static readonly BindableProperty FontSizeProperty =
         BindableProperty.Create(
             nameof(FontSize),
             typeof(int),
             typeof(ComboBox),
             default(int));


        public static readonly BindableProperty TextColorProperty =
            BindableProperty.Create(
                nameof(TextColor),
                typeof(Color),
                typeof(ComboBox),
                default(Color));

        public static readonly BindableProperty SelectedIndexProperty =
            BindableProperty.Create(
                nameof(SelectedIndex),
                typeof(int),
                typeof(ComboBox),
                -1,
                BindingMode.TwoWay,
                propertyChanged: OnSelectedIndexChanged,
                coerceValue: CoerceSelectedIndex);

        public static readonly BindableProperty ItemsSourceProperty =
            BindableProperty.Create(
                nameof(ItemsSource),
                typeof(IList),
                typeof(ComboBox),
                default(IList),
                propertyChanged: OnItemsSourceChanged);

        public static readonly BindableProperty SelectedItemProperty =
            BindableProperty.Create(
                nameof(SelectedItem),
                typeof(object),
                typeof(ComboBox),
                null,
                BindingMode.TwoWay,
                propertyChanged: OnSelectedItemChanged);
        #endregion

        #region Properties

        public IList ItemsSource
        {
            get { return (IList)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public int SelectedIndex
        {
            get { return (int)GetValue(SelectedIndexProperty); }
            set { SetValue(SelectedIndexProperty, value); }
        }

        public object SelectedItem
        {
            get { return GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        public Color TextColor
        {
            get { return (Color)GetValue(TextColorProperty); }
            set { SetValue(TextColorProperty, value); }
        }

        public int FontSize
        {
            get { return (int)GetValue(FontSizeProperty); }
            set { SetValue(FontSizeProperty, value); }
        }
        #endregion

        #region Events
        public event EventHandler<SelectedIndexChangedEventArgs> SelectedIndexChanged;
        #endregion

        #region Static functions
        static object CoerceSelectedIndex(BindableObject bindable, object value)
        {
            var comboBox = (ComboBox)bindable;
            if (value is int)
            {
                return comboBox.ItemsSource == null ? -1 : ((int)value).Clamp(-1, comboBox.ItemsSource.Count - 1);
            }

            throw new InvalidOperationException("Selected Index must be an interger");
        }

        static void OnSelectedIndexChanged(object bindable, object oldValue, object newValue)
        {
            var comboBox = (ComboBox)bindable;
            comboBox.UpdateSelectedItem();
            comboBox.SelectedIndexChanged?.Invoke(bindable, new SelectedIndexChangedEventArgs((int)oldValue, (int)newValue));
        }

        static void OnSelectedItemChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var comboBox = (ComboBox)bindable;
            comboBox.UpdateSelectedIndex(newValue);
        }

        static void OnItemsSourceChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ((ComboBox)bindable).OnItemsSourceChanged((IList)oldValue, (IList)newValue);
        }
        #endregion

        void OnItemsSourceChanged(IList oldValue, IList newValue)
        {
            if (oldValue is INotifyCollectionChanged oldObservable)
            {
                oldObservable.CollectionChanged -= CollectionChanged;
            }

            if (newValue is INotifyCollectionChanged newObservable)
            {
                newObservable.CollectionChanged += CollectionChanged;
            }
        }

        void CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    break;
                case NotifyCollectionChangedAction.Remove:
                    break;
                default: //Move, Replace, Reset
                    break;
            }
        }

        void UpdateSelectedIndex(object selectedItem)
        {
            if (ItemsSource != null)
            {
                SelectedIndex = ItemsSource.IndexOf(selectedItem);
                return;
            }
        }

        void UpdateSelectedItem()
        {
            if (SelectedIndex == -1)
            {
                SelectedItem = null;
            }
            else if (ItemsSource != null)
            {
                SelectedItem = ItemsSource[SelectedIndex];
            }
        }

    }
}
