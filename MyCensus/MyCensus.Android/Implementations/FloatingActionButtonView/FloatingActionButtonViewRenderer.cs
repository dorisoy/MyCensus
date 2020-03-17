using Android.Content;
using Android.Graphics;
using Android.Views;
using Android.Widget;

using Refractored.Fab;

using System;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;


using MyCensus.Controls;
using MyCensus.Droid.Renderers;



[assembly: ExportRenderer(typeof(FloatingActionButtonView), typeof(FloatingActionButtonViewRenderer))]
namespace MyCensus.Droid.Renderers
{
    public class FloatingActionButtonViewRenderer : ViewRenderer<FloatingActionButtonView, FrameLayout>
    {
        private const int MARGIN_DIPS = 16;
        private const int FAB_HEIGHT_NORMAL = 56;
        private const int FAB_HEIGHT_MINI = 40;
        private const int FAB_FRAME_HEIGHT_WITH_PADDING = MARGIN_DIPS * 2 + FAB_HEIGHT_NORMAL;
        private const int FAB_FRAME_WIDTH_WITH_PADDING = MARGIN_DIPS * 2 + FAB_HEIGHT_NORMAL;
        private const int FAB_MINI_FRAME_HEIGHT_WITH_PADDING = MARGIN_DIPS * 2 + FAB_HEIGHT_MINI;
        private const int FAB_MINI_FRAME_WIDTH_WITH_PADDING = MARGIN_DIPS * 2 + FAB_HEIGHT_MINI;
        private readonly Context context;

        private readonly Refractored.Fab.FloatingActionButton fab;
        private int appearingListItemIndex = 0;

        public FloatingActionButtonViewRenderer(Context context) : base(context)
        {
            this.context = context;
            var d = context.Resources.DisplayMetrics.Density;
            var margin = (int)(MARGIN_DIPS * d); // margin in pixels
            fab = new FloatingActionButton(context);
            var lp = new FrameLayout.LayoutParams(LayoutParams.WrapContent, LayoutParams.WrapContent)
            {
                Gravity = GravityFlags.CenterVertical | GravityFlags.CenterHorizontal,
                LeftMargin = margin,
                TopMargin = margin,
                BottomMargin = margin,
                RightMargin = margin
            };
            fab.LayoutParameters = lp;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<FloatingActionButtonView> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null || Element == null)
                return;

            if (e.OldElement != null)
            {
                e.OldElement.PropertyChanged -= HandlePropertyChanged;
                // Experimental - Hiding and showing the FAB correctly is dependent on the objects in the list being unique
                if (e.OldElement.ParentList != null)
                {
                    e.OldElement.ParentList.ItemAppearing -= OnListItemAppearing;
                    e.OldElement.ParentList.ItemDisappearing -= OnListItemDisappearing;
                }
            }

            if (Element != null)
            {
                Element.PropertyChanged += HandlePropertyChanged;
                // Experimental - Hiding and showing the FAB correctly is dependent on the objects in the list being unique
                if (Element.ParentList != null)
                {
                    Element.ParentList.ItemAppearing += OnListItemAppearing;
                    Element.ParentList.ItemDisappearing += OnListItemDisappearing;
                }
            }

            Element.Show = Show;
            Element.Hide = Hide;

            SetFabImage(Element.ImageName);
            SetFabSize(Element.Size);

            fab.ColorNormal = Element.ColorNormal.ToAndroid();
            fab.ColorPressed = Element.ColorPressed.ToAndroid();
            fab.ColorRipple = Element.ColorRipple.ToAndroid();
            fab.HasShadow = Element.HasShadow;
            fab.Click += Fab_Click;

            var frame = new FrameLayout(context);
            frame.RemoveAllViews();
            frame.AddView(fab);

            SetNativeControl(frame);
        }

        private async void OnListItemDisappearing(object sender, ItemVisibilityEventArgs e)
        {
            // Experimental - Hiding and showing the FAB correctly is dependent on the objects in the list being unique
            if (!(sender is Xamarin.Forms.ListView list)) return;
            await Task.Run(() =>
            {
                if (list.ItemsSource is IList items)
                {
                    var index = items.IndexOf(e.Item);
                    if (index < appearingListItemIndex && index >= 0)
                    {
                        appearingListItemIndex = index;
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            fab.Hide();
                            await Task.Delay(500);
                            fab.Show();
                        });
                    }
                    else
                    {
                        appearingListItemIndex = index;
                    }
                }
            });
        }

        private async void OnListItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            // Experimental - Hiding and showing the FAB correctly is dependent on the objects in the list being unique
            if (!(sender is Xamarin.Forms.ListView list)) return;
            await Task.Run(() =>
            {
                if (list.ItemsSource is IList items)
                {
                    var index = items.IndexOf(e.Item);
                    if (index < appearingListItemIndex)
                    {
                        appearingListItemIndex = index;
                        Device.BeginInvokeOnMainThread(() => fab.Show());
                    }
                    else
                    {
                        appearingListItemIndex = index;
                    }
                }
            });
        }

        public void Show(bool animate = true)
        {
            fab.Show(animate);
        }

        public void Hide(bool animate = true)
        {
            fab.Hide(animate);
        }

        private void HandlePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Content")
            {
                Tracker.UpdateLayout();
            }
            else if (e.PropertyName == FloatingActionButtonView.ColorNormalProperty.PropertyName)
            {
                fab.ColorNormal = Element.ColorNormal.ToAndroid();
            }
            else if (e.PropertyName == FloatingActionButtonView.ColorPressedProperty.PropertyName)
            {
                fab.ColorPressed = Element.ColorPressed.ToAndroid();
            }
            else if (e.PropertyName == FloatingActionButtonView.ColorRippleProperty.PropertyName)
            {
                fab.ColorRipple = Element.ColorRipple.ToAndroid();
            }
            else if (e.PropertyName == FloatingActionButtonView.ImageNameProperty.PropertyName)
            {
                SetFabImage(Element.ImageName);
            }
            else if (e.PropertyName == FloatingActionButtonView.SizeProperty.PropertyName)
            {
                SetFabSize(Element.Size);
            }
            else if (e.PropertyName == FloatingActionButtonView.HasShadowProperty.PropertyName)
            {
                fab.HasShadow = Element.HasShadow;
            }
        }

        private void SetFabImage(string imageName)
        {
            if (!string.IsNullOrWhiteSpace(imageName))
            {
                try
                {
                    var drawableNameWithoutExtension = System.IO.Path.GetFileNameWithoutExtension(imageName);
                    var resources = context.Resources;
                    var imageResourceName = resources.GetIdentifier(drawableNameWithoutExtension, "drawable",
                        context.PackageName);
                    fab.SetImageBitmap(BitmapFactory.DecodeResource(context.Resources, imageResourceName));
                }
                catch (Exception ex)
                {
                    throw new FileNotFoundException("There was no Android Drawable by that name.", ex);
                }
            }
        }

        private void SetFabSize(FloatingActionButtonSize size)
        {
            if (size == FloatingActionButtonSize.Mini)
            {
                fab.Size = FabSize.Mini;
                Element.WidthRequest = FAB_MINI_FRAME_WIDTH_WITH_PADDING;
                Element.HeightRequest = FAB_MINI_FRAME_HEIGHT_WITH_PADDING;
            }
            else
            {
                fab.Size = FabSize.Normal;
                Element.WidthRequest = FAB_FRAME_WIDTH_WITH_PADDING;
                Element.HeightRequest = FAB_FRAME_HEIGHT_WITH_PADDING;
            }
        }

        private void Fab_Click(object sender, EventArgs e)
        {
            var clicked = Element.Clicked;
            if (Element != null)
            {
                clicked?.Invoke(sender, e);
                if (Element.Command != null && Element.Command.CanExecute(null)) Element.Command.Execute(null);
            }
        }

        protected override FrameLayout CreateNativeControl()
        {
            return null;
        }
    }
}