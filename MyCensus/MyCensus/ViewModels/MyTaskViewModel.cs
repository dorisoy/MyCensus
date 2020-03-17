using Prism;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;

using System;
using System.Collections.Generic;
using System.Linq;

using System.Windows.Input;
using Xamarin.Forms;
using System.ComponentModel;
using System.Threading.Tasks;
using MyCensus.Models;
using MyCensus.DataServices.Interfaces;
using MyCensus.Models.Rides;
using MyCensus.Utils;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using MyCensus.Services;
using MyCensus.DataServices;
using MyCensus.Cache;
using Acr.UserDialogs;

namespace MyCensus.ViewModels
{
    public class MyTaskViewModel : ViewModelBase, IActiveAware
    {
        private new INavigationService _navigationService;
        private new IDialogService _dialogService;
        private readonly ICensusService _cnsusService;
        private readonly ISettingService _settingService;
        private readonly ICacheManager _cacheManager;

        public MyTaskViewModel(INavigationService navigationService,
            ICensusService cnsusService,
            ICacheManager cacheManager,
            ISettingService settingService,
            IDialogService dialogService) : base(navigationService, dialogService)
        {
            _navigationService = navigationService;
            _dialogService = dialogService;
            _cnsusService = cnsusService;
            _cacheManager = cacheManager;
            _settingService = settingService;
            Title = "添加终端普查";
        }


        private DelegateCommand<string> _cmdSelectedCommand;
        public DelegateCommand<string> CmdSelectedCommand
        {
            get
            {
                if (_cmdSelectedCommand == null)
                {
                    _cmdSelectedCommand = new DelegateCommand<string>(async (r) =>
                    {
                        try
                        {
                            if (GlobalSettings.CurrtntCoordinate == null && GlobalSettings.OffMap == false)
                            {
                                //提示定位是否开启
                                var confirm = await _dialogService.ShowConfirmAsync("你的位置定位没有开启", "提示", "去开启", "不开启");
                                if (confirm)
                                {
                                    await Application.Current.MainPage.Navigation.PushAsync(new MyCensus.Views.SiteMap());
                                    return;
                                }
                            }

                            switch (r)
                            {
                                case "Tradition":
                                    {
                                        //传统终端普查
                                        var parameter = new NavigationParameters() { { "action", "add" } };
                                        await _navigationService.NavigateAsync("Tradition_Card_Page1", parameter);
                                        break;
                                    }
                                   
                                case "Restaurant":
                                    {
                                        //餐饮终端普查
                                        var parameter = new NavigationParameters() { { "action", "add" } };
                                        await _navigationService.NavigateAsync("Restaurant_Card_Page1", parameter);
                                        break;
                                    }
                                case "Kayechang":
                                    {
                                        //Ka夜场终端普查
                                        var parameter = new NavigationParameters() { { "action", "add" } };
                                        await _dialogService.ShowAlertAsync("抱歉,功能稍后开放...", "提示", "取消");
                                        break;
                                    }
                                case "Qudao":
                                    {
                                        //渠道终端普查
                                        var parameter = new NavigationParameters() { { "action", "add" } };
                                        await _dialogService.ShowAlertAsync("抱歉,功能稍后开放...", "提示", "取消");
                                        break;
                                    }
                            }
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.Print(ex.Message);
                        }
                    });
                }
                return _cmdSelectedCommand;
            }
        }


        private int _traditionSum;
        private int _restaurantSum;
        private int _kayechangSum;
        private int _qudaoSum;
        public int TraditionSum
        {
            get
            {
                return _traditionSum;
            }
            set
            {
                _traditionSum = value;
                RaisePropertyChanged(() => TraditionSum);
            }
        }
        public int RestaurantSum
        {
            get
            {
                return _restaurantSum;
            }
            set
            {
                _restaurantSum = value;
                RaisePropertyChanged(() => RestaurantSum);
            }
        }
        public int KayechangSum
        {
            get
            {
                return _traditionSum;
            }
            set
            {
                _traditionSum = value;
                RaisePropertyChanged(() => KayechangSum);
            }
        }
        public int QudaoSum
        {
            get
            {
                return _traditionSum;
            }
            set
            {
                _traditionSum = value;
                RaisePropertyChanged(() => QudaoSum);
            }
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
                try
                {
                    var statistics = await _cnsusService.GetStatistics();
                    TraditionSum = statistics.Where(s => s.Type == "Tradition").FirstOrDefault().EndPointSum;
                    RestaurantSum = statistics.Where(s => s.Type == "Restaurant").FirstOrDefault().EndPointSum;

                    var cacheTraditionSetting = await _cacheManager.Get<TraditionSetting>(GlobalSettings.traditionSetting_key);
                    var cacheRestaurantSetting = await _cacheManager.Get<RestaurantSetting>(GlobalSettings.restaurantSetting_key);
                    var cacheSalesProductSetting = await _cacheManager.Get<SalesProductSetting>(GlobalSettings.salesProductSetting_key);

                    if (cacheTraditionSetting == null)
                    {
                        cacheTraditionSetting = await _settingService.GetTraditionSetting();
                        await _cacheManager.Set<TraditionSetting>(GlobalSettings.traditionSetting_key, cacheTraditionSetting);
                    }

                    if (cacheRestaurantSetting == null)
                    {
                        cacheRestaurantSetting = await _settingService.GetRestaurantSetting();
                        await _cacheManager.Set<RestaurantSetting>(GlobalSettings.restaurantSetting_key, cacheRestaurantSetting);
                    }

                    if (cacheTraditionSetting == null)
                    {
                        cacheSalesProductSetting = await _settingService.GetSalesProductSetting();
                        await _cacheManager.Set<SalesProductSetting>(GlobalSettings.salesProductSetting_key, cacheSalesProductSetting);
                    }

                    GlobalSettings.TraditionSetting = cacheTraditionSetting;
                    GlobalSettings.RestaurantSetting = cacheRestaurantSetting;
                    GlobalSettings.SalesProductSetting = cacheSalesProductSetting;
                }
                catch (Exception)
                {
                    _dialogService.LongAlert("系统初始错误!");
                }
            }
        }

    }


}
