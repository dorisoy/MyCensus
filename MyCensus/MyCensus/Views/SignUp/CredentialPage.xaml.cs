﻿using MyCensus.Animations;
using MyCensus.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyCensus.Views.SignUp
{
    [XamlCompilation(XamlCompilationOptions.Skip)]
    public partial class CredentialPage : ContentView
    {
        public CredentialPage()
        {
            InitializeComponent();

            var cloudCard1Animation = this.Resources["CloudCard1Animation"] as StoryBoard;

            if (cloudCard1Animation != null)
            {
                CloudCard1.Animate(cloudCard1Animation);
            }

            var cloudCard2Animation = this.Resources["CloudCard2Animation"] as StoryBoard;

            if (cloudCard2Animation != null)
            {
                CloudCard2.Animate(cloudCard2Animation);
            }
        }
    }
}
