using Prism;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;

using MyCensus.DataServices.Interfaces;
using MyCensus.Models.Rides;
using MyCensus.Utils;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using MyCensus.Services;

namespace MyCensus.ViewModels
{
    public class AboutPageViewModel : ViewModelBase, IActiveAware
    {
        private new readonly INavigationService _navigationService;
        private new readonly IDialogService _dialogService;


        public AboutPageViewModel(INavigationService navigationService, IDialogService dialogService) : base(navigationService, dialogService)
        {
            _navigationService = navigationService;
            _dialogService = dialogService;
        }

        //OpenWebCommand = new Command(() => Device.OpenUri(new Uri("https://xamarin.com/platform")));
        //public ICommand OpenWebCommand { get; }

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


    }
}