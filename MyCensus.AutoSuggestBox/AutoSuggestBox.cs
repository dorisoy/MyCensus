using System;
using System.Linq;
using System.Runtime.CompilerServices;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using MyCensus.Droid.AutoSuggestBox;

[assembly: ExportRenderer(typeof(MyCensus.Droid.AutoSuggestBox.AutoSuggestBox), typeof(MyCensus.Droid.AutoSuggestBox.AutoSuggestBoxRenderer))]
namespace MyCensus.Droid.AutoSuggestBox
{

	public partial class AutoSuggestBox : View
    {

        internal NativeAutoSuggestBox NativeAutoSuggestBox { get; }

        private bool suppressTextChangedEvent;

        /// <summary>
        /// Initializes a new instance of the <see cref="AutoSuggestBox"/> class
        /// </summary>
        public AutoSuggestBox() 
        {

            NativeAutoSuggestBox = new NativeAutoSuggestBox(

                Android.App.Application.Context

                );
            NativeAutoSuggestBox.SuggestionChosen += (s, e) => { SuggestionChosen?.Invoke(this, new AutoSuggestBoxSuggestionChosenEventArgs(e.SelectedItem)); };
            NativeAutoSuggestBox.TextChanged += (s, e) => { 
                suppressTextChangedEvent = true;
                Text = NativeAutoSuggestBox.Text; 
                suppressTextChangedEvent = false;
                TextChanged?.Invoke(this, new AutoSuggestBoxTextChangedEventArgs((AutoSuggestionBoxTextChangeReason) e.Reason)); 
            };
            NativeAutoSuggestBox.QuerySubmitted += (s, e) => QuerySubmitted?.Invoke(this, new AutoSuggestBoxQuerySubmittedEventArgs(e.QueryText, e.ChosenSuggestion));

        }

        /// <inheritdoc />
        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if(propertyName == nameof(IsEnabled))
            {
                NativeAutoSuggestBox.Enabled = IsEnabled;
            }
            base.OnPropertyChanged(propertyName);
        }

        /// <summary>
        /// Gets or sets the Text property
        /// </summary>
        /// <seealso cref="TextColor"/>
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="Text"/> bindable property.
        /// </summary>
        public static readonly BindableProperty TextProperty =
            BindableProperty.Create(nameof(Text), typeof(string), typeof(AutoSuggestBox), "", BindingMode.OneWay, null, OnTextPropertyChanged);

        private static void OnTextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var box = (AutoSuggestBox)bindable;

            if (box.NativeAutoSuggestBox.Text != newValue as string)
                box.NativeAutoSuggestBox.Text = newValue as string;

            if (!box.suppressTextChangedEvent)
                box.TextChanged?.Invoke(box, new AutoSuggestBoxTextChangedEventArgs(AutoSuggestionBoxTextChangeReason.ProgrammaticChange));
        }

        /// <summary>
        /// Gets or sets the foreground color of the control
        /// </summary>
        /// <seealso cref="Text"/>
        public global::Xamarin.Forms.Color TextColor
        {
            get { return (global::Xamarin.Forms.Color )GetValue(TextColorProperty); }
            set { SetValue(TextColorProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="TextColor"/> bindable property.
        /// </summary>
        public static readonly BindableProperty TextColorProperty =
            BindableProperty.Create(nameof(TextColor), typeof(global::Xamarin.Forms.Color ), typeof(AutoSuggestBox), global::Xamarin.Forms.Color.Gray, BindingMode.OneWay, null, OnTextColorPropertyChanged);

        private static void OnTextColorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var box = (AutoSuggestBox)bindable;
            var color = (global::Xamarin.Forms.Color)newValue;
            box.NativeAutoSuggestBox.SetTextColor(color);
        }



        /// <summary>
        /// Gets or sets the PlaceholderText
        /// </summary>
        public string PlaceholderText
        {
            get { return (string)GetValue(PlaceholderTextProperty); }
            set { SetValue(PlaceholderTextProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="PlaceholderText"/> bindable property.
        /// </summary>
        public static readonly BindableProperty PlaceholderTextProperty =
            BindableProperty.Create(nameof(PlaceholderText), typeof(string), typeof(AutoSuggestBox), "", BindingMode.OneWay, null, OnPlaceholderTextPropertyChanged);

        private static void OnPlaceholderTextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var box = (AutoSuggestBox)bindable;
            box.NativeAutoSuggestBox.PlaceholderText = newValue as string;
        }

        /// <summary>
        /// Gets or sets the property path that is used to get the value for display in the
        /// text box portion of the AutoSuggestBox control, when an item is selected.
        /// </summary>
        /// <value>
        /// The property path that is used to get the value for display in the text box portion
        /// of the AutoSuggestBox control, when an item is selected.
        /// </value>
        public string TextMemberPath
        {
            get { return (string)GetValue(TextMemberPathProperty); }
            set { SetValue(TextMemberPathProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="TextMemberPath"/> bindable property.
        /// </summary>
        public static readonly BindableProperty TextMemberPathProperty =
            BindableProperty.Create(nameof(TextMemberPath), typeof(string), typeof(AutoSuggestBox), null, BindingMode.OneWay, null, OnTextMemberPathPropertyChanged);

        private static void OnTextMemberPathPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {

        }

        /// <summary>
        /// Gets or sets the name or path of the property that is displayed for each data item.
        /// </summary>
        /// <value>
        /// The name or path of the property that is displayed for each the data item in
        /// the control. The default is an empty string ("").
        /// </value>
        public string DisplayMemberPath
        {
            get { return (string)GetValue(DisplayMemberPathProperty); }
            set { SetValue(DisplayMemberPathProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="DisplayMemberPath"/> bindable property.
        /// </summary>
        public static readonly BindableProperty DisplayMemberPathProperty =
            BindableProperty.Create(nameof(DisplayMemberPath), typeof(string), typeof(AutoSuggestBox), "", BindingMode.OneWay, null, OnDisplayMemberPathPropertyChanged);

        private static void OnDisplayMemberPathPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var box = (AutoSuggestBox)bindable;
            box.NativeAutoSuggestBox.SetItems(box.ItemsSource?.OfType<object>(), (o) => FormatType(o, box.DisplayMemberPath), (o) => FormatType(o, box.TextMemberPath));
        }

        /// <summary>
        /// Gets or sets a Boolean value indicating whether the drop-down portion of the AutoSuggestBox is open.
        /// </summary>
        /// <value>A Boolean value indicating whether the drop-down portion of the AutoSuggestBox is open.</value>
        public bool IsSuggestionListOpen
        {
            get { return (bool)GetValue(IsSuggestionListOpenProperty); }
            set { SetValue(IsSuggestionListOpenProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="IsSuggestionListOpen"/> bindable property.
        /// </summary>
        public static readonly BindableProperty IsSuggestionListOpenProperty =
            BindableProperty.Create(nameof(IsSuggestionListOpen), typeof(bool), typeof(AutoSuggestBox), false, BindingMode.OneWay, null, OnIsSuggestionListOpenPropertyChanged);

        private static void OnIsSuggestionListOpenPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var box = (AutoSuggestBox)bindable;
            bool isOpen = (bool)newValue;
            box.NativeAutoSuggestBox.IsSuggestionListOpen = isOpen;
        }

        /// <summary>
        /// Gets or sets the header object for the text box portion of this control.
        /// </summary>
        /// <value>The header object for the text box portion of this control.</value>
        public System.Collections.IList ItemsSource
        {
            get { return GetValue(ItemsSourceProperty) as System.Collections.IList; }
            set { SetValue(ItemsSourceProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="ItemsSource"/> bindable property.
        /// </summary>
        public static readonly BindableProperty ItemsSourceProperty =
            BindableProperty.Create(nameof(ItemsSource), typeof(System.Collections.IList), typeof(AutoSuggestBox), null, BindingMode.OneWay, null, OnItemsSourcePropertyChanged);

        private static void OnItemsSourcePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var box = (AutoSuggestBox)bindable;
            box.NativeAutoSuggestBox.SetItems(box.ItemsSource?.OfType<object>(), (o) => FormatType(o, box.DisplayMemberPath), (o) => FormatType(o, box.TextMemberPath));
        }

        /// <summary>
        /// Raised before the text content of the editable control component is updated.
        /// </summary>
        public event EventHandler<AutoSuggestBoxSuggestionChosenEventArgs> SuggestionChosen;

        /// <summary>
        /// Raised after the text content of the editable control component is updated.
        /// </summary>
        public event EventHandler<AutoSuggestBoxTextChangedEventArgs> TextChanged;

        /// <summary>
        /// Occurs when the user submits a search query.
        /// </summary>
        public event EventHandler<AutoSuggestBoxQuerySubmittedEventArgs> QuerySubmitted;

        private static string FormatType(object instance, string memberPath)
        {
            if (!string.IsNullOrEmpty(memberPath))
                return instance?.GetType().GetProperty(memberPath)?.GetValue(instance)?.ToString() ?? "";
            else
                return instance?.ToString() ?? "";
        }
    }
}
