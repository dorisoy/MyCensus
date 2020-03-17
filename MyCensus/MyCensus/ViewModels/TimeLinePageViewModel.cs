using Prism;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;

using System;
using System.Collections.Generic;
using System.Linq;


using System.Windows.Input;

using Xamarin.Forms;

using MyCensus.DataServices.Interfaces;
using MyCensus.Models.Rides;
using MyCensus.Utils;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

using System.ComponentModel;
using MyCensus.Models;

using MyCensus.Services;
using MyCensus.ViewModels.Base;

using System.Collections.ObjectModel;
using MyCensus.Cache;

namespace MyCensus.ViewModels
{
    public class TimeLinePageViewModel : ViewModelBase, IActiveAware
    {
        private new INavigationService _navigationService;
        private new IDialogService _dialogService;
        private readonly ICensusService _censusService;
        private readonly ICacheManager _cacheManager;

        public TimeLinePageViewModel(INavigationService navigationService,
            ICensusService censusService,
            ICacheManager cacheManager,
            IDialogService dialogService) : base(navigationService, dialogService)
        {
            _navigationService = navigationService;
            _dialogService = dialogService;
            _censusService = censusService;
            _cacheManager = cacheManager;
            Title = "当前普查进度";
            IsBusy = false;
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

        private ObservableRangeCollection<TrackTimeLine> _classes;
        public ObservableRangeCollection<TrackTimeLine> Classes
        {
            get
            {
                return _classes;
            }
            set
            {
                _classes = value;
                RaisePropertyChanged(() => Classes);
            }
        }



        public ICommand RefreshCommand => new Command(RefreshRidesCommand);
        private async void RefreshRidesCommand(object obj)
        {
            await LoadData();
        }



        private async Task LoadData()
        {
            IsBusy = true;

            try
            {
                //0:H:mm
                var timeLines = new List<TrackTimeLine>(); 
                var cacheTimeLine = await _cacheManager.Get<List<TrackTimeLine>>(GlobalSettings.timeLine_key);
                if (cacheTimeLine != null)
                {
                    timeLines = cacheTimeLine;
                }
                else
                {
                    timeLines = await _censusService.GetTrackTimeLines();
                    await _cacheManager.Set<List<TrackTimeLine>>(GlobalSettings.timeLine_key, timeLines);
                }

                if (timeLines.Count > 0)
                {
                    var lastNode = timeLines.Last().IsLast = true; ;
                    Classes = new ObservableRangeCollection<TrackTimeLine>(timeLines);
                }
            }
            catch (Exception ex) when (ex is WebException || ex is HttpRequestException)
            {
                //await _dialogService.ShowAlertAsync("Communication error", "Error", "Ok");
            }

            IsBusy = false;
        }



        private static DateTime TodayAt(int hour, int minute)
        {
            return new DateTime(DateTime.Now.Year,
                DateTime.Now.Month,
                DateTime.Now.Day,
                hour, minute, 0);
        }
    }
}
