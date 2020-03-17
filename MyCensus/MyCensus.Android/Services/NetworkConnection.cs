using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Threading.Tasks;
using MyCensus.Droid.Services;
using Android.Graphics;
using Android.Locations;
using Android.Provider;
using Android.Net;
using Android.Telephony;

using Xamarin.Forms;
using MyCensus.Services.Interfaces;

[assembly: Dependency(typeof(MyCensus.Droid.Services.NetworkConnection))]
namespace MyCensus.Droid.Services
{
    //public class NetworkConnection : INetworkConnection
    //{

    //    public bool IsNetWorkConnected()
    //    {
    //        ConnectivityManager manager = ConnectivityManager.FromContext(MainActivity.AppContext);
    //        NetworkInfo info = manager.ActiveNetworkInfo;
    //        if (info != null)
    //        {
    //            return info.IsAvailable;
    //        }
    //        return false;
    //    }

    //    public bool IsWifiConnected()
    //    {
    //        ConnectivityManager manager = ConnectivityManager.FromContext(MainActivity.AppContext);
    //        NetworkInfo info = manager.GetNetworkInfo(ConnectivityType.Wifi);
    //        if (info != null)
    //        {
    //            return info.IsAvailable;
    //        }
    //        return false;
    //    }

    //    public bool IsMobileConnected()
    //    {
    //        ConnectivityManager manager = ConnectivityManager.FromContext(MainActivity.AppContext);
    //        NetworkInfo info = manager.GetNetworkInfo(ConnectivityType.Mobile);
    //        if (info != null)
    //        {
    //            return info.IsAvailable;
    //        }
    //        return false;
    //    }
    //}

    public class NetworkConnection : INetworkConnection
    {
        //没有网络
        public const int NETWORKTYPE_INVALID = 0;
        //wap网络
        public const int NETWORKTYPE_WAP = 1;
        //2G网络
        public const int NETWORKTYPE_2G = 2;
        //3G和3G以上网络，或统称为快速网络
        public const int NETWORKTYPE_3G = 3;
        //wifi网络
        public const int NETWORKTYPE_WIFI = 4;
        public bool IsConnected { get; set; }

        /// <summary>
        /// 判断是否有网络连接
        /// </summary>
        public void CheckNetworkConnection()
        {
            var connectivityManager = (ConnectivityManager)Android.App.Application.Context.GetSystemService(Context.ConnectivityService);
            var activeNetworkInfo = connectivityManager.ActiveNetworkInfo;
            if (activeNetworkInfo != null && activeNetworkInfo.IsConnectedOrConnecting)
            {
                IsConnected = true;
            }
            else
            {
                IsConnected = false;
            }
        }


        /// <summary>
        /// 检查启用了网络位置提供商报告
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public bool isNetLocEnabled(Context context)
        {
            LocationManager lm = (LocationManager)context.GetSystemService(Context.LocationService);
            return lm.IsProviderEnabled(LocationManager.NetworkProvider);
        }


        /// <summary>
        /// 判断是否有WIFI连接
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public bool isWifiConnected(Context context)
        {
            if (context != null)
            {
                ConnectivityManager mConnectivityManager = (ConnectivityManager)context.GetSystemService(Context.ConnectivityService);
                var mWiFiNetworkInfo = mConnectivityManager.ActiveNetworkInfo;
                if (mWiFiNetworkInfo != null && mWiFiNetworkInfo.Type is ConnectivityType.Wifi)
                {
                    return mWiFiNetworkInfo.IsAvailable;
                }
            }
            return false;
        }


        /// <summary>
        /// 判断Mobile网络是否可用
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public bool isMobileConnected(Context context)
        {
            if (context != null)
            {
                ConnectivityManager mConnectivityManager = (ConnectivityManager)context.GetSystemService(Context.ConnectivityService);
                NetworkInfo mMobileNetworkInfo = mConnectivityManager.ActiveNetworkInfo;
                if (mMobileNetworkInfo != null && mMobileNetworkInfo.Type is ConnectivityType.Mobile)
                {
                    return mMobileNetworkInfo.IsAvailable;
                }
            }
            return false;
        }


        /// <summary>
        /// 检查飞行模式 - 我们要关闭飞机模式
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public bool confirmAirplaneModeOff(Context context)
        {
            ///Settings.System
            int airplaneSetting = Settings.System.GetInt(context.ContentResolver, Settings.System.AirplaneModeOn, 0);
            //int airplaneSetting = Settings.System.getInt(context.getContentResolver(), Settings.System.AIRPLANE_MODE_ON, 0);
            return airplaneSetting == 0;
        }


        /// <summary>
        /// 确定网络位置提供程序是否真的可用
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public bool isNetlocUsable(Context context)
        {
            return isNetLocEnabled(context) && confirmAirplaneModeOff(context) && isWifiConnected(context) && isMobileConnected(context);
        }


        /// <summary>
        /// 判断是否是FastMobileNetWork，将3G或者3G以上的网络称为快速网络
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private static bool isFastMobileNetwork(Context context)
        {
            TelephonyManager telephonyManager = (TelephonyManager)context.GetSystemService(Context.TelephonyService);
            switch (telephonyManager.NetworkType)
            {
                case NetworkType.OneXrtt:
                    return false; // ~ 50-100 kbps  
                case NetworkType.Cdma:
                    return false; // ~ 14-64 kbps  
                case NetworkType.Edge:
                    return false; // ~ 50-100 kbps  
                case NetworkType.Evdo0:
                    return true; // ~ 400-1000 kbps  
                case NetworkType.EvdoA:
                    return true; // ~ 600-1400 kbps  
                case NetworkType.Gprs:
                    return false; // ~ 100 kbps  
                case NetworkType.Hsdpa:
                    return true; // ~ 2-14 Mbps  
                case NetworkType.Hspa:
                    return true; // ~ 700-1700 kbps  
                case NetworkType.Hsupa:
                    return true; // ~ 1-23 Mbps  
                case NetworkType.Umts:
                    return true; // ~ 400-7000 kbps  
                case NetworkType.Ehrpd:
                    return true; // ~ 1-2 Mbps  
                case NetworkType.EvdoB:
                    return true; // ~ 5 Mbps  
                case NetworkType.Hspap:
                    return true; // ~ 10-20 Mbps  
                case NetworkType.Iden:
                    return false; // ~25 kbps  
                case NetworkType.Lte:
                    return true; // ~ 10+ Mbps  
                case NetworkType.Unknown:
                    return false;
                default:
                    return false;
            }
        }


        ///获取网络状态，wifi,wap,2g,3g.
        /// <summary>
        /// context 上下文 
        /// return int 网络状态 {@link #NETWORKTYPE_2G},{@link #NETWORKTYPE_3G}
        /// {@link #NETWORKTYPE_INVALID},{@link #NETWORKTYPE_WAP}* <p>{@link #NETWORKTYPE_WIFI} 
        /// </summary>
        public static int getNetWorkType(Context context)
        {
            int mNetWorkType = 0;
            ConnectivityManager manager = (ConnectivityManager)context.GetSystemService(Context.ConnectivityService);
            NetworkInfo networkInfo = manager.ActiveNetworkInfo;


            if (networkInfo != null && networkInfo.IsConnected)
            {
                String type = networkInfo.TypeName;


                if (type.Equals("WIFI".ToUpper()))
                {
                    mNetWorkType = NETWORKTYPE_WIFI;
                }
                else if (type.Equals("MOBILE".ToUpper()))
                {
                    String proxyHost = Proxy.DefaultHost;
                    //String proxyHost = android.net.Proxy.getDefaultHost();
                    mNetWorkType = string.IsNullOrEmpty(proxyHost) ? (isFastMobileNetwork(context) ? NETWORKTYPE_3G : NETWORKTYPE_2G) : NETWORKTYPE_WAP;
                }
            }
            else
            {
                mNetWorkType = NETWORKTYPE_INVALID;
            }
            return mNetWorkType;
        }


    }
}