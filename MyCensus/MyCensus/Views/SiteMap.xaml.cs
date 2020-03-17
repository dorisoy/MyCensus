using MyCensus.Cache;
using MyCensus.Controls;
using MyCensus.DataServices.Interfaces;
using MyCensus.Models;
using System;
//using System.Timers;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyCensus.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SiteMap : ContentPage
    {

        public IEnumerable<Option> Options;
        public EventHandler<string> Value;

        //Timer _dispatcherTimer;
        //计数
        //int sec = 10;

        public SiteMap()
        {

            InitializeComponent();

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


            //tick为执行防范
            //TimerCallback timerDelegate = new TimerCallback(Tick);
            //_dispatcherTimer = new Timer(timerDelegate, null, 0, 1000);


            Title = "定位当前位置";
            //http://www.cnblogs.com/jtang/p/4698496.html
            //http://www.cnblogs.com/loda7023link/p/6443225.html

            var bar = new ToolbarItem("\uf013", "", () =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    //DisplayAlert("提示", "抱歉,功能稍后开放...", "取消");
                });
            });

            ToolbarItems.Add(bar);

            //DependencyService.Get<IToolbarItemBadgeService>().SetBadge(this, MapSetting, "3", Color.Red, Color.White);
            //108.941338,34.364438

            map.ShowCompass = true;
            map.IsVisible = true;
            map.Loaded += Map_Loaded;
        }


        private void Map_Loaded(object sender, EventArgs e)
        {
            InitLocationService();
        }


        //public void Tick(object state)
        //{
        //    Device.BeginInvokeOnMainThread(() =>
        //    {
        //        if (sec > 0)
        //        {
        //            //smsbt.Text = sec.ToString() + "秒可重发";
        //            sec--;
        //        }
        //        else
        //        {
        //            _dispatcherTimer.Dispose();
        //            sec = 10;
        //            //smsbt.Text = "获取验证码";
        //            //Refresh();
        //            AutoPostion();
        //        }
        //    });
        //}

        public void InitLocationService()
        {
            //位置更新
            map.LocationService.LocationUpdated += (_, e) =>
            {
                //System.Diagnostics.Debug.WriteLine("LocationUpdated: " + e.Direction);
                //在地图上标出我所处的位置
                //map.LocationService.AnimateTo(e.Coordinate);
                GlobalSettings.CurrtntCoordinate = e.Coordinate;
                GlobalSettings.EventLatitude = (float)e.Altitude;
                GlobalSettings.EventLongitude = (float)e.Accuracy;
                GlobalSettings.CurrentAddComp = new AddComp()
                {
                    Province = e.Province,
                    City = e.City,
                    District = e.District,
                    Street = e.Street,
                    Address = e.Address
                };
                GlobalSettings.OffMap = true;
                if (map.UserTrackingMode == UserTrackingMode.Follow)
                {
                    if (GlobalSettings.CurrtntCoordinate != null)
                    {
                        map.LocationService.AnimateTo(new Coordinate(GlobalSettings.CurrtntCoordinate.Latitude, GlobalSettings.CurrtntCoordinate.Longitude));
                    }
                }
            };

            //定位失败
            map.LocationService.Failed += (_, e) =>
            {
                //System.Diagnostics.Debug.WriteLine("Location failed: " + e.Message);
                if (e.Message == "62")
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        DisplayAlert("提示", "无法获取有效定位依据导致定位失败，一般是由于手机的原因，处于飞行模式下一般会造成这种结果，可以试着重启手机.", "确定");
                    });

                    GlobalSettings.OffMap = false;
                    map.UserTrackingMode = UserTrackingMode.None;
                    map.ShowUserLocation = true;
                    this.TrackBtnIcon.Icon = "fa-play";
                    this.TrackBtn.Text = "开启";
                    if (map.LocationService.IsStarted)
                        map.LocationService.Stop();
                }
            };

            //获取跟踪并标注
            //Refresh();
            //开始定位服务,标注网点
            map.LocationService.Start(new List<Option>());


            if (GlobalSettings.OffMap || map.LocationService.IsStarted)
            {
                this.TrackBtn.Text = "停止";
                this.TrackBtnIcon.Icon = "fa-pause";
            }

        }


        private void AutoPostion()
        {
            if (map.UserTrackingMode == UserTrackingMode.Follow)
            {
                if (GlobalSettings.CurrtntCoordinate != null)
                {
                    map.LocationService.AnimateTo(new Coordinate(GlobalSettings.CurrtntCoordinate.Latitude, GlobalSettings.CurrtntCoordinate.Longitude));
                }
            }
        }



        /// <summary>
        /// 获取跟踪并标注
        /// </summary>
        private void Refresh()
        {
            try
            {
                var _censusService = App.ResolveService<ICensusService>();
                var _cacheManager = App.ResolveService<ICacheManager>();

                if (_censusService != null)
                {
                    var points = Task.Run(async () =>
                    {

                        var timeLines = new List<TrackTimeLine>();
                        var cacheTimeLine = await _cacheManager.Get<List<TrackTimeLine>>(GlobalSettings.timeLine_key);
                        if (cacheTimeLine != null)
                        {
                            timeLines = cacheTimeLine;
                        }
                        else
                        {
                            timeLines = await _censusService.GetTrackTimeLines();
                            await _cacheManager.Set<List<TrackTimeLine>>(GlobalSettings.timeLine_key, timeLines);
                        }

                        return timeLines;

                    }).Result;

                    var pointList = new List<Option>();
                    if (points != null)
                    {
                        foreach (var p in points)
                        {
                            pointList.Add(new Option() { Title = p.EndPointStorsName, Lat = p.Latitude, Lng = p.Longitude });
                        }
                    }

                    Options = pointList;

                    if (pointList != null && pointList.Count > 0)
                    {
                        if (map.LocationService != null)
                        {

                            //开始定位服务,标注网点
                            map.LocationService.Refresh(pointList);
                        }
                    }
          
                }

            }
            catch (Exception ex)
            {
                //{System.NullReferenceException: Object reference not set to an instance of an object.
                //at MyCensus.Views.SiteMap.Refresh()[0x00116] in E:\Xamarin\MyCensus - v4\MyCensus\MyCensus\MyCensus\Views\SiteMap.xaml.cs:186 }
                System.Diagnostics.Debug.Print(ex.Message);
            }
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
            MessagingCenter.Subscribe<Coordinate>(this, "MyBDLocation", (position) =>
            {
                //DisplayAlert("坐标:", "longitude:" + string.Format("{0:0.0000000}", GlobalSettings.CurrtntCoordinate.Latitude) + "  latitude:" + string.Format("{0:0.0000000}", GlobalSettings.CurrtntCoordinate.Longitude), "确定");
                //System.Diagnostics.Debug.Print("定位---> Latitude:" + string.Format("{0:0.0000000}", position.Latitude) + "  Longitude:" + string.Format("{0:0.0000000}", position.Longitude));

                //map.CompassPosition = new Point(10, 10);
                if (map.ShowUserLocation)
                {
                    GlobalSettings.OffMap = true;
                    map.UserTrackingMode = UserTrackingMode.Follow;
                    map.ShowUserLocation = false;
                    this.TrackBtn.Text = "停止";
                    this.TrackBtnIcon.Icon = "fa-pause";
                    if (!map.LocationService.IsStarted)
                        map.LocationService.Start(new List<Option>());

                }
                else
                {
                    GlobalSettings.OffMap = false;
                    map.UserTrackingMode = UserTrackingMode.None;
                    map.ShowUserLocation = true;
                    this.TrackBtnIcon.Icon = "fa-play";
                    this.TrackBtn.Text = "开启";
                }
            });
        }

        private void UnsubscribeToMessages()
        {
            // MessengerKeys.TrackLocationUpdated
            MessagingCenter.Unsubscribe<Coordinate>(this, "MyBDLocation");
        }
    }
}