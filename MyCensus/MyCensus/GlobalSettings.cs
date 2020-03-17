using System;
using System.Collections.Generic;
using MyCensus.Controls;
using MyCensus.DataServices;

namespace MyCensus
{

    /// <summary>
    /// 定义全局配置设置
    /// </summary>
    public static class GlobalSettings
    {
        public static Coordinate CurrtntCoordinate { get; set; }
        public static bool OffMap { get; set; }
        public static AddComp CurrentAddComp { get; set; }


        //身份认证服务  
        public const string AuthenticationEndpoint = "http://api.jshcrb.com";
        //public const string AuthenticationEndpoint = "http://192.168.1.42:8998";

        //事件订阅服务
        public const string EventsEndpoint = "http://events.jshcrb.com";
        //public const string EventsEndpoint = "http://192.168.1.42:8998";

        //Bug反馈服务
        public const string IssuesEndpoint = "http://issues.jshcrb.com";
        //public const string IssuesEndpoint = "http://192.168.1.42:8998";

        //Rides缓存服务
        public const string RidesEndpoint = "http://rides.jshcrb.com";
        //public const string RidesEndpoint = "http://192.168.1.42:8998";

        //文件存储服务
        public const string FileCenterEndpoint = "http://www.jshcrb.com:9100";

        public const string OpenWeatherMapAPIKey = "YOUR_WEATHERMAP_API_KEY";
        public const string HockeyAppAPIKeyForAndroid = "YOUR_HOCKEY_APP_ID";
        public const string HockeyAppAPIKeyForiOS = "YOUR_HOCKEY_APP_ID";
        public const string SkypeBotAccount = "skype:YOUR_BOT_ID?chat";
        public const string BingMapsAPIKey = "YOUR_BINGMAPS_API_KEY";

        public static string City => "西安";
        public static int TenantId = 1;
        public static DateTime EventDate = DateTime.Now;
 
        public static float EventLatitude = 34.364438f;
        public static float EventLongitude = 108.941338f;

        public static string TempGrade { get; set; }

        //cache key
        public const string traditions_key = "home_traditions_statistics";
        public const string restaurants_key = "home_restaurants_statistics";
        public const string timeLine_key = "timeline_gettracktimeLines";
        public const string network_traditions_key = "network_traditions_getTraditions";
        public const string network_restaurants_key = "network_restaurants_getRestaurants";
        public const string menupage_profile_key = "menupage_getcurrent_profile_async";
        public const string profilepage_profile_key = "profilepage_getcurrent_profile_async";

        public const string traditionSetting_key = "global_setting_tradition";
        public const string restaurantSetting_key = "global_setting_restaurant";
        public const string salesProductSetting_key = "global_setting_salesproduct";

        //Setting
        public static TraditionSetting TraditionSetting { get; set; }
        public static RestaurantSetting RestaurantSetting { get; set; }
        public static SalesProductSetting SalesProductSetting { get; set; }
        public const string productProviders_key = "global_product_providers";

    }
}
