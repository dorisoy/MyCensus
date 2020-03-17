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
using MyCensus.Models.Census;
using MyCensus.Services;
using MyCensus.ViewModels.Base;
using MyCensus.Cache;
using Plugin.Connectivity;



namespace MyCensus.ViewModels
{
	public class NetworksViewModel : ViewModelBase, IActiveAware
    {

        private ObservableRangeCollection<Ride> _myRides;

        private readonly IRidesService _ridesService;
        private new INavigationService _navigationService;
        private new IDialogService _dialogService;
        private readonly ICensusService _censusService;
        private readonly ICacheManager _cacheManager;

        public ObservableRangeCollection<Ride> MyRides
        {
            get
            {
                return _myRides;
            }
            set
            {
                _myRides = value;
                RaisePropertyChanged(() => MyRides);
            }
        }


        public NetworksViewModel(INavigationService navigationService,
            IRidesService ridesService,
            ICensusService censusService,
            ICacheManager cacheManager,
            IDialogService dialogService) : base(navigationService, dialogService)
        {
            _navigationService = navigationService;
            _dialogService = dialogService;
            _censusService = censusService;
            _ridesService = ridesService;
            _cacheManager = cacheManager;
            _myRides = new ObservableRangeCollection<Ride>();

            Title = "我的网点列表";

            IsBackOption = true;
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

        //IsBackOption
        private bool _isBackOption;
        public bool IsBackOption
        {
            get { return _isBackOption; }
            set
            {
                _isBackOption = value;
                RaisePropertyChanged(() => IsBackOption);
            }
        }

        private async void OnActiveTabChangedAsync()
        {
            if (IsActive)
            {
                await LoadData();
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

                if (CrossConnectivity.Current.IsConnected)
                {
                    var traditions = new List<Tradition>();
                    var restaurants = new List<Restaurant>();

                    //销量
                    try
                    {
                        var cacheTraditions = await _cacheManager.Get<List<Tradition>>(GlobalSettings.network_traditions_key);
                        if (cacheTraditions != null)
                        {
                            traditions = cacheTraditions;
                        }
                        else
                        {
                            traditions = await _censusService.GetTraditions(200, 0);
                            await _cacheManager.Set<List<Tradition>>(GlobalSettings.network_traditions_key, traditions);
                        }

                        //
                        var cacheRestaurants = await _cacheManager.Get<List<Restaurant>>(GlobalSettings.network_restaurants_key);
                        if (cacheRestaurants != null)
                        {
                            restaurants = cacheRestaurants;
                        }
                        else
                        {
                            restaurants = await _censusService.GetRestaurants(200, 0);
                            await _cacheManager.Set<List<Restaurant>>(GlobalSettings.network_restaurants_key, restaurants);
                        }

                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine(ex);
                    }

                    var ridesResult = new List<Ride>();
                    foreach (var t in traditions)
                    {
                        var photo = t.DoorheadPhotos.FirstOrDefault();
                        var track = t.TrackLocation;
                        ridesResult.Add(new Ride()
                        {
                            Id = t.Id,
                            EventId = t.Id,
                            RideType = RideType.Tradition,
                            Name = "传统普查",
                            Start = DateTime.Now,
                            Stop = t.UpdateDate,
                            Duration = track != null ? track.Duration : 0,
                            Distance = 0,
                            From = "",
                            FromStation = new Station(),
                            To = "",
                            ToStation = new Station(),
                            City = t.BaseInfo.City,
                            TraditionInfo = new Tradition()
                            {
                                BaseInfo = new TraditionBaseInfo()
                                {
                                    EndPointStorsName = t.BaseInfo.EndPointStorsName,
                                    EndPointAddress = t.BaseInfo.EndPointAddress,
                                },
                                BusinessInfo = new TraditionBusinessInfo()
                                {
                                    EndPointType = t.BusinessInfo.EndPointType
                                },
                                UpdateDate = t.UpdateDate
                            },
                            ThumbnailPhoto = photo != null ? photo.StoragePath : "noimg.jpg"
                        });
                    }

                    foreach (var t in restaurants)
                    {
                        var photo = t.DoorheadPhotos.FirstOrDefault();
                        var track = t.TrackLocation;
                        ridesResult.Add(new Ride()
                        {
                            Id = t.Id,
                            EventId = t.Id,
                            RideType = RideType.Restaurant,
                            Name = "餐饮普查",
                            Start = DateTime.Now,
                            Stop = t.UpdateDate,
                            Duration = track != null ? track.Duration : 0,
                            Distance = 0,
                            From = "",
                            FromStation = new Station(),
                            To = "",
                            ToStation = new Station(),
                            City = t.BaseInfo.City,
                            RestaurantInfo = new Restaurant()
                            {
                                BaseInfo = new RestaurantBaseInfo()
                                {
                                    EndPointStorsName = t.BaseInfo.EndPointStorsName,
                                    EndPointAddress = t.BaseInfo.EndPointAddress,
                                },
                                BusinessInfo = new RestaurantBusinessInfo()
                                {
                                    EndPointType = t.BusinessInfo.EndPointType
                                },
                                UpdateDate = t.UpdateDate
                            },
                            ThumbnailPhoto = photo != null ? photo.StoragePath : "noimg.jpg"
                        });
                    }

                    MyRides = new ObservableRangeCollection<Ride>(ridesResult);
                }
                else
                {
                    _dialogService.LongAlert("网络连接失败");
                }

            }
            catch (Exception ex) when (ex is WebException || ex is HttpRequestException)
            {
               // await _dialogService.ShowAlertAsync("Communication error", "Error", "Ok");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in: {ex}");
            }
            finally
            {
                IsBusy = false;
            }
          
        }


        public ICommand ItemSelectedCommand => new Command<Ride>(OnSelectItem);
        private async void OnSelectItem(Ride item)
        {

            var parameter = new NavigationParameters();
            parameter.Add("action", "view");
            parameter.Add("type", (int)item.RideType);
            parameter.Add("id", item.Id);

            switch (item.RideType)
            {
                case RideType.Tradition:
                    await _navigationService.NavigateAsync("TraditionViewCard", parameter);
                    break;
                case RideType.Restaurant:
                    await _navigationService.NavigateAsync("RestaurantViewCard", parameter);
                    break;
            }
        }

    }
}
