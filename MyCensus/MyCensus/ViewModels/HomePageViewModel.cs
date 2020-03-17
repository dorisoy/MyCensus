using Prism;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;

using MyCensus.DataServices.Interfaces;
using MyCensus.Models;
using MyCensus.Models.Events;
using MyCensus.Models.Rides;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using MyCensus.Services;
using MyCensus.Services.Interfaces;
using MyCensus.Models.Users;
using MyCensus.Models.Census;
using MyCensus.Cache;


namespace MyCensus.ViewModels
{
    public class HomePageViewModel : ViewModelBase, IActiveAware
    {
        private readonly IWeatherService _weatherService;
        private readonly IEventsService _eventsService;
        private readonly IRidesService _ridesService;
        private readonly ICensusService _censusService;
        private readonly ICacheManager _cacheManager;


        private DateTime currentDate;
        private string location = "-";
        private string temp = "-";
        private int _totalSum;

        private new INavigationService _navigationService;
        private new IDialogService _dialogService;


        public int TotalSum
        {
            get
            {
                return _totalSum;
            }
            set
            {
                _totalSum = value;
                RaisePropertyChanged(() => TotalSum);
            }
        }

        public DateTime CurrentDate
        {
            get
            {
                return currentDate;
            }

            set
            {
                currentDate = value;
                RaisePropertyChanged(() => CurrentDate);
            }
        }
        public string Location
        {
            get
            {
                return location;
            }

            set
            {
                location = value;
                RaisePropertyChanged(() => Location);
            }
        }
        public string Temp
        {
            get
            {
                return temp;
            }

            set
            {
                temp = value;
                RaisePropertyChanged(() => Temp);
            }
        }
        private ObservableCollection<Event> _events = new ObservableCollection<Event>();
        public ObservableCollection<Event> Events
        {
            get
            {
                return _events;
            }

            set
            {
                _events = value;
                RaisePropertyChanged(() => Events);
            }
        }
        private ObservableCollection<Suggestion> _suggestions = new ObservableCollection<Suggestion>();
        public ObservableCollection<Suggestion> Suggestions
        {
            get
            {
                return _suggestions;
            }

            set
            {
                _suggestions = value;
                RaisePropertyChanged(() => Suggestions);
            }
        }

        private ObservableCollection<Stats> _statistics = new ObservableCollection<Stats>();
        public ObservableCollection<Stats> Statistics {
            get
            {
                return _statistics;
            }

            set
            {
                _statistics = value;
                RaisePropertyChanged(() => Statistics);
            }
        }

        public ICommand ShowEventCommand => new Command<Event>(ShowEventAsync);

        public ICommand ShowCustomRideCommand => new Command(CustomRideAsync);

        //public ICommand ShowRecommendedRideCommand => new Command<Suggestion>(RecommendedRideAsync);

        public HomePageViewModel(IWeatherService weatherService, 
            IEventsService eventsService, 
            IRidesService ridesService,
            ICensusService censusService,
            INavigationService navigationService,
            ICacheManager cacheManager,
            IDialogService dialogService) : base(navigationService, dialogService)
        {
            _navigationService = navigationService;
            _dialogService = dialogService;
            _weatherService = weatherService;
            _eventsService = eventsService;
            _ridesService = ridesService;
            _censusService = censusService;
            _cacheManager = cacheManager;
            Title = "普查终端";
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
                await Initialize(null);
            }
        }

        public  async Task Initialize(object navigationData)
        {
            CurrentDate = GlobalSettings.EventDate;
            IsBusy = true;
            try
            {
                var weather = _weatherService.GetDemoWeatherInfoAsync();
                var events = _eventsService.GetEvents();
                var suggestions = _ridesService.GetSuggestions();

                var tasks = new List<Task> { weather, events, suggestions };
                while (tasks.Count > 0)
                {
                    var finishedTask = await Task.WhenAny(tasks);
                    tasks.Remove(finishedTask);

                    if (finishedTask.Status == TaskStatus.RanToCompletion)
                    {
                        if (finishedTask == weather)
                        {
                            var weatherResults = await weather;
                            if (weatherResults is WeatherInfo)
                            {
                                var weatherInfo = weatherResults as WeatherInfo;
                                Location = weatherInfo.LocationName;
                                Temp = Math.Round(weatherInfo.Temp).ToString();
                            }
                        }
                        else if (finishedTask == events)
                        {
                            var eventsResults = await events;
                            Events = new ObservableCollection<Event>(eventsResults);
                        }
                        else if (finishedTask == suggestions)
                        {
                            var suggestionsResults = await suggestions;
                            Suggestions = new ObservableCollection<Suggestion>(suggestionsResults);
                        }
                    }
                }

                var traditions = new List<Tradition>();
                var restaurants = new List<Restaurant>();


                //销量

                //traditions = await _censusService.GetTraditions(10, 0);
                //restaurants = await _censusService.GetRestaurants(10, 0);

                try
                {
                    var traditionsModel = await _cacheManager.Get<List<Tradition>>(GlobalSettings.traditions_key);
                    if (traditionsModel != null)
                    {
                        traditions = traditionsModel;
                    }
                    else
                    {
                        traditions = await _censusService.GetTraditions(10, 0);
                        await _cacheManager.Set<List<Tradition>>(GlobalSettings.traditions_key, traditions);
                    }

                    //
                    var restaurantsModel = await _cacheManager.Get<List<Restaurant>>(GlobalSettings.restaurants_key);
                    if (restaurantsModel != null)
                    {
                        restaurants = restaurantsModel;
                    }
                    else
                    {
                        restaurants = await _censusService.GetRestaurants(10, 0);
                        await _cacheManager.Set<List<Restaurant>>(GlobalSettings.restaurants_key, restaurants);
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex);
                }

                var ridesResult = new List<Stats>();
                foreach (var t in traditions)
                {
                    if (t != null)
                    {
                        ridesResult.Add(new Stats
                        {
                            Title = t.BaseInfo.EndPointStorsName,
                            Label1 = string.Format("{0}月销量", DateTime.Now.AddMonths(-1).Month),
                            Label2 = string.Format("{0}月销量", DateTime.Now.Month),
                            Value1 = "0",
                            Value2 = "0"
                        });
                    }
                }

                foreach (var t in restaurants)
                {
                    if (t != null)
                    {
                        ridesResult.Add(new Stats
                        {
                            Title = t.BaseInfo.EndPointStorsName,
                            Label1 = string.Format("{0}月", DateTime.Now.AddMonths(-1).Month),
                            Label2 = string.Format("{0}月", DateTime.Now.Month),
                            Value1 = "0",
                            Value2 = "0"
                        });
                    }
                }

                Statistics = new ObservableCollection<Stats>(ridesResult);

                //合计
                TotalSum = 0;
                var statistics = await _censusService.GetStatistics();
                statistics.ForEach(s => {
                    TotalSum += s.EndPointSum;
                });

            }
            catch (Exception ex) when (ex is WebException || ex is HttpRequestException)
            {
                //await _dialogService.ShowAlertAsync("Communication error", "Error", "Ok");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading data in: {ex}");
            }

            IsBusy = false;
        }

        private async void ShowEventAsync(Event @event)
        {
            if (@event != null)
            {
                await _navigationService.NavigateAsync("EventSummaryView");
            }
        }

        private async void CustomRideAsync()
        {
            await _navigationService.NavigateAsync("CustomRideView");
        }

        //private async void RecommendedRideAsync(object obj)
        //{
        //    var suggestion = obj as Suggestion;
        //    if (suggestion != null)
        //    {
        //        var parameters = new CustomRideViewModel.NavigationParameter
        //        {
        //            Latitude = suggestion.Latitude,
        //            Longitude = suggestion.Longitude
        //        };
        //        await _navigationService.NavigateAsync("CustomRideView");
        //    }
        //}
    }



}
