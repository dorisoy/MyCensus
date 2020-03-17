using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;

/*
[assembly: Dependency(typeof(NetworkConnection))]
namespace MyCensus.iOS.Services
{
    public class NetworkConnection : INetworkConnection
    {
        //public NetworkConnection()
        //{
        //    InternetConnectionStatus();
        //}
        public bool IsConnected { get; set; }
        public void CheckNetworkConnection()
        {
            InternetConnectionStatus();
        }


        private void UpdateNetworkStatus()
        {
            if (InternetConnectionStatus())
            {
                IsConnected = true;
            }
            else if (LocalWifiConnectionStatus())
            {
                IsConnected = true;
            }
            else
            {
                IsConnected = false;
            }
        }


        private event EventHandler ReachabilityChanged;
        private void OnChange(NetworkReachabilityFlags flags)
        {
            var h = ReachabilityChanged;
            if (h != null)
                h(null, EventArgs.Empty);
        }


        private NetworkReachability defaultRouteReachability;
        private bool IsNetworkAvailable(out NetworkReachabilityFlags flags)
        {
            if (defaultRouteReachability == null)
            {
                defaultRouteReachability = new NetworkReachability(new IPAddress(0));
                defaultRouteReachability.SetNotification(OnChange);
                //defaultRouteReachability.SetCallback(OnChange);
                defaultRouteReachability.Schedule(CFRunLoop.Current, CFRunLoop.ModeDefault);
            }
            if (!defaultRouteReachability.TryGetFlags(out flags))
                return false;
            return IsReachableWithoutRequiringConnection(flags);
        }


        private NetworkReachability adHocWiFiNetworkReachability;
        private bool IsAdHocWiFiNetworkAvailable(out NetworkReachabilityFlags flags)
        {
            if (adHocWiFiNetworkReachability == null)
            {
                adHocWiFiNetworkReachability = new NetworkReachability(new IPAddress(new byte[] { 169, 254, 0, 0 }));
                adHocWiFiNetworkReachability.SetNotification(OnChange);
                //adHocWiFiNetworkReachability.SetCallback(OnChange);
                adHocWiFiNetworkReachability.Schedule(CFRunLoop.Current, CFRunLoop.ModeDefault);
            }


            if (!adHocWiFiNetworkReachability.TryGetFlags(out flags))
                return false;


            return IsReachableWithoutRequiringConnection(flags);
        }


        public static bool IsReachableWithoutRequiringConnection(NetworkReachabilityFlags flags)
        {
            // 它是否可以与当前的网络配置联系在一起?
            bool isReachable = (flags & NetworkReachabilityFlags.Reachable) != 0;


            // 我们需要连接来达到它吗?
            bool noConnectionRequired = (flags & NetworkReachabilityFlags.ConnectionRequired) == 0;


            // 网络堆栈
            // 检查
            if ((flags & NetworkReachabilityFlags.IsWWAN) != 0)
                noConnectionRequired = true;


            return isReachable && noConnectionRequired;
        }


        private bool InternetConnectionStatus()
        {
            NetworkReachabilityFlags flags;
            bool defaultNetworkAvailable = IsNetworkAvailable(out flags);
            if (defaultNetworkAvailable && ((flags & NetworkReachabilityFlags.IsDirect) != 0))
            {
                return false;
            }
            else if ((flags & NetworkReachabilityFlags.IsWWAN) != 0)
            {
                return true;
            }
            else if (flags == 0)
            {
                return false;
            }


            return true;
        }


        private bool LocalWifiConnectionStatus()
        {
            NetworkReachabilityFlags flags;
            if (IsAdHocWiFiNetworkAvailable(out flags))
            {
                if ((flags & NetworkReachabilityFlags.IsDirect) != 0)
                    return true;
            }
            return false;
        }
    }
}
*/