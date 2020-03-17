using MyCensus.Abstractions;
using System;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace MyCensus.Controls
{
    public partial class IconButton : ContentView
    {
        private bool addedAnimation;

        public static readonly BindableProperty IconSourceProperty =
            BindableProperty.Create(
                "IconSource",
                typeof(ImageSource),
                typeof(IconButton),
                null,
                propertyChanged: (bindable, oldValue, newValue) =>
                {
                    ((IconButton)bindable).IconSource = (ImageSource)newValue;
                }
            );

        public ImageSource IconSource
        {
            get { return (ImageSource)GetValue(IconSourceProperty); }
            set
            {
                SetValue(IconSourceProperty, value);
                imgIcon.Source = value;
            }
        }

        public static readonly BindableProperty IconMarginProperty =
            BindableProperty.Create(
                "IconMargin",
                typeof(Thickness),
                typeof(IconButton),
                new Thickness(0),
                propertyChanged: (bindable, oldValue, newValue) =>
                {
                    ((IconButton)bindable).IconMargin = (Thickness)newValue;
                }
            );

        public Thickness IconMargin
        {
            get { return (Thickness)GetValue(IconMarginProperty); }
            set
            {
                SetValue(IconMarginProperty, value);
                grdIconButton.Margin = value;
            }
        }

        public static readonly BindableProperty IconHeightProperty =
            BindableProperty.Create(
                "IconHeight",
                typeof(double),
                typeof(IconButton),
                0d,
                propertyChanged: (bindable, oldValue, newValue) =>
                {
                    ((IconButton)bindable).IconHeight = (double)newValue;
                }
            );
        public double IconHeight
        {
            get { return (double)GetValue(IconHeightProperty); }
            set
            {
                SetValue(IconHeightProperty, value);
                imgIcon.HeightRequest = value;
            }
        }

        public static readonly BindableProperty DescriptionPositionProperty =
            BindableProperty.Create(
                "DescriptionPosition",
                typeof(DescriptionPosition),
                typeof(IconButton),
                DescriptionPosition.None,
                propertyChanged: (bindable, oldValue, newValue) =>
                {
                    ((IconButton)bindable).DescriptionPosition = (DescriptionPosition)newValue;
                }
            );

        public DescriptionPosition DescriptionPosition
        {
            get { return (DescriptionPosition)GetValue(DescriptionPositionProperty); }
            set
            {
                SetValue(DescriptionPositionProperty, value);
                setPosition(value);
            }
        }

        public static readonly BindableProperty DescriptionSizeProperty =
            BindableProperty.Create(
                "DescriptionSize",
                typeof(double),
                typeof(IconButton),
                0d,
                propertyChanged: (bindable, oldValue, newValue) =>
                {
                    ((IconButton)bindable).DescriptionSize = (double)newValue;
                }
            );

        public double DescriptionSize
        {
            get { return (double)GetValue(DescriptionSizeProperty); }
            set
            {
                SetValue(DescriptionSizeProperty, value);
                lblDescription.FontSize = value;
            }
        }


        public static readonly BindableProperty DescriptionColorProperty =
            BindableProperty.Create(
                "DescriptionColor",
                typeof(Color),
                typeof(IconButton),
                Color.Black,
                propertyChanged: (bindable, oldValue, newValue) =>
                {
                    ((IconButton)bindable).DescriptionColor = (Color)newValue;
                }
            );

        public Color DescriptionColor
        {
            get { return (Color)GetValue(DescriptionColorProperty); }
            set
            {
                SetValue(DescriptionColorProperty, value);
                lblDescription.TextColor = value;
            }
        }

        public string DescriptionText
        {
            get { return (string)GetValue(DescriptionTextProperty); }
            set
            {
                SetValue(DescriptionTextProperty, value);
                lblDescription.Text = value;
            }
        }

        public static readonly BindableProperty DescriptionTextProperty =
            BindableProperty.Create(
                "DescriptionText",
                typeof(string),
                typeof(IconButton),
                string.Empty,
                propertyChanged: (bindable, oldValue, newValue) =>
                {
                    ((IconButton)bindable).DescriptionText = (string)newValue;
                }
            );

        public IconButton()
        {
            InitializeComponent();
        }

        private void imgIcon_BindingContextChanged(object sender, EventArgs e)
        {
            if (addedAnimation || GestureRecognizers.Count == 0)
                return;

            var tapGesture = GestureRecognizers[0] as TapGestureRecognizer;
            if (tapGesture == null)
                return;

            tapGesture.Tapped += (tapsender, tape) =>
            {
                Device.BeginInvokeOnMainThread(async () => await tappedAnimation());
            };

            addedAnimation = true;
        }

        private async Task tappedAnimation()
        {
            BackgroundColor = Color.FromRgba(68, 68, 68, 70);
            await imgIcon.ScaleTo(0.8, 75);
            await imgIcon.ScaleTo(1.0, 75);
            BackgroundColor = Color.Transparent;
        }

        private void setPosition(DescriptionPosition position)
        {
            switch (position)
            {
                case DescriptionPosition.None:
                    imgIcon.SetValue(Grid.RowProperty, 0);
                    lblDescription.SetValue(Grid.RowProperty, 0);
                    imgIcon.SetValue(Grid.ColumnProperty, 0);
                    lblDescription.SetValue(Grid.ColumnProperty, 0);
                    lblDescription.IsVisible = false;
                    break;
                case DescriptionPosition.Left:
                    imgIcon.SetValue(Grid.RowProperty, 0);
                    lblDescription.SetValue(Grid.RowProperty, 0);
                    imgIcon.SetValue(Grid.ColumnProperty, 1);
                    lblDescription.SetValue(Grid.ColumnProperty, 0);
                    break;
                case DescriptionPosition.Top:
                    imgIcon.SetValue(Grid.RowProperty, 1);
                    lblDescription.SetValue(Grid.RowProperty, 0);
                    imgIcon.SetValue(Grid.ColumnProperty, 0);
                    lblDescription.SetValue(Grid.ColumnProperty, 0);
                    break;
                case DescriptionPosition.Right:
                    imgIcon.SetValue(Grid.RowProperty, 0);
                    lblDescription.SetValue(Grid.RowProperty, 0);
                    imgIcon.SetValue(Grid.ColumnProperty, 0);
                    lblDescription.SetValue(Grid.ColumnProperty, 1);
                    break;
                case DescriptionPosition.Bottom:
                    imgIcon.SetValue(Grid.RowProperty, 0);
                    lblDescription.SetValue(Grid.RowProperty, 1);
                    imgIcon.SetValue(Grid.ColumnProperty, 0);
                    lblDescription.SetValue(Grid.ColumnProperty, 0);
                    break;
                default:
                    break;
            }
        }
    }
}
