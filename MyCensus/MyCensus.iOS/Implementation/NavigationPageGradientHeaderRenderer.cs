﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;

using CoreAnimation;
using CoreGraphics;
using MyCensus.Controls;
using MyCensus.iOS.Implementation;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(NavigationPageGradientHeader), typeof(NavigationPageGradientHeaderRenderer))]
namespace MyCensus.iOS.Implementation
{
    public class NavigationPageGradientHeaderRenderer : NavigationRenderer
    {
        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            var control = (NavigationPageGradientHeader)this.Element;

            var gradientLayer = new CAGradientLayer();
            gradientLayer.Bounds = NavigationBar.Bounds;
            gradientLayer.Colors = new CGColor[] { control.RightColor.ToCGColor(), control.LeftColor.ToCGColor() };
            gradientLayer.StartPoint = new CGPoint(0.0, 0.5);
            gradientLayer.EndPoint = new CGPoint(1.0, 0.5);

            UIGraphics.BeginImageContext(gradientLayer.Bounds.Size);
            gradientLayer.RenderInContext(UIGraphics.GetCurrentContext());
            UIImage image = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();

            NavigationBar.SetBackgroundImage(image, UIBarMetrics.Default);
        }
    }
}