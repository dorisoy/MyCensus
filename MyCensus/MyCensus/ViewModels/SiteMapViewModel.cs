using Prism;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;

using MyCensus.DataServices;
using MyCensus.DataServices.Base;
using MyCensus.Validations;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using MyCensus.Controls;
using MyCensus.Services;
using Plugin.Geolocator;
using MyCensus.ViewModels.Base;
using Acr.UserDialogs;
using MyCensus.Cache;

namespace MyCensus.ViewModels
{

    public class SiteMapViewModel : ViewModelBase, IActiveAware
    {
        private new INavigationService _navigationService;
        private new IDialogService _dialogService;
        public SiteMapViewModel(INavigationService navigationService, IDialogService dialogService) : base(navigationService, dialogService)
        {
            _navigationService = navigationService;
            _dialogService = dialogService;
            Title = "定位当前位置";
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
                //var series = await _tsApiService.GetStatsTopSeries();
                await Task.Delay(100);
            }
        }

        //TimeLineCommand
        public ICommand TimeLineCommand => new Command<string>(async (id) => { await TimeLine(id); });
        private async Task TimeLine(string pid)
        {
            await _navigationService.NavigateAsync("TimeLinePage");
        }



        public ICommand TrackCommand => new Command(() => { TrackCommandAsync(); });
        private void TrackCommandAsync()
        {
            //await DialogService.ShowAlertAsync("Communication error", "Error", "Ok");
            //var longitude = string.Format("{0:0.0000000}", position.Longitude);
            //var latitude = string.Format("{0:0.0000000}", position.Latitude);
            //await _dialogService.ShowAlertAsync("longitude:" + longitude + "  latitude:" + latitude, "提示", "取消");
            //Plugin.Geolocator.Abstractions.Position
            //Plugin.Geolocator.Abstractions.Position position = new Plugin.Geolocator.Abstractions.Position();
            //Lat=34.364438,Lng=108.941338
            //position.Latitude = 34.364438;
            //position.Longitude = 108.941338;
            //Plugin.Geolocator.Abstractions.Position
            var position = new Coordinate(34.344438, 108.921338);
            MessagingCenter.Send<Coordinate>(position, "MyBDLocation");
        }

        public override Task InitializeAsync(object navigationData)
        {
            return Task.FromResult(false);
        }
        
    }
}
