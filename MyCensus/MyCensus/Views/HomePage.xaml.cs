using MyCensus.Utils;
using Xamarin.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Naxam.Controls.Forms;
using MyCensus.Controls;
using MyCensus.ViewModels;
using MyCensus.ViewModels.Base;
using MyCensus.Services.Interfaces;

namespace MyCensus.Views
{
    public partial class HomePage : ContentPage
    {
        private const int ScrollMinLimit = 0;
        private const int ScrollMaxLimit = 190;

        public HomePage()
        {
            //DependencyService.Get<IToolbarItemBadgeService>().SetBadge(this, bar, "3", Color.Red, Color.White);
            InitializeComponent();
            var bar = new ToolbarItem("\uf003", "", () =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    DisplayAlert("提示", "抱歉,功能稍后开放...", "取消");
                });
            });
            this.ToolbarItems.Add(bar);
            this.mainScrollView.Scrolled += ScrollView_Scrolled;
        }

        private void ScrollView_Scrolled(object sender, ScrolledEventArgs e)
        {
            var val = MathHelper.ReMap(e.ScrollY, ScrollMinLimit, ScrollMaxLimit, 1, 0);

            this.infoPanel.Scale = val;
            this.infoPanel.Opacity = val;
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            //SuggestionsList.ListOrientation = Device.Idiom == TargetIdiom.Phone ? StackOrientation.Vertical : StackOrientation.Horizontal;
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            //DependencyService.Get<IToolbarItemBadgeService>().SetBadge(this, MapSetting, "3", Color.Red, Color.White);
        }

        protected override void OnAppearing()
        {

            //this.MyMessage.Icon = "fa-envelope";
            base.OnAppearing();
        }
        protected override void OnDisappearing()
        {
          
            base.OnDisappearing();
        }
    }
}
