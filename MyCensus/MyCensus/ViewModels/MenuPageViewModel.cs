using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;

using MyCensus.Models.Enums;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using MyCensus.DataServices;
using System;
using System.Net.Http;
using System.Diagnostics;
using System.Net;
using Xamarin.Forms;
using MenuItem = MyCensus.Models.MenuItem;
using MyCensus.Models.Rides;
using MyCensus.ViewModels.Base;
using System.Linq;
using MyCensus.Helpers;
using MyCensus.Models.Users;
using Prism;
using System.Collections.Generic;
using System.ComponentModel;
using MyCensus.Models;
using MyCensus.DataServices.Interfaces;
using MyCensus.Utils;
using MyCensus.Services;
using MyCensus.Cache;
using Acr.UserDialogs;

namespace MyCensus.ViewModels
{
    public class MenuPageViewModel : ViewModelBase, IActiveAware
    {
        private readonly IProfileService _profileService;
        private readonly IAuthenticationService _authenticationService;
        private readonly ICacheManager _cacheManager;

        public ICommand ItemSelectedCommand => new Command<MenuItem>(OnSelectItem);

        public ICommand LogoutCommand => new Command(OnLogout);

        ObservableCollection<MenuItem> menuItems = new ObservableCollection<MenuItem>();

        public ObservableCollection<MenuItem> MenuItems
        {
            get
            {
                return menuItems;
            }
            set
            {
                menuItems = value;
                RaisePropertyChanged(() => MenuItems);
            }
        }

        JH_Auth_User profile;

        public JH_Auth_User Profile
        {
            get
            {
                return profile;
            }

            set
            {
                profile = value;
                RaisePropertyChanged(() => Profile);
                RaisePropertyChanged(() => ProfileFullName);
            }
        }

        public string ProfileFullName
        {
            get
            {
                if (Profile == null)
                {
                    return "-";
                }
                else
                {
                    return Profile.UserRealName;
                }
            }
        }



        private new INavigationService _navigationService;
        private new IDialogService _dialogService;
        public MenuPageViewModel(IProfileService profileService, 
            IAuthenticationService authenticationService,
            INavigationService navigationService,
            ICacheManager cacheManager,
            IDialogService dialogService) : base(navigationService, dialogService)
        {
            _navigationService = navigationService;
            _dialogService = dialogService;
            _profileService = profileService;
            _authenticationService = authenticationService;
            _cacheManager = cacheManager;

            //MessagingCenter.Subscribe<Booking>(this, MessengerKeys.BookingRequested, OnBookingRequested);
            //MessagingCenter.Subscribe<Booking>(this, MessengerKeys.BookingFinished, OnBookingFinished);
            MessagingCenter.Subscribe<JH_Auth_User>(this, MessengerKeys.ProfileUpdated, OnProfileUpdated);

            Title = "账户信息";

            InitMenuItems();
        }


        public event EventHandler IsActiveChanged;
        private bool _isActive;
        public bool IsActive
        {
            get { return _isActive; }
            set
            {
                _isActive = value;
                OnActiveTabChangedAsync();
            }
        }
        private async void OnActiveTabChangedAsync()
        {
            if (IsActive)
            {
                await LoadData();
            }
        }



        public async Task LoadData() 
        {
            IsBusy = true;
            try
            {
                //获取个人资料
                var cacheProfile = await _cacheManager.Get<JH_Auth_User>(GlobalSettings.menupage_profile_key);
                if (cacheProfile != null)
                {
                    Profile = cacheProfile;
                }
                else
                {
                    Profile = await _profileService.GetCurrentProfileAsync();
                    await _cacheManager.Set<JH_Auth_User>(GlobalSettings.menupage_profile_key, Profile);
                }

                if (!IsConnected())
                    _dialogService.LongAlert("网络异常,数据获取失败!");

                if (string.IsNullOrEmpty(Profile.tx))
                    Profile.tx = "profile_generic.png";

            }
            catch (Exception ex) when (ex is WebException || ex is HttpRequestException)
            {
                //await _dialogService.ShowAlertAsync("Communication error", "Error", "Ok");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error fetching profile with exception: {ex}");
            }
            IsBusy = false;
        }

        private void InitMenuItems()
        {
            //if(Device.RuntimePlatform == Device.UWP)
            //{
            //    //MenuItems.Add(new MenuItem
            //    //{
            //    //    Title = "Home",
            //    //    MenuItemType = MenuItemType.Home,
            //    //    ViewModelType = typeof(MainPageViewModel),
            //    //    IsEnabled = true
            //    //});
            //}


            //MenuItems.Add(new MenuItem
            //{
            //    Title = "系统设置",
            //    MenuItemType = MenuItemType.Setting,
            //    ViewModelType = typeof(CustomRideViewModel),
            //    IsEnabled = true
            //});


            //if (Device.RuntimePlatform == Device.Android)
            //{
            //    MenuItems.Add(new MenuItem
            //    {
            //        Title = "新任务",
            //        MenuItemType = MenuItemType.NewRide,
            //        ViewModelType = typeof(CustomRideViewModel),
            //        IsEnabled = true
            //    });
            //}

            //MenuItems.Add(new MenuItem
            //{
            //    Title = "My Rides",
            //    MenuItemType = MenuItemType.MyRides,
            //    ViewModelType = Device.Idiom == TargetIdiom.Desktop ? typeof(UwpMyRidesViewModel) : typeof(MyRidesViewModel),
            //    IsEnabled = true
            //});

            MenuItems.Add(new MenuItem
            {
                Title = "拜访计划",
                MenuItemType = MenuItemType.UpcomingRide,
                MenuItemIcon= "fa-calendar",
                ViewModelType = null,
                //IsEnabled = Settings.CurrentBookingId != 0
                IsEnabled = true
            });

            MenuItems.Add(new MenuItem
            {
                Title = "报表",
                MenuItemType = MenuItemType.ReportIncident,
                MenuItemIcon = "fa-bar-chart",
                ViewModelType = typeof(ReportIncidentPageViewModel),
                IsEnabled = true
            });

            MenuItems.Add(new MenuItem
            {
                Title = "个人资料",
                MenuItemType = MenuItemType.Profile,
                MenuItemIcon = "fa-user",
                ViewModelType = typeof(ProfilePageViewModel),
                IsEnabled = true
            });

            MenuItems.Add(new MenuItem
            {
                Title = "版本更新",
                MenuItemType = MenuItemType.Setting,
                MenuItemIcon = "fa-cloud-download",
                ViewModelType = typeof(UpdatePageViewModel),
                IsEnabled = true
            });

            MenuItems.Add(new MenuItem
            {
                Title = "清理缓存",
                MenuItemType = MenuItemType.ClearCache,
                MenuItemIcon = "fa-trash",
                ViewModelType = null,
                IsEnabled = true
            });
        }

        private async void OnSelectItem(MenuItem item)
        {
            if (item.IsEnabled)
            {
                object parameter = null;

                if (item.MenuItemType == MenuItemType.UpcomingRide)
                {
                    //parameter = new BookingViewModel.BookingViewModelNavigationParameter
                    //{
                    //    ShowThanks = false,
                    //    BookingId = Settings.CurrentBookingId
                    //};
                }
                else if (item.MenuItemType == MenuItemType.Profile)
                {
                    //await _navigationService.NavigateToAsync(item.ViewModelType, parameter);
                    await _navigationService.NavigateAsync("ProfilePage");
                }
                else if (item.MenuItemType == MenuItemType.Setting)
                {
                     await _navigationService.NavigateAsync("UpdatePage");
                }
                else if (item.MenuItemType == MenuItemType.ClearCache)
                {
                    using (UserDialogs.Instance.Loading("清理中..."))
                    {
                        try
                        {
                            await Task.Delay(1000);
                            //await _cacheManager.Delete(GlobalSettings.traditions_key);
                            //await _cacheManager.Delete(GlobalSettings.restaurants_key);
                            //await _cacheManager.Delete(GlobalSettings.timeLine_key);
                            //await _cacheManager.Delete(GlobalSettings.network_traditions_key);
                            //await _cacheManager.Delete(GlobalSettings.network_restaurants_key);
                            //await _cacheManager.Delete(GlobalSettings.menupage_profile_key);
                            //await _cacheManager.Delete(GlobalSettings.profilepage_profile_key);
                            //await _cacheManager.Delete(GlobalSettings.traditionSetting_key);
                            //await _cacheManager.Delete(GlobalSettings.restaurantSetting_key);
                            //await _cacheManager.Delete(GlobalSettings.salesProductSetting_key);
                            await _cacheManager.DeleteAll();
                        }
                        catch(Exception ex)
                        {
                            _dialogService.ShortAlert(ex.Message);
                        }
                    };
                }
                else
                {
                    await _dialogService.ShowAlertAsync("功能稍后开放哦!", "提示", "取消");
                }

            }
        }

        /// <summary>
        /// 注销推出
        /// </summary>
        private async void OnLogout()
        {
            await _authenticationService.LogoutAsync();
            await _navigationService.RemovePageAsync("MenuPage");
            await _navigationService.NavigateAsync("LoginPage?return=MenuPage");
        }

        private void OnBookingRequested(Booking booking)
        {
            SetMenuItemStatus(MenuItemType.UpcomingRide, true);
            SetMenuItemStatus(MenuItemType.ReportIncident, true);
        }

        private void OnBookingFinished(Booking booking)
        {
            SetMenuItemStatus(MenuItemType.UpcomingRide, false);
        }

        private void SetMenuItemStatus(MenuItemType type, bool enabled)
        {
            MenuItem rideItem = MenuItems.FirstOrDefault(m => m.MenuItemType == type);

            if (rideItem != null)
            {
                rideItem.IsEnabled = enabled;
            }
        }

        private async void OnProfileUpdated(JH_Auth_User profile)
        {
            Profile = null;

            if (Device.RuntimePlatform == Device.UWP)
            {
                await Task.Delay(2000); // Give UWP enough time (for Photo reload)
            }

            Profile = profile;
        }
    }
}