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

    public class ViewLocationViewModel : ViewModelBase, IActiveAware
    {
        private new INavigationService _navigationService;
        private new IDialogService _dialogService;
        public ViewLocationViewModel(INavigationService navigationService, IDialogService dialogService) : base(navigationService, dialogService)
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
                await Task.Delay(100);
            }
        }

        public override Task InitializeAsync(object navigationData)
        {
            return Task.FromResult(false);
        }

    }
}
