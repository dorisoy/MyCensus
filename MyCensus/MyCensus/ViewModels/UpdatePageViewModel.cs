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
using System.Collections.ObjectModel;

namespace MyCensus.ViewModels
{

    public class UpdatePageViewModel : ViewModelBase, IActiveAware
    {
        private new readonly INavigationService _navigationService;
        private new readonly IDialogService _dialogService;
        private readonly IOperatingSystemVersionProvider _operatingSystemVersionProvider;
        private readonly IUpdateService _updateService;

        public UpdatePageViewModel(INavigationService navigationService,
            IOperatingSystemVersionProvider operatingSystemVersionProvider,
            IUpdateService updateService,
        IDialogService dialogService) : base(navigationService, dialogService)
        {
            _navigationService = navigationService;
            _operatingSystemVersionProvider = operatingSystemVersionProvider;
            _dialogService = dialogService;
            _updateService = updateService;
        }


        private DelegateCommand<string> _getNewVersion;
        public DelegateCommand<string> GetNewVersion
        {
            get
            {
                if (_getNewVersion == null)
                {
                    _getNewVersion = new DelegateCommand<string>(async (r) =>
                    {
                        try
                        {
                            _operatingSystemVersionProvider.CheckUpdate();
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.Print(ex.Message);
                        }
                    });
                }
                return _getNewVersion;
            }
        }


        private string _version;
        public string Version
        {
            get
            {
                return _version;
            }

            set
            {
                _version = value;
                RaisePropertyChanged(() => Version);
            }
        }


        public event EventHandler IsActiveChanged;


        private bool _isNew = false;
        public bool IsNew
        {
            get { return _isNew; }
            set
            {
                _isNew = value;
                RaisePropertyChanged(() => IsNew);
            }
        }

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

        //updatetxt
        private string _updatetxt;
        public string Updatetxt
        {
            get
            {
                return _updatetxt;
            }

            set
            {
                _updatetxt = value;
                RaisePropertyChanged(() => Updatetxt);
            }
        }

        private async void OnActiveTabChangedAsync()
        {
            if (IsActive)
            {
                Updatetxt = "更新";
                var version = await _updateService.GetCurrentVersion();
                var curVersion = _operatingSystemVersionProvider.GetVersion();
                IsNew = true;
                if (!string.IsNullOrEmpty(curVersion))
                {
                    if (version != null)
                    {
                        if (version.Version == curVersion)
                            IsNew = false; Updatetxt = "Version " + version.Version;
                    }
                }

                Version = IsNew ? string.Format("最新版本:{0}", version.Version) : "已经是最新版本了";

                IsActive = false;
            }
        }


    }
}
