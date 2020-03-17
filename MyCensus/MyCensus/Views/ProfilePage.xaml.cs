﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using MyCensus.Controls;

namespace MyCensus.Views
{
    public partial class ProfilePage : ContentPage
    {
        public ProfilePage()
        {

            CustomNavigationPage.SetTitleIcon(this, "icon.png");
            CustomNavigationPage.SetHasNavigationBar(this, true);
            CustomNavigationPage.SetHasBackButton(this, true);

            CustomNavigationPage.SetTitleMargin(this, new Thickness(0, 0, 5, 0));
            CustomNavigationPage.SetTitleColor(this, Color.White);
            CustomNavigationPage.SetSubtitleColor(this, Color.White);
            CustomNavigationPage.SetTitlePosition(this, CustomNavigationPage.TitleAlignment.Start);
            CustomNavigationPage.SetTitleFont(this, Font.SystemFontOfSize(NamedSize.Large));
            CustomNavigationPage.SetHasShadow(this, true);
            CustomNavigationPage.SetTitlePadding(this, new Thickness(0, 0, 0, 0));
            //渐变从左到右
            CustomNavigationPage.SetGradientColors(this, new Tuple<Color, Color>(Color.FromHex("#5178bd"), Color.FromHex("#7fadf7")));
            CustomNavigationPage.SetGradientDirection(this, CustomNavigationPage.GradientDirection.RightToLeft);


            InitializeComponent();

            if (Device.RuntimePlatform != Device.iOS)
            {
                ToolbarItems.Remove(LogoutToolbarItem);
            }
        }
    }
}