using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using Prism.Ioc;

using MyCensus.Services;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using MyCensus.Models.Census;
using MyCensus.DataServices.Interfaces;


using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Windows.Input;

using MyCensus.ViewModels.Base;
using MyCensus.Helpers;
using MyCensus.Services.Interfaces;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Forms;
using Acr.UserDialogs;

using MyCensus.Models;
using Newtonsoft.Json;
using Stormlion.PhotoBrowser;
using MyCensus.Controls;

namespace MyCensus.ViewModels
{
	public class TraditionViewCardViewModel : ViewModelBase, IActiveAware
    {
        private new INavigationService _navigationService;
        private new IDialogService _dialogService;
        private readonly ICensusService _censusService;

        public TraditionViewCardViewModel(INavigationService navigationService,
            ICensusService censusService,
            IDialogService dialogService) : base(navigationService, dialogService)
        {
            _navigationService = navigationService;
            _dialogService = dialogService;
            _censusService = censusService;
            _productList = new ObservableRangeCollection<SalesProduct>();

        }
        #region Property

        public new event PropertyChangedEventHandler PropertyChanged;
        protected new virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(storage, value))
            {
                return false;
            }

            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        #endregion

        private Tradition _tradition;
        public Tradition Tradition
        {
            get
            {
                return _tradition;
            }
            set
            {
                _tradition = value;
                RaisePropertyChanged(() => Tradition);
            }
        }


        private string _thumbnailPhotoUrl;
        public string ThumbnailPhotoUrl
        {
            get
            {
                return _thumbnailPhotoUrl;
            }
            set
            {
                _thumbnailPhotoUrl = value;
                RaisePropertyChanged(() => ThumbnailPhotoUrl);
            }
        }


        private ObservableRangeCollection<SalesProduct> _productList;
        public ObservableRangeCollection<SalesProduct> ProductList
        {
            get
            {
                return _productList;
            }
            set
            {
                _productList = value;
                RaisePropertyChanged(() => ProductList);
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

        private string _action;
        public string Action
        {
            get { return _action; }
            set
            {
                this.SetProperty(ref _action, value);
            }
        }


        private int _curIteamId;
        public int CurIteamId
        {
            get { return _curIteamId; }
            set
            {
                this.SetProperty(ref _curIteamId, value);
            }
        }

        public int RideType { get; set; }



        /// <summary>
        /// 查看定位
        /// </summary>
        public ICommand ViewLocationCmd => new Command(ViewLocation);
        private async void ViewLocation(object obj)
        {
            await _navigationService.NavigateAsync("ViewLocation");
            if (Tradition != null)
            {
                var coordinate = new Coordinate(Tradition.Latitude, Tradition.Longitude);
                MessagingCenter.Send<Coordinate>(coordinate, "ViewLocationMap");
            }
        }



        private async void OnActiveTabChangedAsync()
        {
            if (IsActive)
            {
                //await _dialogService.ShowAlertAsync("OnActiveTabChangedAsync", "提示", "ok");
                await LoadData();
            }
        }


        public override void OnNavigatingTo(NavigationParameters parameters)
        {
            if (parameters.ContainsKey("action"))
            {
                Action = (string)parameters["action"];
                RideType = (int)parameters["type"];
                CurIteamId = (int)parameters["id"];
                if (Action.ToLower() == "view")
                {
                    Title = "普查详细信息";
                }
            }
        }



        private async Task LoadData()
        {
            IsBusy = true;

            while (CurIteamId == 0)
            {
                await Task.Delay(1000);
            }

            if (CurIteamId == 0)
                return;

            var tradition = await _censusService.GetTradition(CurIteamId);
            if (tradition != null)
            {
                if (tradition.DoorheadPhotos.FirstOrDefault() != null)
                    ThumbnailPhotoUrl = tradition.DoorheadPhotos.FirstOrDefault().StoragePath;

                Tradition = tradition;
            }
            else
            {
                Tradition = new Tradition();
            }
            IsBusy = false;
        }




    }
}
