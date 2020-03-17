using MyCensus.Animations;
using MyCensus.Extensions;
using MyCensus.Models.ReportIncident;
using MyCensus.ViewModels.Base;
using System.Threading.Tasks;
using Xamarin.Forms;
using System;

namespace MyCensus.Views
{
    public partial class ReportIncidentPage : ContentPage
    {
        public ReportIncidentPage()
        {
            InitializeComponent();
            InitializeDoneMessageContainer();

            MessagingCenter.Subscribe<ReportedIssue>(this, MessengerKeys.ReportSent, OnReportSent);

            if (Device.RuntimePlatform == Device.UWP)
            {
                IncidentInformation.WidthRequest = 620;
                SendIncident.WidthRequest = 450;
            }

            if (Device.Idiom == TargetIdiom.Desktop)
            {
                SizeChanged += OnSizeChanged;
            }
        }

        private void OnReportSent(ReportedIssue issue)
        {
            var storyboard = new StoryBoard();

            storyboard.Animations.Add(new FadeToAnimation
            {
                Opacity = 1,
                Easing = EasingType.Linear,
                Duration = "800"
            });

            storyboard.Animations.Add(new TranslateToAnimation
            {
                TranslateY = 0,
                Easing = EasingType.CubicOut,
                Duration = "800"
            });

            DoneMessageContainer.Animate(storyboard, async () =>
            {
                await OnDoneMessageAnimationFinished(issue);
            });
        }

        private async Task OnDoneMessageAnimationFinished(ReportedIssue issue)
        {
            if ( Device.RuntimePlatform == Device.iOS)
            {
                await Task.Delay(2000);
                MessagingCenter.Send(issue, MessengerKeys.GoBackFromReportRequest);
                InitializeDoneMessageContainer();
            }
            else
            {
                this.Animate(new FadeToAnimation
                {
                    Opacity = 0,
                    Easing = EasingType.Linear,
                    Duration = "800",
                    Delay = 2000
                }, () =>
                {
                    MessagingCenter.Send(issue, MessengerKeys.GoBackFromReportRequest);
                });
            }
        }

        private void InitializeDoneMessageContainer()
        {
            DoneMessageContainer.Opacity = 0;
            DoneMessageContainer.TranslationY = 1000;
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            UwpIncidentSelector.IsVisible = Device.Idiom == TargetIdiom.Desktop;
            IncidentSelector.IsVisible = Device.Idiom != TargetIdiom.Desktop;

            if (Device.Idiom == TargetIdiom.Desktop)
            {
                LabelTitle.Margin = new Thickness(6, 20, 6, 0);
                LabelDescription.Margin = new Thickness(6, 20, 6, 0);
                //GridRow1.Height = new GridLength(0, GridUnitType.Auto);
                GridRow2.Height = new GridLength(0, GridUnitType.Auto);
                GridRow3.Height = new GridLength(0, GridUnitType.Auto);
                SendIncident.Margin = new Thickness(0, 40, 0, 0);

                BotButtonContainer.HorizontalOptions = LayoutOptions.Center;
                BotButtonContainer.WidthRequest = 620;
                BotButtonContainer.Margin = new Thickness(0, 200, 0, 0);
            }
            else if (Device.RuntimePlatform == Device.UWP)
            {
                LabelTitle.Margin = new Thickness(6, 0, 6, 0);
                LabelDescription.Margin = new Thickness(6, 0, 6, 0);
                //GridRow1.Height = new GridLength(4, GridUnitType.Star);
                GridRow2.Height = new GridLength(3, GridUnitType.Star);
                GridRow3.Height = new GridLength(2, GridUnitType.Star);
                SendIncident.Margin = new Thickness(12, 0, 12, 12);
            }
        }

        private void OnSizeChanged(object sender, EventArgs e)
        {
            if (Height < 700)
            {
                BotButtonContainer.Margin = new Thickness(0, 0, 0, 0);
            }
            else
            {
                BotButtonContainer.Margin = new Thickness(0, 200, 0, 0);
            }
        }
    }
}