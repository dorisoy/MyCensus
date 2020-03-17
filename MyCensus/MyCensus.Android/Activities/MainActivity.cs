using System;
using System.Collections;
using System.Collections.Generic;

using Android.App;
using Android.Content.PM;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Graphics;
using Android.Content;
using Android.Util;
//using Android.Locations;
using Java.Lang.Reflect;
using Java.Lang;
using Naxam.Controls.Platform.Droid;

using Plugin.Iconize;
using Plugin.Iconize.Droid.Controls;
using FormsPlugin.Iconize.Droid;


using Prism;
using Prism.Ioc;

using Acr.UserDialogs;

using MyCensus.Services;
using MyCensus.ViewModels;
using MyCensus.DataServices.Base;
using MyCensus.DataServices;
using MyCensus.DataServices.Interfaces;
using MyCensus.Droid.Services;
using MyCensus.Services.Interfaces;
using MyCensus.Controls.DialogKit;
using MyCensus.Controls;
using MyCensus.Droid.BaiDuMapClass;

using Plugin.Media;

using View = Android.Views.View;

//using Com.Tencent.Tencentmap.Mapsdk.Map;
//using Com.Tencent.Mapsdk.Raster.Model;
using LocationManager = Android.Locations.LocationManager;


using Com.Baidu.Mapapi.Map;
using Com.Baidu.Location;
using Com.Baidu.Mapapi;
using Com.Baidu.Mapapi.Model;
using Com.Baidu.Mapapi.Model.Inner;

using MyCensus.Droid.AutoUpdater;
using MyCensus.Cache;
using MyCensus.Database;

namespace MyCensus.Droid
{

    [Activity(Label = "MyCensus", Icon = "@mipmap/icon", Theme = "@style/MainTheme", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity//, IBDLocationListener
    {
        //int count = 1;

        ///// <summary>
        ///// 地图控件
        ///// </summary>
        //private MapView mapView = null;


        //BaiduMap mBaiduMap;

        /// <summary>
        /// MapView的控制器 
        /// </summary>
        //private MapController mapController = null;


        //百度地图的管理类
        //private BMapManager bMapManager = null;


        //在Log中用于标识BMapManager即充当Map的Key作用
        //private String TAG = "bMapManager";


        //为客户端服务选项的主类
        // private LocationClientOption locationClientOption = null;

        //我的图层，用于在地图上标识我所处的位置
        //private MyOverlay myOverlay = null;


        /// <summary>
        /// 定位SDK的核心类
        /// </summary>
        public LocationClient mLocationClient = null;

        /// <summary>
        /// 我所处位置的 纬度
        /// </summary>
        private double myLocationLatitude = 0;

        /// <summary>
        /// 我所处位置的经度
        /// </summary>
        private double myLocationLongitude = 0;



        protected override async void OnCreate(Bundle bundle)
        {
            //MyCensus.BasePage.MyPage.EmulateBackPressed = OnBackPressed;

            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            ////缓存
            //CacheUtils.Init(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal));
            ////缓存保持时间默认10分钟
            //CacheUtils.CACHE_HOLD_TIME = new TimeSpan(0, 10, 0);
            ////最大响应内容缓冲区大小
            //CacheUtils.MAX_RESPONSE_CONTENT_BUFFER_SIZE = 256000;
            ////HTTP客户端请求超时
            //CacheUtils.TIMEOUT = new TimeSpan(0, 1, 30);


            //初始Icon 字体图标
            Plugin.Iconize.Iconize.With(new Plugin.Iconize.Fonts.FontAwesomeModule());

            //初始对话框组件
            UserDialogs.Init(this);

            //初始图片缓存渲染器
            FFImageLoading.Forms.Platform.CachedImageRenderer.Init(true);

            //注册底部Tab导航菜单栏控件
            SetupBottomTabs();

            base.OnCreate(bundle);

            //初台化百度地图
            //SDKInitializer.Initialize(Application.Context);
            SDKInitializer.Initialize(Application.Context);

            //注册弹出框输入
            Rg.Plugins.Popup.Popup.Init(this, bundle);

            //拍照
            await CrossMedia.Current.Initialize();

            //扫码
            //https://github.com/Redth/ZXing.Net.Mobile
            ZXing.Net.Mobile.Forms.Android.Platform.Init();

            //图片浏览
            Stormlion.PhotoBrowser.Droid.Platform.Init(this);

            global::Xamarin.Forms.Forms.Init(this, bundle);

            //初始IconControls
            IconControls.Init(Resource.Id.toolbar, Resource.Id.sliding_tabs);


            //版本更新
            OperatingSystemVersionProvider.Init(this);

            //LoadApplication(new App());
            LoadApplication(new App(new AndroidInitializer()));

        }


        public void TestAlert(Activity activity, string msg)
        {
            Toast.MakeText(this, "软件更新完毕", ToastLength.Long).Show();
        }


        private void StartLocationService()
        {
            LocationClientOption option = new LocationClientOption();
            option.SetLocationMode(LocationClientOption.LocationMode.HightAccuracy);//可选，默认高精度，设置定位模式，高精度，低功耗，仅设备  
            option.CoorType = "bd09ll";//可选，默认gcj02，设置返回的定位结果坐标系  
            int span = 1000;
            option.ScanSpan = span;//可选，默认0，即仅定位一次，设置发起定位请求的间隔需要大于等于1000ms才是有效的  
            option.SetIsNeedAddress(true);//可选，设置是否需要地址信息，默认不需要  
            option.OpenGps = true;//可选，默认false,设置是否使用gps  
            option.LocationNotify = true;//可选，默认false，设置是否当GPS有效时按照1S/1次频率输出GPS结果  
            option.SetIsNeedLocationDescribe(true);//可选，默认false，设置是否需要位置语义化结果，可以在BDLocation.getLocationDescribe里得到，结果类似于“在北京天安门附近”  
            option.SetIsNeedLocationPoiList(true);//可选，默认false，设置是否需要POI结果，可以在BDLocation.getPoiList里得到  
            option.SetIgnoreKillProcess(false);//可选，默认true，定位SDK内部是一个SERVICE，并放到了独立进程，设置是否在stop的时候杀死这个进程，默认不杀死    
            option.SetIgnoreCacheException(false);//可选，默认false，设置是否收集CRASH信息，默认收集  
            option.EnableSimulateGps = false;//可选，默认false，设置是否需要过滤GPS仿真结果，默认需要  
            mLocationClient.LocOption = option;
            mLocationClient.Start();
        }


        ///// <summary>
        ///// 实现位置监听，实时缓存我的位置
        ///// </summary>
        ///// <param name="location"></param>
        //public void OnReceiveLocation(BDLocation location)
        //{
        //    //在地图上标出我所处的位置
        //    markLocationOnMap(location);
        //    //用于显示我所处位置的详细信息
        //    showMyLocation(location);
        //}


        /// <summary>
        /// 在地图上标出我所处的位置
        /// </summary>
        /// <param name="bdLocation"></param>
        private void markLocationOnMap(BDLocation bdLocation)
        {
            //myLocationLatitude = bdLocation.Latitude;
            //myLocationLongitude = bdLocation.Longitude;
            GeoPoint geoPoint = new GeoPoint((int)(myLocationLatitude * 1E6), (int)(myLocationLongitude * 1E6));

            //GlobalSettings.EventLatitude = (float)myLocationLatitude;
            //GlobalSettings.EventLongitude = (float)myLocationLongitude;

            //mapController.setZoom(16);
            //Bitmap bitmap = BitmapFactory.DecodeResource(GetResources(), R.drawable.point_where);
            //myOverlay = new MyOverlay(null, mapView, bitmap, geoPoint, size);
            //myOverlay.addMyOverlay(myOverlay, bitmap, geoPoint);
            ////mapController.animateTo(geoPoint);

            var pint = new Plugin.Geolocator.Abstractions.Position();
            pint.Latitude = bdLocation.Latitude;
            pint.Longitude = bdLocation.Longitude;
            Xamarin.Forms.MessagingCenter.Send(pint, "MyBDLocation");

        }


        /// <summary>
        /// 用于显示我所处位置的详细信息，不过为了使得界面
        /// 更人性化，这里采用先使得TextView不显示，待用户
        /// 出发相应Menu菜单要求显示时才会显示。
        /// </summary>
        /// <param name="bdLocation"></param>
        private void showMyLocation(BDLocation location)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("time : ");
            sb.Append(location.Time);
            sb.Append("\nerror code : ");
            sb.Append(location.LocType);
            sb.Append("\nlatitude : ");
            sb.Append(location.Latitude);
            sb.Append("\nlontitude : ");
            sb.Append(location.Longitude);
            sb.Append("\nradius : ");
            sb.Append(location.Radius);
            if (location.LocType == BDLocation.TypeGpsLocation)
            {// GPS定位结果  
                sb.Append("\nspeed : ");
                sb.Append(location.Speed);// 单位：公里每小时  
                sb.Append("\nsatellite : ");
                sb.Append(location.SatelliteNumber);
                sb.Append("\nheight : ");
                sb.Append(location.Altitude);// 单位：米  
                sb.Append("\ndirection : ");
                sb.Append(location.Direction);// 单位度  
                sb.Append("\naddr : ");
                sb.Append(location.AddrStr);
                sb.Append("\ndescribe : ");
                sb.Append("gps定位成功");
            }
            else if (location.LocType == BDLocation.TypeNetWorkLocation)
            {// 网络定位结果  
                sb.Append("\naddr : ");
                sb.Append(location.AddrStr);
                //运营商信息  
                sb.Append("\noperationers : ");
                sb.Append(location.Operators);
                sb.Append("\ndescribe : ");
                sb.Append("网络定位成功");
            }
            else if (location.LocType == BDLocation.TypeOffLineLocation)
            {// 离线定位结果  
                sb.Append("\ndescribe : ");
                sb.Append("离线定位成功，离线定位结果也是有效的");
            }
            else if (location.LocType == BDLocation.TypeServerError)
            {
                sb.Append("\ndescribe : ");
                sb.Append("服务端网络定位失败，可以反馈IMEI号和大体定位时间到loc-bugs@baidu.com，会有人追查原因");
            }
            else if (location.LocType == BDLocation.TypeNetWorkException)
            {
                sb.Append("\ndescribe : ");
                sb.Append("网络不同导致定位失败，请检查网络是否通畅");
            }
            else if (location.LocType == BDLocation.TypeCriteriaException)
            {
                sb.Append("\ndescribe : ");
                sb.Append("无法获取有效定位依据导致定位失败，一般是由于手机的原因，处于飞行模式下一般会造成这种结果，可以试着重启手机");
            }
            sb.Append("\nlocationdescribe : ");
            sb.Append(location.LocationDescribe);// 位置语义化信息  
            System.Collections.Generic.IList<Poi> list = location.PoiList;// POI数据  
            if (list != null)
            {
                sb.Append("\npoilist size = : ");
                sb.Append(list.Count.ToString());
                foreach (Poi p in list)
                {
                    sb.Append("\npoi= : ");
                    sb.Append(p.Id + " " + p.Name + " " + p.Rank);
                }
            }
            System.Diagnostics.Debug.Write(sb.ToString());
        }

        public override bool OnPrepareOptionsMenu(IMenu menu)
        {
            //BadgeDrawable.SetBadgeCount(this, menu.GetItem(0), 3,Android.Graphics.Color.Red, Android.Graphics.Color.White);
            return base.OnPrepareOptionsMenu(menu);
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            return base.OnCreateOptionsMenu(menu);
        }

        public void AppenIconForMenu(IMenu menu)
        {
            for (int i = 0; i < menu.Size(); i++)
            {
                var item = menu.GetItem(i);
                var iconized = Iconize.FindIconForKey("fa-envelope");
                if (iconized != null)
                {
                    var drawable = new IconDrawable(this, iconized).Color(Android.Graphics.Color.ParseColor("#7fadf7")).SizeDp(30);
                    item.SetIcon(drawable);
                }
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            global::ZXing.Net.Mobile.Android.PermissionsHandler.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }


        public override void OnBackPressed()
        {
            //if (Device.OS == TargetPlatform.Android)
            //    DependencyService.Get<ICloseAppService>().CloseApp();
            //return base.OnBackButtonPressed();
            //Object reference not set to an instance of an object
            //var IsAuthenticated = ViewModelLocator.Instance.Resolve
            //var app = new App(new AndroidInitializer());
            //var _authenticationService = AppContext..Resolve<IAuthenticationService>();
            var _authenticationService = App.ResolveService<IAuthenticationService>();
            //System.Diagnostics.Debug.Print(_authenticationService.IsAuthenticated.ToString());
            if (!_authenticationService.IsAuthenticated)
            {
                var exit = App.ResolveService<ICloseAppService>();
                exit.CloseApp();
            }
        }



        public class AndroidInitializer : IPlatformInitializer
        {
            public void RegisterTypes(IContainerRegistry container)
            {
                //注册服务
                container.Register<ISaveAndLoad, SaveAndLoadService>();
                container.Register<IRequestProvider, RequestProvider>();
                container.Register<ILocationProvider, LocationProvider>();
                container.Register<IMediaPickerService, MediaPickerService>();
                container.Register<IOperatingSystemVersionProvider, OperatingSystemVersionProvider>();
                container.Register<IDialogService, DialogService>();
                container.Register<IAuthenticationService, AuthenticationService>();
                container.Register<IProfileService, ProfileService>();
                container.Register<IRidesService, RidesService>();
                container.Register<IEventsService, EventsService>();
                container.Register<IWeatherService, OpenWeatherMapService>();
                container.Register<IFeedbackService, FeedbackService>();
                container.Register<ICloseAppService, ImpDroidCloseAppService>();
                container.Register<ICensusService, CensusService>();
                container.Register<IToolbarItemBadgeService, ToolbarItemBadgeService>();
                container.Register<IDialogKit, DialogKit>();
                //LocationServiceImpl
                container.Register<ILocationService, LocationServiceImpl>();
                container.Register<IImageEditor, ImageEditor>();
                container.Register<IEditableImage, EditableImage>();
                //UpdateService
                container.Register<IUpdateService, UpdateService>();
                //INetworkConnection
                container.Register<INetworkConnection, NetworkConnection>();
                //ICacheManager
                container.Register<ICacheManager, CacheManager>();
                // SQLiteClient : IDatabaseConnection
                container.RegisterSingleton<IDatabaseConnection, SQLiteClient>();
                //ISettingService
                container.RegisterSingleton<ISettingService, SettingService>();
            }
        }

        private void SetupBottomTabs()
        {
            var stateList = new Android.Content.Res.ColorStateList(
                new int[][] {
                    new int[] { Android.Resource.Attribute.StateChecked
                },
                new int[] { Android.Resource.Attribute.StateEnabled
                }
                },
                new int[]
                {
                   Android.Graphics.Color.ParseColor("#7fadf7"), //Selected
                   Android.Graphics.Color.ParseColor("#9b9797") //Normal
	            });

            //new Color(Resource.Color.colorPrimary) //Normal
            //{StaticResource Primary}
            //new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFD2D2D2"));
            //System.Drawing.Color sdColor2 = Xamarin.Forms.Color.FromHex("FF6A00");
            BottomTabbedRenderer.BackgroundColor = Android.Graphics.Color.ParseColor("#e5e0e0");
            BottomTabbedRenderer.FontSize = 11f;
            BottomTabbedRenderer.IconSize = 16;
            BottomTabbedRenderer.ItemTextColor = stateList;
            BottomTabbedRenderer.ItemIconTintList = stateList;
            BottomTabbedRenderer.Typeface = Typeface.CreateFromAsset(this.Assets, "iconize-fontawesome.ttf");
            //BottomTabbedRenderer.ItemBackgroundResource = Resource.Drawable.bnv_selector;
            BottomTabbedRenderer.ItemSpacing = 2;
            //BottomTabbedRenderer.ItemPadding = new Xamarin.Forms.Thickness(6);
            BottomTabbedRenderer.BottomBarHeight = 50;
            BottomTabbedRenderer.ItemAlign = ItemAlignFlags.Center;
            BottomTabbedRenderer.ShouldUpdateSelectedIcon = true;
            BottomTabbedRenderer.MenuItemIconSetter = (menuItem, iconSource, selected) =>
            {
                var iconized = Iconize.FindIconForKey(iconSource.File);
                if (iconized == null)
                {
                    BottomTabbedRenderer.DefaultMenuItemIconSetter.Invoke(menuItem, iconSource, selected);
                    return;
                }
                var drawable = new IconDrawable(this, iconized).Color(selected ? Android.Graphics.Color.ParseColor("#7fadf7") : Android.Graphics.Color.ParseColor("#9b9797")).SizeDp(30);
                menuItem.SetIcon(drawable);
            };
        }


        #region LayoutInflaterFactory


        static Class ActionMenuItemViewClass = null;
        static Constructor ActionMenuItemViewConstructor = null;
        static Typeface typeface = null;
        public static Typeface Typeface
        {
            get
            {
                if (typeface == null)
                    typeface = Typeface.CreateFromAsset(Xamarin.Forms.Forms.Context.ApplicationContext.Assets, "fontawesome.ttf");

                return typeface;
            }
        }
        public override View OnCreateView(string name, Context context, IAttributeSet attrs)
        {
            if (name.Equals("android.support.v7.view.menu.ActionMenuItemView", StringComparison.InvariantCultureIgnoreCase))
            {
                System.Diagnostics.Debug.WriteLine(name);
                var customLoginIfNeeded = CreateCustomToolbarItem(name, context, attrs);
                if (customLoginIfNeeded != null)
                    return customLoginIfNeeded;
            }

            return base.OnCreateView(name, context, attrs);
        }
        public override View OnCreateView(View parent, string name, Context context, IAttributeSet attrs)
        {
            if (name.Equals("android.support.v7.view.menu.ActionMenuItemView", StringComparison.InvariantCultureIgnoreCase))
            {
                System.Diagnostics.Debug.WriteLine(name);
                var customLoginIfNeeded = CreateCustomToolbarItem(name, context, attrs);
                if (customLoginIfNeeded != null)
                    return customLoginIfNeeded;
            }

            return base.OnCreateView(parent, name, context, attrs);
        }
        private View CreateCustomToolbarItem(string name, Context context, IAttributeSet attrs)
        {
            // android.support.v7.widget.Toolbar
            // android.support.v7.view.menu.ActionMenuItemView
            View view = null;

            try
            {
                if (ActionMenuItemViewClass == null)
                    ActionMenuItemViewClass = ClassLoader.LoadClass(name);
            }
            catch (ClassNotFoundException ex)
            {
                return null;
            }

            if (ActionMenuItemViewClass == null)
                return null;

            if (ActionMenuItemViewConstructor == null)
            {
                try
                {
                    ActionMenuItemViewConstructor = ActionMenuItemViewClass.GetConstructor(new Class[] {
                            Class.FromType(typeof(Context)),
                                 Class.FromType(typeof(IAttributeSet))
                        });
                }
                catch (SecurityException)
                {
                    return null;
                }
                catch (NoSuchMethodException)
                {
                    return null;
                }
            }
            if (ActionMenuItemViewConstructor == null)
                return null;

            try
            {
                Java.Lang.Object[] args = new Java.Lang.Object[] { context, (Java.Lang.Object)attrs };
                view = (View)(ActionMenuItemViewConstructor.NewInstance(args));
            }
            catch (IllegalArgumentException)
            {
                return null;
            }
            catch (InstantiationException)
            {
                return null;
            }
            catch (IllegalAccessException)
            {
                return null;
            }
            catch (InvocationTargetException)
            {
                return null;
            }
            if (null == view)
                return null;

            View v = view;
            v.SetPadding(0, 0, 0, 0);

            new Handler().Post(() =>
            {

                try
                {
                    if (v is LinearLayout)
                    {
                        var ll = (LinearLayout)v;
                        for (int i = 0; i < ll.ChildCount; i++)
                        {
                            var button = ll.GetChildAt(i) as Android.Widget.Button;

                            System.Diagnostics.Debug.WriteLine("------button------->" + button.Text);

                            if (button != null)
                            {
                                button.SetPadding(0, 0, 0, 0);
                                var title = button.Text;

                                if (!string.IsNullOrEmpty(title) && title.Length == 1)
                                {
                                    button.SetTypeface(Typeface, TypefaceStyle.Normal);
                                }
                            }
                        }
                    }
                    else if (v is TextView)
                    {
                        var tv = (TextView)v;
                        tv.SetPadding(0, 0, 0, 0);
                        string title = tv.Text;


                        //System.Diagnostics.Debug.WriteLine("-----title-------->" + title);

                        if (!string.IsNullOrEmpty(title) && title.Length == 1)
                        {
                            tv.SetTextSize(ComplexUnitType.Sp, 20);
                            tv.SetTypeface(Typeface, TypefaceStyle.Normal);
                        }
                    }
                }
                catch (ClassCastException)
                {
                }
            });

            return view;
        }


        #endregion
    }
}

