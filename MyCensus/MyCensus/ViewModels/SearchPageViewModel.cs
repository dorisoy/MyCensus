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

namespace MyCensus.ViewModels
{
	public class SearchPageViewModel : ViewModelBase, IActiveAware
    {
        private new INavigationService _navigationService;
        private new IDialogService _dialogService;
        private readonly ICensusService _cnsusService;
        private readonly ISettingService _settingService;
        private readonly ICacheManager _cacheManager;

        public SearchPageViewModel(INavigationService navigationService,
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
            Title = "搜索商品";
        }


        public event EventHandler IsActiveChanged;
        private bool _isActive;
        public bool IsActive
        {
            get { return _isActive; }
            set
            {
                _isActive = value;
                LodaData();
            }
        }
        private async void LodaData()
        {
            if (IsActive)
            {
                try
                {
                    await Task.Delay(500);
                }
                catch (Exception)
                {
                    _dialogService.LongAlert("系统初始错误!");
                }
            }
        }


        /// <summary>
        /// 扫描条码
        /// </summary>
        public ICommand ScanCodeCommand => new Command(ScanCode);
        private async void ScanCode(object obj)
        {
            //await _dialogService.ShowAlertAsync("Communication error", "Error", "Ok");
            //await _navigationService.NavigateAsync("ScanPage");
            await _navigationService.NavigateAsync("SaoPage");
        }

    }
}
