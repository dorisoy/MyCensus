using MyCensus.Models.Enums;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace MyCensus.Converters
{

    /// <summary>
    /// 菜单项目类型转化
    /// </summary>
    public class MenuItemTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var menuItemType = (MenuItemType)value;

            var platform = Device.RuntimePlatform == Device.UWP;

            switch(menuItemType)
            {
                case MenuItemType.Setting:
                    return platform ? "Assets/uwp_ic_setting.png" : "menu_ic_setting.png";
                case MenuItemType.Profile:
                    return platform ? "Assets/uwp_ic_user.png" : "menu_ic_user.png";
                case MenuItemType.MyRides:
                    return platform ? "Assets/uwp_ic_my_rides.png" : "menu_ic_bike.png";
                case MenuItemType.UpcomingRide:
                    return platform ? "Assets/uwp_ic_upcoming_ride.png" : "menu_ic_current_book.png";
                case MenuItemType.ReportIncident:
                    return platform ? "Assets/uwp_ic_ranking.png" : "menu_ic_ranking.png";
                case MenuItemType.NewRide:
                    return platform ? "Assets/uwp_ic_new_ride.png" : "menu_ic_new_ride.png";
                case MenuItemType.Home:
                    return platform ? "Assets/uwp_ic_home.png" : string.Empty;
                default:
                    return string.Empty;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
