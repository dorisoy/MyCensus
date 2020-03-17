using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Graphics;
using AG = Android.Graphics;

using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

using MyCensus.Controls;
using MyCensus.Views;
using MyCensus.Droid.Implementations;
using MyCensus.Droid.BaiDuMapClass;

using Com.Baidu.Location;
using Com.Baidu.Mapapi.Map;
using Com.Baidu.Mapapi.Model;
using Com.Baidu.Mapapi.Map.Offline;
using Com.Baidu.Mapapi.Common;
//using Com.Baidu.Mapsdkplatform.Comapi;
using Com.Baidu.Android.Bbalbs.Common.Util;
using Com.Baidu.Mapapi.Model.Inner;


//ExportRenderer向自定义渲染器类添加属性以指定它将用于渲染Xamarin.Forms自定义地图。
//此属性用于向Xamarin.Forms注册自定义渲染器。第一个是用来承载的界面，第二个是要引用的界面
[assembly: ExportRenderer(typeof(MyBaiduMaps), typeof(BaidiuMapRenderer))]
namespace MyCensus.Droid.Implementations
{
    public class BaidiuMapRenderer : ViewRenderer<MyBaiduMaps, MapView>, BaiduMap.IOnMapLoadedCallback  //PageRenderer
    {


        //protected MapView mapView => Control;

        protected MyBaiduMaps Map => Element;


        //private BaiduMap baiduMap;

        /// <summary>
        /// 地图试图页面
        /// </summary>
        //private SiteMap myAMapPage;


        /// <summary>
        /// 地图视图
        /// </summary>
        private MapView mapView => Control;

        /// <summary>
        /// 布局
        /// </summary>
        private LinearLayout layout;


        public BaidiuMapRenderer(Context context) : base(context)
        {
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (null != Element)
                {
                    //Map.Pins.Clear();
                    ((LocationServiceImpl)Map.LocationService).Unregister();
                }

                //pinImpl.Unregister(Map);
                //polylineImpl.Unregister(Map);
                //polygonImpl.Unregister(Map);
                //circleImpl.Unregister(Map);

                //mapView.Map.Clear();
                //mapView.Map.MapClick -= OnMapClick;
                //mapView.Map.MapPoiClick -= OnMapPoiClick;
                //mapView.Map.MapDoubleClick -= OnMapDoubleClick;
                //mapView.Map.MapLongClick -= OnMapLongClick;
                //mapView.Map.MarkerClick -= OnMarkerClick;
                //mapView.Map.MarkerDragStart -= OnMarkerDragStart;
                //mapView.Map.MarkerDrag -= OnMarkerDrag;
                //mapView.Map.MarkerDragEnd -= OnMarkerDragEnd;
                //mapView.Map.MapStatusChangeFinish -= MapStatusChangeFinish;
                mapView.Map.SetOnMapLoadedCallback(null);

                mapView.OnDestroy();
            }

            System.Diagnostics.Debug.WriteLine("Disposing: " + disposing);
            base.Dispose(disposing);
        }


        public override SizeRequest GetDesiredSize(int widthConstraint, int heightConstraint)
        {
            return new SizeRequest(new Size(Context.ToPixels(0), Context.ToPixels(0)));
        }


        /*
      protected override void OnElementChanged(ElementChangedEventArgs<Page> e)
      {

          base.OnElementChanged(e);
          myAMapPage = e.NewElement as SiteMap;
          layout = new LinearLayout(this.Context);
          this.AddView(layout);

          mapView = new MapView(this.Context)
          {
              LayoutParameters = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent)
          };

          //设置logo到屏幕底部中心
          mapView.LogoPosition = LogoPosition.LogoPostionleftBottom;

          //BaiduMap
          baiduMap = mapView.Map;
          ////普通地图
          baiduMap.MapType = BaiduMap.MapTypeNormal;
          //设置实时路况开启
          baiduMap.TrafficEnabled = true;
          baiduMap.MyLocationEnabled = true;


          //设置logo到屏幕底部中心
          //mapView.LogoPosition = LogoPosition.LogoPostionleftBottom;
          foreach (var item in myAMapPage.Options)
          {

              var point = new LatLng(item.Lat, item.Lat);
              //使地图移动到当前位置
              MapStatus mMapStatus = new MapStatus.Builder()
                  .Target(point)
                  .Zoom(12)
                  .Build();

              //创建当前位置Marker图标  
              BitmapDescriptor bitmap = BitmapDescriptorFactory.FromResource(Resource.Drawable.red_location);
              //构建MarkerOption，用于在地图上添加Marker  
              OverlayOptions option = new MarkerOptions()
                  .InvokePosition(point)
                  .InvokeIcon(bitmap)
                  .InvokeTitle(item.Title)
                  .Anchor(0.5f, 0.5f)
                  .Draggable(true)
                  .InvokeIcon(bitmap);

              //在地图上添加Marker，并显示  
              baiduMap.AddOverlay(option);

          }

          layout.AddView(mapView);

      }
      */

        protected override void OnElementChanged(ElementChangedEventArgs<MyBaiduMaps> e)
        {
            base.OnElementChanged(e);
            layout = new LinearLayout(this.Context);
            //this.AddView(layout);

            if (null == Control)
            {
                var mapView = new MapView(this.Context)
                {
                    LayoutParameters = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent)
                };

                SetNativeControl(mapView);
            }

            if (null != e.OldElement)
            {
                var oldMap = e.OldElement;
                // oldMap.Pins.Clear();
                ((LocationServiceImpl)oldMap.LocationService).Unregister();
                MapView oldMapView = Control;
                oldMapView.Map.Clear();
                //oldMapView.Map.MapClick -= OnMapClick;
                //oldMapView.Map.MapPoiClick -= OnMapPoiClick;
                //oldMapView.Map.MapDoubleClick -= OnMapDoubleClick;
                //oldMapView.Map.MapLongClick -= OnMapLongClick;
                //oldMapView.Map.MarkerClick -= OnMarkerClick;
                //oldMapView.Map.MarkerDragStart -= OnMarkerDragStart;
                //oldMapView.Map.MarkerDrag -= OnMarkerDrag;
                //oldMapView.Map.MarkerDragEnd -= OnMarkerDragEnd;
                //oldMapView.Map.MapStatusChangeFinish -= MapStatusChangeFinish;
                oldMapView.Map.SetOnMapLoadedCallback(null);
                oldMapView.OnDestroy();
            }

            if (null != e.NewElement)
            {
                //baiduMap = mapView.Map;

                Map.LocationService = new LocationServiceImpl(mapView, Context);
                // mapView.OnResume
                //mapView.Map.MapClick += OnMapClick;
                //mapView.Map.MapPoiClick += OnMapPoiClick;
                //mapView.Map.MapDoubleClick += OnMapDoubleClick;
                //mapView.Map.MapLongClick += OnMapLongClick;
                //mapView.Map.MarkerClick += OnMarkerClick;
                //mapView.Map.MarkerDragStart += OnMarkerDragStart;
                //mapView.Map.MarkerDrag += OnMarkerDrag;
                //mapView.Map.MarkerDragEnd += OnMarkerDragEnd;
                //mapView.Map.MapStatusChangeFinish += MapStatusChangeFinish;

                mapView.Map.SetOnMapLoadedCallback(this);
              
                ////缩放
                //mapView.ShowZoomControls(true);
                //mapView.SetZoomControlsPosition(new AG.Point(50, 50));
                ////缩放控件
                //mapView.ShowScaleControl(true);
                //mapView.SetScaleControlPosition(new AG.Point(10, 10));


                UpdateMapType();
                UpdateUserTrackingMode();
                UpdateShowUserLocation();

                UpdateShowCompass();
                UpdateCompassPosition();

                UpdateZoomLevel();
                UpdateMinZoomLevel();
                UpdateMaxZoomLevel();

                //UpdateCenter();
                UpdateShowScaleBar();
                UpdateShowZoomControl();

                //pinImpl.Unregister(e.OldElement);
                //pinImpl.Register(Map, mapView);

                //polylineImpl.Unregister(e.OldElement);
                //polylineImpl.Register(Map, mapView);

                //polygonImpl.Unregister(e.OldElement);
                //polygonImpl.Register(Map, mapView);

                //circleImpl.Unregister(e.OldElement);
                //circleImpl.Register(Map, mapView);

              

            }


            //layout.AddView(mapView);
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if ("Height" == e.PropertyName)
            {
                System.Diagnostics.Debug.WriteLine("Height = " + Map.Height);
                return;
            }

            if ("Width" == e.PropertyName)
            {
                System.Diagnostics.Debug.WriteLine("Width = " + Map.Width);
                return;
            }

            if (MyBaiduMaps.MapTypeProperty.PropertyName == e.PropertyName)
            {
                System.Diagnostics.Debug.WriteLine("MapType = " + Map.MapType);
                UpdateMapType();
                return;
            }

            if (MyBaiduMaps.UserTrackingModeProperty.PropertyName == e.PropertyName)
            {
                System.Diagnostics.Debug.WriteLine("UserTrackingMode = " + Map.UserTrackingMode);
                UpdateUserTrackingMode();
                return;
            }

            if (MyBaiduMaps.ShowUserLocationProperty.PropertyName == e.PropertyName)
            {
                System.Diagnostics.Debug.WriteLine("ShowUserLocation = " + Map.ShowUserLocation);
                UpdateShowUserLocation();
                return;
            }

            if (MyBaiduMaps.ShowCompassProperty.PropertyName == e.PropertyName)
            {
                System.Diagnostics.Debug.WriteLine("ShowCompass = " + Map.ShowCompass);
                UpdateShowCompass();
                return;
            }

            if (MyBaiduMaps.CompassPositionProperty.PropertyName == e.PropertyName)
            {
                System.Diagnostics.Debug.WriteLine("CompassPosition = " + Map.CompassPosition);
                UpdateCompassPosition();
                return;
            }

            if (MyBaiduMaps.ZoomLevelProperty.PropertyName == e.PropertyName)
            {
                System.Diagnostics.Debug.WriteLine("ZoomLevel = " + Map.ZoomLevel);
                UpdateZoomLevel();
                return;
            }

            if (MyBaiduMaps.MinZoomLevelProperty.PropertyName == e.PropertyName)
            {
                System.Diagnostics.Debug.WriteLine("MinZoomLevel = " + Map.MinZoomLevel);
                UpdateMinZoomLevel();
                return;
            }

            if (MyBaiduMaps.MaxZoomLevelProperty.PropertyName == e.PropertyName)
            {
                System.Diagnostics.Debug.WriteLine("MaxZoomLevel = " + Map.MaxZoomLevel);
                UpdateMaxZoomLevel();
                return;
            }

            //if (MyBaiDuMaps.CenterProperty.PropertyName == e.PropertyName)
            //{
            //    System.Diagnostics.Debug.WriteLine("Center = " + Map.Center);
            //    UpdateCenter();
            //    return;
            //}

            if (MyBaiduMaps.ShowScaleBarProperty.PropertyName == e.PropertyName)
            {
                System.Diagnostics.Debug.WriteLine("ShowScaleBar = " + Map.ShowScaleBar);
                UpdateShowScaleBar();
                return;
            }

            if (MyBaiduMaps.ShowZoomControlProperty.PropertyName == e.PropertyName)
            {
                System.Diagnostics.Debug.WriteLine("ShowZoomControl = " + Map.ShowZoomControl);
                UpdateShowZoomControl();
                return;
            }

            System.Diagnostics.Debug.WriteLine("OnElementPropertyChanged: " + e.PropertyName);
            base.OnElementPropertyChanged(sender, e);
        }


        void UpdateMapType()
        {
            switch (Map.MapType)
            {
                case MapType.None:
                    mapView.Map.MapType = 0;
                    break;

                case MapType.Standard:
                    mapView.Map.MapType = 1;
                    break;

                case MapType.Satellite:
                    mapView.Map.MapType = 2;
                    break;
            }
        }

        void UpdateUserTrackingMode()
        {
            MyLocationConfiguration.LocationMode mode;

            switch (Map.UserTrackingMode)
            {
                default:
                case UserTrackingMode.None:
                    mode = MyLocationConfiguration.LocationMode.Normal;
                    break;

                case UserTrackingMode.Follow:
                    mode = MyLocationConfiguration.LocationMode.Following;
                    break;

                case UserTrackingMode.FollowWithCompass:
                    mode = MyLocationConfiguration.LocationMode.Compass;
                    break;
            }

            mapView.Map.SetMyLocationConfigeration(
                new MyLocationConfiguration(mode, true, null)
            );

            if (UserTrackingMode.FollowWithCompass != Map.UserTrackingMode)
            {
                // 恢复俯视角
                MapStatusUpdate overlook = MapStatusUpdateFactory.NewMapStatus(
                    new MapStatus.Builder(mapView.Map.MapStatus).Rotate(0).Overlook(0).Build()
                );

                mapView.Map.AnimateMapStatus(overlook);
            }
        }

        void UpdateShowUserLocation()
        {
            mapView.Map.MyLocationEnabled = Map.ShowUserLocation;
        }

        void UpdateShowCompass()
        {
            mapView.Map.UiSettings.CompassEnabled = Map.ShowCompass;
        }

        void UpdateCompassPosition()
        {
            mapView.Map.CompassPosition = new AG.Point
            {
                X = (int)Map.CompassPosition.X,
                Y = (int)Map.CompassPosition.Y
            };
        }

        void UpdateZoomLevel()
        {
            mapView.Map.AnimateMapStatus(
                MapStatusUpdateFactory.ZoomTo(Map.ZoomLevel)
            );
        }

        void UpdateMinZoomLevel()
        {
            mapView.Map.SetMaxAndMinZoomLevel(Map.MaxZoomLevel, Map.MinZoomLevel);
        }

        void UpdateMaxZoomLevel()
        {
            mapView.Map.SetMaxAndMinZoomLevel(Map.MaxZoomLevel, Map.MinZoomLevel);
        }


        void UpdateShowScaleBar()
        {
            mapView.ShowScaleControl(Map.ShowScaleBar);
            mapView.SetScaleControlPosition(new AG.Point(10, 10));
        }

        void UpdateShowZoomControl()
        {
            mapView.ShowZoomControls(Map.ShowZoomControl);
            //mapView.SetZoomControlsPosition(new AG.Point(50, 50));

            //ZoomControls zoom_SF = null;
            //for (int i = 0; i < mapView.ChildCount; i++)
            //{
            //    var child = mapView.GetChildAt(i);
            //    if (child is ZoomControls)
            //    {
            //        zoom_SF = (ZoomControls)child;
            //        //mapView.removeViewAt(2);
            //        break;
            //    }
            //}

            //if (zoom_SF != null)
            //    //调整缩放控件的位置
            //    zoom_SF.SetPadding(0, 0, 0, 100);

            //获取mapview中的百度地图图标
            //ImageView iv = (ImageView)mapView.GetChildAt(1);
            //mapView.removeViewAt(1);
            //调整百度地图图标的位置
            //iv.SetPadding(0, 0, 0, 100);
        }


        public void OnMapLoaded()
        {
            // Map.Projection = new ProjectionImpl(mapView);
            mapView.OnResume();
            Map.SendLoaded();

            //指南针位置
            mapView.Map.SetCompassEnable(false);
            //mapView.Map.CompassPosition = new AG.Point(50, 50);
            /// 

            //百度地图API SDK 3.0.0版本以上该方法设置比例尺/缩放控件的位置
            AG.Point point = new AG.Point();
            point.X = 20;
            point.Y = 20;
            mapView.SetZoomControlsPosition(point);


            
        }

        protected override void OnAttachedToWindow()
        {
            mapView.Visibility = ViewStates.Visible;
            mapView.OnResume();
            base.OnAttachedToWindow();
        }
        protected override void OnDetachedFromWindow()
        {
            mapView.Visibility = ViewStates.Invisible;
            base.OnDetachedFromWindow();
        }

      

        protected override void OnLayout(bool changed, int l, int t, int r, int b)
        {
            base.OnLayout(changed, l, t, r, b);
            var msw = Android.Views.View.MeasureSpec.MakeMeasureSpec(r - l, MeasureSpecMode.Exactly);
            var msh = Android.Views.View.MeasureSpec.MakeMeasureSpec(b - t, MeasureSpecMode.Exactly);
            layout.Measure(msw, msh);
            layout.Layout(0, 0, r - l, b - t);
        }
    }


    /*
            protected override void OnElementChanged(ElementChangedEventArgs<Page> e)
            {

                base.OnElementChanged(e);
                myAMapPage = e.NewElement as TencentMapPage;
                layout1 = new LinearLayout(this.Context);
                this.AddView(layout1);

                mapView = new MapView(this.Context)
                {
                    LayoutParameters = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent)
                };
                LatLng SHANGHAI = new LatLng(31.238068, 121.501654);
                mapView.Map.SetCenter(SHANGHAI);

                var pins = myAMapPage.Pins;

                Drawable d = Resources.GetDrawable(Resource.Drawable.red_location);
                Bitmap bitmap = ((BitmapDrawable)d).Bitmap;
                LatLng latLng1;
                foreach (UserTaskEntInfo pin in pins)
                {
                    latLng1 = new LatLng(pin.Longitude ?? 31.238068, pin.Latitude ?? 121.501654);
                    var markOption = new MarkerOptions();
                    markOption.InvokeIcon(new BitmapDescriptor(bitmap));
                    markOption.InvokeTitle(pin.Name);
                    markOption.InvokePosition(latLng1);
                    var fix = mapView.Map.AddMarker(markOption);
                    fix.ShowInfoWindow();
                }
                mapView.Map.SetZoom(15);
                mapView.OnCreate(bundle);
                layout1.AddView(mapView);

            }

            protected override void OnLayout(bool changed, int l, int t, int r, int b)
            {
                base.OnLayout(changed, l, t, r, b);
                var msw = View.MeasureSpec.MakeMeasureSpec(r - l, MeasureSpecMode.Exactly);
                var msh = View.MeasureSpec.MakeMeasureSpec(b - t, MeasureSpecMode.Exactly);
                layout1.Measure(msw, msh);
                layout1.Layout(0, 0, r - l, b - t);
            }
            */
}