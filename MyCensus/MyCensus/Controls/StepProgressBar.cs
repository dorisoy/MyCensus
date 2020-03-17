using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using System.Linq;

namespace MyCensus.Controls
{

    /// <summary>
    /// 步骤进度条
    /// </summary>
    public class StepProgressBar : StackLayout
    {
        Button _lastStepSelected;
        public static readonly BindableProperty IsCircleProperty = BindableProperty.Create(nameof(IsCircle), typeof(bool), typeof(StepProgressBar), true);

        public static readonly BindableProperty StepsProperty = BindableProperty.Create(nameof(Steps), typeof(Dictionary<int, string>), typeof(StepProgressBar), new Dictionary<int, string> { { 0, "" } });

        public static readonly BindableProperty StepSelectedProperty = BindableProperty.Create(nameof(StepSelected), typeof(int), typeof(StepProgressBar), 0, defaultBindingMode: BindingMode.TwoWay);

        public static readonly BindableProperty StepColorProperty = BindableProperty.Create(nameof(StepColor), typeof(Xamarin.Forms.Color), typeof(StepProgressBar), Color.Black, defaultBindingMode: BindingMode.TwoWay);

        public static readonly BindableProperty StepCanTouchProperty = BindableProperty.Create(nameof(StepCanTouch), typeof(bool), typeof(StepProgressBar), true);

        public Color StepColor
        {
            get { return (Color)GetValue(StepColorProperty); }
            set { SetValue(StepColorProperty, value); }
        }

        public Dictionary<int,string> Steps
        {
            get { return (Dictionary<int, string>)GetValue(StepsProperty); }
            set { SetValue(StepsProperty, value); }
        }

        public int StepSelected
        {
            get { return (int)GetValue(StepSelectedProperty); }
            set { SetValue(StepSelectedProperty, value); }
        }

        public bool StepCanTouch
        {
            get { return (bool)GetValue(StepCanTouchProperty); }
            set { SetValue(StepCanTouchProperty, value); }
        }

        public bool IsCircle
        {
            get { return (bool)GetValue(IsCircleProperty); }
            set { SetValue(IsCircleProperty, value); }
        }


        public StepProgressBar()
        {
            Orientation = StackOrientation.Horizontal;
            HorizontalOptions = LayoutOptions.FillAndExpand;
            Padding = new Thickness(10, 0);
            Spacing = 0;
            AddStyles();
        }

        protected override void OnPropertyChanged(string propertyName = null)
        {
            try
            {
                base.OnPropertyChanged(propertyName);

                if (propertyName == StepsProperty.PropertyName)
                {
                    for (int i = 0; i < Steps.Count; i++)
                    {
                        var button = new Button()
                        {
                            Text = (i + 1) + "." + Steps[i].ToString(),
                            ClassId = $"{i + 1}",
                            FontSize = 12,
                            Style = (IsCircle ? Resources["unselectedCircleStyle"] : Resources["unselectedSquareStyle"]) as Style
                        };

                        button.CornerRadius = 20;
                        button.Clicked -= Handle_Clicked;
                        button.Clicked += Handle_Clicked;

                        //button.StyleClass = new List<string> { "steperbutton" };
                        this.Children.Add(button);

                        if (i < Steps.Count - 1)
                        {
                            var separatorLine = new BoxView()
                            {
                                BackgroundColor = Color.FromHex("#2399a5"),
                                HeightRequest = 1,
                                WidthRequest = 10,
                                VerticalOptions = LayoutOptions.Center,
                                HorizontalOptions = LayoutOptions.FillAndExpand
                            };
                            this.Children.Add(separatorLine);
                        }
                    }
                }
                else if (propertyName == StepSelectedProperty.PropertyName)
                {

                    var children = this.Children.FirstOrDefault(p => (!string.IsNullOrEmpty(p.ClassId) && Convert.ToInt32(p.ClassId) == StepSelected));
                    if (children != null)
                    {
                        var button = children as Button;
                        SelectElement(button);
                    }

                }
                else if (propertyName == StepColorProperty.PropertyName)
                {
                    AddStyles();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Print(ex.Message);
            }
        }



        void Handle_Clicked(object sender, System.EventArgs e)
        {
            if (StepCanTouch)
                SelectElement(sender as Button);
        }

        void SelectElement(Button elementSelected)
        {
            if (_lastStepSelected != null) _lastStepSelected.Style = (IsCircle ? Resources["unselectedCircleStyle"] : Resources["unselectedSquareStyle"]) as Style;
            elementSelected.Style = (IsCircle ? Resources["selectedCircleStyle"] : Resources["selectedSquareStyle"]) as Style;
            StepSelected = Convert.ToInt32(elementSelected.ClassId);
            _lastStepSelected = elementSelected;
        }

        void AddStyles()
        {
            double borderWith = IsCircle ? 0.5 : 0;

            var unselectedCircleStyle = new Style(typeof(Button))
            {
                Setters = {
                    new Setter { Property = BackgroundColorProperty,   Value = Color.FromHex("#ffffff") },
                    new Setter { Property = Button.BorderColorProperty,   Value = Color.FromHex("#ffffff") },
                    new Setter { Property = Button.TextColorProperty,   Value = StepColor },
                    new Setter { Property = Button.BorderWidthProperty,   Value = 0.5 },
                    new Setter { Property = Button.CornerRadiusProperty,   Value = 0 },
                    new Setter { Property = HeightRequestProperty,   Value = 35 },
                    new Setter { Property = Button.FontSizeProperty,   Value = 12 }
            }
            };

            var selectedCircleStyle = new Style(typeof(Button))
            {
                Setters = {
                    new Setter { Property = BackgroundColorProperty, Value = StepColor },
                    new Setter { Property = Button.TextColorProperty, Value = Color.White },
                    new Setter { Property = Button.BorderColorProperty, Value = StepColor },
                    new Setter { Property = Button.BorderWidthProperty,   Value =  0.5 },
                    new Setter { Property = Button.CornerRadiusProperty,   Value = 0 },
                    new Setter { Property = HeightRequestProperty,   Value = 35 },
                    new Setter { Property = Button.FontSizeProperty,   Value = 12 },
                    new Setter { Property = Button.FontAttributesProperty,   Value = FontAttributes.Bold }
            }
            };

            var unselectedSquareStyle = new Style(typeof(Button))
            {
                Setters = {
                    new Setter { Property = BackgroundColorProperty,   Value = Color.Transparent },
                    new Setter { Property = Button.BorderColorProperty,   Value = StepColor },
                    new Setter { Property = Button.TextColorProperty,   Value = StepColor },
                    new Setter { Property = Button.BorderWidthProperty,   Value = 1 },
                    new Setter { Property = Button.CornerRadiusProperty,   Value = 0},
                    new Setter { Property = HeightRequestProperty,   Value = 40 },
                    //new Setter { Property = WidthRequestProperty,   Value = 40 }
            }
            };

            var selectedSquareStyle = new Style(typeof(Button))
            {
                Setters = {
                    new Setter { Property = BackgroundColorProperty, Value = StepColor },
                    new Setter { Property = Button.TextColorProperty, Value = Color.White },
                    new Setter { Property = Button.BorderColorProperty, Value = StepColor },
                    new Setter { Property = Button.BorderWidthProperty,   Value = 1 },
                    new Setter { Property = Button.CornerRadiusProperty,   Value = 0 },
                    new Setter { Property = HeightRequestProperty,   Value = 40 },
                    //new Setter { Property = WidthRequestProperty,   Value = 40 },
                    new Setter { Property = Button.FontAttributesProperty,   Value = FontAttributes.Bold }
            }
            };

            Resources = new ResourceDictionary();
            Resources.Add("unselectedCircleStyle", unselectedCircleStyle);
            Resources.Add("selectedCircleStyle", selectedCircleStyle);
            Resources.Add("unselectedSquareStyle", unselectedSquareStyle);
            Resources.Add("selectedSquareStyle", selectedSquareStyle);
        }


    }
}
