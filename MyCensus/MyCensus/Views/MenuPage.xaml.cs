﻿using Prism;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyCensus.Views
{
    [XamlCompilation(XamlCompilationOptions.Skip)]
    public partial class MenuPage : ContentPage
    {
        public MenuPage()
        {
            InitializeComponent();
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            //LogoutButton.Margin = Device.Idiom == TargetIdiom.Desktop ? new Thickness(14, 0, 0, 0) : new Thickness(14, 0, 0, 14);
        }


        protected override bool OnBackButtonPressed()
        {
            //if (Device.OS == TargetPlatform.Android)
            //    DependencyService.Get<IAndroidMethods>().CloseApp();
            


            return base.OnBackButtonPressed();
        }
    }
}
