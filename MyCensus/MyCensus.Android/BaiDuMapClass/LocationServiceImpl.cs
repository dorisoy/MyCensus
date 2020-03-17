using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AG = Android.Graphics;

using Com.Baidu.Location;
using MyCensus.Droid.BaiDuMapClass;
using BMap = Com.Baidu.Mapapi.Map;
using MyCensus.Controls;

using Com.Baidu.Mapapi.Map;
using Com.Baidu.Mapapi.Model;
using Com.Baidu.Mapapi.Map.Offline;
using Com.Baidu.Mapapi.Common;
//using Com.Baidu.Mapsdkplatform.Comapi;
using Com.Baidu.Android.Bbalbs.Common.Util;
using Com.Baidu.Mapapi.Model.Inner;
using MyCensus.Models;

namespace MyCensus.Droid.BaiDuMapClass
{
    internal class LocationServiceImpl : Java.Lang.Object, IBDLocationListener, ILocationService
    {
        private BMap.MapView mapView;
        private LocationClient native;

        public LocationServiceImpl(BMap.MapView mapView, Context context)
        {
            this.mapView = mapView;
            LocationClientOption option = new LocationClientOption();

            //option.SetLocationMode(LocationClientOption.LocationMode.HightAccuracy);
            //option.CoorType = "bd09ll"; // gcj02 by default
            //option.ScanSpan = 1000; // 0 by default(just once)
            //option.SetIsNeedAddress(false); // false by default
            //option.OpenGps = true; // false by default
            //option.LocationNotify = true; // false by default
            //option.SetIsNeedLocationDescribe(false); // 通过 getLocationDescribe 获取语义化结果
            //option.SetIsNeedLocationPoiList(false); // 通过 getPoiList 获取
            //option.IsIgnoreKillProcess = false;
            //option.IsIgnoreCacheException = false;
            //option.SetEnableSimulateGps(false);
            //option.LocationNotify = true;


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
            option.SetEnableSimulateGps(false);
            option.LocationNotify = true;


            native = new LocationClient(context);
            native.LocOption = option;
            native.RegisterLocationListener(this);

        }

        internal void Unregister()
        {
            native.UnRegisterLocationListener(this);
        }

        public void Start(List<Option> points)
        {
            native.Start();
            stopped = false;

            /*
           var options = new List<MyCensus.Models.Option>()
               {
                   new Option() { Title="雪花啤酒(西安)分公司",Lat=34.114438,Lng=108.111338},
                    new Option() { Title="雪花啤酒(西安)分公司",Lat=34.224438,Lng=108.421338},
                     new Option() { Title="雪花啤酒(西安)分公司",Lat=34.334438,Lng=108.531338},
                   new Option() { Title="雪花啤酒(西安)分公司",Lat=34.464438,Lng=108.741338}
               };

           this.mapView.Map.Clear();
           LatLng latLng = null;
           foreach (var op in options)
           {
               Marker marker;
               latLng = new LatLng(op.Lat, op.Lng);
               BitmapDescriptor bitmap = BitmapDescriptorFactory.FromResource(Resource.Drawable.red_location);
               //构建MarkerOption，用于在地图上添加Marker  
               OverlayOptions option = new MarkerOptions()
                   .InvokePosition(latLng)
                   .InvokeTitle("雪花啤酒(西安)分公司")
                   .Anchor(0.5f, 0.5f)
                   .Draggable(true)
                   .ScaleX(0.5f)
                   .ScaleY(0.5f)
                   .InvokeIcon(bitmap);

               ////在地图上添加Marker，并显示  
               marker = (Marker)this.mapView.Map.AddOverlay(option);

               //Bundle bundle = new Bundle();
               //info必须实现序列化接口
               //bundle.PutSerializable("info", op);
               //marker.se(bundle);
           }

           //将地图显示在最后一个marker的位置
           MapStatusUpdate msu = MapStatusUpdateFactory.NewLatLng(latLng);
           this.mapView.Map.SetMapStatus(msu);
           */


            // 路线
            //LatLng p1 = new LatLng(31.209933, 121.608515);
            //LatLng p2 = new LatLng(30.905841, 121.927665);
            //LatLng p3 = new LatLng(31.049502, 121.432088);
            //LatLng p4 = new LatLng(31.160318, 121.434962);
            //LatLng p5 = new LatLng(34.283806, 117.198051);
            //LatLng p6 = new LatLng(29.545097, 106.568581);
            //LatLng p7 = new LatLng(34.358342, 108.922285);
            //List<LatLng> points = new List<LatLng>();
            //points.Add(p1);
            //points.Add(p2);
            //points.Add(p3);
            //points.Add(p4);
            //points.Add(p5);
            //points.Add(p6);
            //points.Add(p7);

            //OverlayOptions ooPolyline = new PolylineOptions().InvokeWidth(10).InvokePoints(points);
            //mapView.Map.AddOverlay(ooPolyline);

            //this.mapView.Map.Clear();

            //LatLngBounds.Builder builder = new LatLngBounds.Builder();
            //foreach (var point in points)
            //{
            //    var latLng = new LatLng(point.Lat, point.Lng);
            //    builder = builder.Include(latLng);
            //    BitmapDescriptor bitmap = BitmapDescriptorFactory.FromResource(Resource.Drawable.red_location);
            //    //构建MarkerOption，用于在地图上添加Marker  
            //    OverlayOptions option = new MarkerOptions()
            //        .InvokePosition(latLng)
            //        .InvokeTitle(point.Title)
            //        .Anchor(0.5f, 0.5f)
            //        .Draggable(true)
            //        .ScaleX(0.5f)
            //        .ScaleY(0.5f)
            //        .InvokeIcon(bitmap);

            //    ////在地图上添加Marker，并显示  
            //    this.mapView.Map.AddOverlay(option);
            //}

            //LatLngBounds latlngBounds = builder.Build();
            //MapStatusUpdate u = MapStatusUpdateFactory.NewLatLngBounds(latlngBounds, mapView.Width, mapView.Height);
            //mapView.Map.AnimateMapStatus(u);
            //this.mapView.Map.MarkerClick += Map_MarkerClick;
            AnimateTo(new Coordinate(34.364438, 108.941338));
        }




        private void Map_MarkerClick(object sender, BaiduMap.MarkerClickEventArgs e)
        {
            System.Diagnostics.Debug.Print(e.P0.Title);
        }


        public void Refresh(List<Option> points)
        {

            this.mapView.Map.Clear();
            LatLngBounds.Builder builder = new LatLngBounds.Builder();
            foreach (var point in points)
            {
                var latLng = new LatLng(point.Lat, point.Lng);
                builder = builder.Include(latLng);
                BitmapDescriptor bitmap = BitmapDescriptorFactory.FromResource(Resource.Drawable.red_location);
                //构建MarkerOption，用于在地图上添加Marker  
                OverlayOptions option = new MarkerOptions()
                    .InvokePosition(latLng)
                    .InvokeTitle(point.Title)
                    .Anchor(0.5f, 0.5f)
                    .Draggable(true)
                    .ScaleX(0.5f)
                    .ScaleY(0.5f)
                    .InvokeIcon(bitmap);

                ////在地图上添加Marker，并显示  
                this.mapView.Map.AddOverlay(option);
            }

            LatLngBounds latlngBounds = builder.Build();
            MapStatusUpdate u = MapStatusUpdateFactory.NewLatLngBounds(latlngBounds, mapView.Width, mapView.Height);
            mapView.Map.AnimateMapStatus(u);
        }

        /// <summary>
        /// 移动位置
        /// </summary>
        /// <param name="coordinate"></param>
        public void AnimateTo(Coordinate coordinate)
        {

            this.mapView.Map.Clear();

            var point = new LatLng(coordinate.Latitude, coordinate.Longitude);

            //Title="雪花啤酒(西安)分公司",Lat=34.364438,Lng=108.941338
            //创建当前位置Marker图标  
            BitmapDescriptor bitmap = BitmapDescriptorFactory.FromResource(Resource.Drawable.red_location);
            //构建MarkerOption，用于在地图上添加Marker  
            OverlayOptions option = new MarkerOptions()
                .InvokePosition(point)
                .Anchor(0.5f, 0.5f)
                .ScaleX(0.5f)
                .ScaleY(0.5f)
                .Draggable(true)
                .InvokeIcon(bitmap);

            ////在地图上添加Marker，并显示  
            this.mapView.Map.AddOverlay(option);


            //使地图移动到当前位置
            MapStatus mMapStatus = new MapStatus.Builder()
                .Target(point)
                .Zoom(18)
                .Build();

            //定义MapStatusUpdate对象,以便描述地图状态将要发生的变化 
            MapStatusUpdate mMapStatusUpdate = MapStatusUpdateFactory.NewMapStatus(mMapStatus);
            //改变地图状态 
            this.mapView.Map.SetMapStatus(mMapStatusUpdate);

        }


        public void Stop()
        {
            stopped = true;
            native.Stop();
        }

        public bool stopped = true;


        public bool IsStarted
        {
            get { return native.IsStarted; }
        }

        /// <summary>
        /// 实现位置监听，实时缓存我的位置
        /// </summary>
        /// <param name="location"></param>
        public void OnReceiveLocation(BDLocation location)
        {
            if (stopped)
            {
                return;
            }

            System.Diagnostics.Debug.WriteLine("LocType: " + location.LocType);

            switch (location.LocType)
            {
                default: break;
                case BDLocation.TypeServerError:
                    {
                        System.Diagnostics.Debug.Print("服务端网络定位失败，可以反馈IMEI号和大体定位时间到loc-bugs@baidu.com，会有人追查原因");
                    }
                    break;
                case BDLocation.TypeNetWorkException:
                    {
                        System.Diagnostics.Debug.Print("网络不同导致定位失败，请检查网络是否通畅");
                    }
                    break;
                case BDLocation.TypeCriteriaException:
                    {
                        System.Diagnostics.Debug.Print("无法获取有效定位依据导致定位失败，一般是由于手机的原因，处于飞行模式下一般会造成这种结果，可以试着重启手机");
                    }
                    break;
                case BDLocation.TypeGpsLocation:
                case BDLocation.TypeNetWorkLocation:
                case BDLocation.TypeOffLineLocation:
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

                        System.Diagnostics.Debug.Print(sb.ToString());

                        BMap.MyLocationData loc = new BMap.MyLocationData.Builder()
                      .Accuracy(location.Radius)
                      .Direction(location.Direction)
                      .Latitude(location.Latitude)
                      .Longitude(location.Longitude)
                      .Build();

                        mapView.Map.SetMyLocationData(loc);

                        ///位置更新
                        LocationUpdated?.Invoke(this, new LocationUpdatedEventArgs
                        {
                            Coordinate = new Coordinate(loc.Latitude, loc.Longitude),
                            Direction = location.Direction,
                            Accuracy = location.HasRadius ? location.Radius : double.NaN,
                            Altitude = location.HasAltitude ? location.Altitude : double.NaN,
                            Satellites = location.SatelliteNumber,
                            Type = location.LocTypeDescription,
                            Province = location.Province,
                            City = location.City,
                            District = location.District,
                            Street = location.Street,
                            Address = location.AddrStr
                        });
                        return;
                    }
            }

            Failed?.Invoke(this, new LocationFailedEventArgs(location.LocType.ToString()));
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
            { 
                // GPS定位结果  
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
            {
                // 网络定位结果  
                sb.Append("\naddr : ");
                sb.Append(location.AddrStr);
                //运营商信息  
                sb.Append("\noperationers : ");
                sb.Append(location.Operators);
                sb.Append("\ndescribe : ");
                sb.Append("网络定位成功");
            }
            else if (location.LocType == BDLocation.TypeOffLineLocation)
            {
                // 离线定位结果  
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

        public void OnConnectHotSpotMessage(string p0, int p1)
        {
            //throw new NotImplementedException();
        }

        public event EventHandler<LocationFailedEventArgs> Failed;
        public event EventHandler<LocationUpdatedEventArgs> LocationUpdated;
    }
}