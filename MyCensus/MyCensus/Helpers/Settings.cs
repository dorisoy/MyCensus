using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace MyCensus.Helpers
{
    /// <summary>
    /// æ≤Ã¨≤Âº˛≈‰÷√¿‡
    /// This is the Settings static class that can be used in your Core solution or in any
    /// of your client applications. All settings are laid out the same exact way with getters
    /// and setters. 
    /// </summary>
    public static class Settings
    {
        private static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }

        #region Setting Constants

        private const string UserIdKey = "user_id_key";
        private static readonly int UserIdDefault = 0;

        private const string ProfileIdKey = "profile_id_key";
        private static readonly int ProfileIdDefault = 0;

        private const string AccessTokenKey = "access_token_key";
        private static readonly string AccessTokenDefault = string.Empty;

        private const string CurrentBookingIdKey = "current_booking_id";
        private static readonly int CurrentBookingIdDefault = 0;

        private const string UwpWindowSizeKey = "uwp_window_size";
        private static readonly string UwpWindowSizeDefault = string.Empty;


        private const string SaleRegionKey = "sales_region_key";
        private static readonly string SaleRegionDefault = string.Empty;

        private const string SalesDepartmentKey = "sales_departmen_key";
        private static readonly string SalesDepartmentDefault = string.Empty;

        private const string RegionCodeKey = "region_code_key";

        private static readonly int RegionCodeDefault = 0;

        #endregion

        public static int UserId
        {
            get
            {
                return AppSettings.GetValueOrDefault(UserIdKey, UserIdDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(UserIdKey, value);
            }
        }

        public static int ProfileId
        {
            get
            {
                return AppSettings.GetValueOrDefault(ProfileIdKey, ProfileIdDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(ProfileIdKey, value);
            }
        }

        public static string AccessToken
        {
            get
            {
                return AppSettings.GetValueOrDefault(AccessTokenKey, AccessTokenDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(AccessTokenKey, value);
            }
        }

        public static int CurrentBookingId
        {
            get
            {
                return AppSettings.GetValueOrDefault(CurrentBookingIdKey, CurrentBookingIdDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(CurrentBookingIdKey, value);
            }
        }

        public static string UwpWindowSize
        {
            get
            {
                return AppSettings.GetValueOrDefault(UwpWindowSizeKey, UwpWindowSizeDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(UwpWindowSizeKey, value);
            }
        }



        public static string SaleRegion
        {
            get
            {
                return AppSettings.GetValueOrDefault(SaleRegionKey, SaleRegionDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(SaleRegionKey, value);
            }
        }

        public static string SalesDepartment
        {
            get
            {
                return AppSettings.GetValueOrDefault(SalesDepartmentKey, SalesDepartmentDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(SalesDepartmentKey, value);
            }
        }


        public static int RegionCode
        {
            get
            {
                return AppSettings.GetValueOrDefault(RegionCodeKey, RegionCodeDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(RegionCodeKey, value);
            }
        }


        public static void RemoveUserId()
        {
            AppSettings.Remove(UserIdKey);
        }

        public static void RemoveProfileId()
        {
            AppSettings.Remove(ProfileIdKey);
        }

        public static void RemoveAccessToken()
        {
            AppSettings.Remove(AccessTokenKey);
        }

        public static void RemoveCurrentBookingId()
        {
            AppSettings.Remove(CurrentBookingIdKey);
        }

        public static void RemoveSaleRegion()
        {
            AppSettings.Remove(SaleRegionKey);
        }
        public static void RemoveSalesDepartment()
        {
            AppSettings.Remove(SalesDepartmentKey);
        }
        public static void RemoveRegionCode()
        {
            AppSettings.Remove(RegionCodeKey);
        }
    }
}