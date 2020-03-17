using System;
using System.Threading;
//using System.Timers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MyCensus.Models;
using MyCensus.Controls;
using MyCensus.Services.Interfaces;
using Plugin.Geolocator;
using MyCensus.ViewModels.Base;
using MyCensus.DataServices.Interfaces;
using MyCensus.Cache;
using MyCensus.DataServices;
using MyCensus.Models.Census;

namespace MyCensus.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewLocation : ContentPage
    {

        public IEnumerable<Option> Options;
        public EventHandler<string> Value;

        public int CurrentId { get; set; }
        public ViewLocation()
        {
            Title = "定位当前位置";
            InitializeComponent();

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


            //DependencyService.Get<IToolbarItemBadgeService>().SetBadge(this, MapSetting, "3", Color.Red, Color.White);
            //108.941338,34.364438
            map.ShowCompass = true;
            map.IsVisible = true;
            map.Loaded += Map_Loaded;


        }

        private void Map_Loaded(object sender, EventArgs e)
        {
            //开始定位服务,标注网点
            map.LocationService.Start(new List<Option>());
        }


        protected override void OnAppearing()
        {
            SubscribeToMessages();
            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            UnsubscribeToMessages();
            base.OnDisappearing();
        }

        private void SubscribeToMessages()
        {
            MessagingCenter.Subscribe<Coordinate>(this, "ViewLocationMap", (position) =>
            {
                try
                {
                    var coordinate = new Coordinate(position.Latitude, position.Longitude);
                    map.LocationService.AnimateTo(coordinate);
                }
                catch (Exception ex)
                { }

            });
        }

        private void UnsubscribeToMessages()
        {
            MessagingCenter.Unsubscribe<Coordinate>(this, "ViewLocationMap");
        }

    }
}
