using MyCensus.DataServices.Base;
using MyCensus.DataServices.Interfaces;
using MyCensus.Helpers;
using MyCensus.Models;
using MyCensus.Models.Events;
using MyCensus.Models.Rides;
using MyCensus.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using MyCensus.Models.Census;

namespace MyCensus.DataServices
{
    public class RidesService : IRidesService
    {
        private readonly IRequestProvider _requestProvider;
        private readonly IAuthenticationService _authenticationService;

        private static List<Suggestion> suggestions = StaticData.GetSuggestions();

        private static int StationsCounter = 0;

        private static List<Station> stations = new List<Station>
        {
            new Station
            {
                Name = "雪花啤酒西安分公司",
                Slots = 22,
                Occupied = 4,
                Latitude = 47.5790791f,
                Longitude = -122.4136163f
            },
            new Station
            {
                Name = "雪花啤酒西安分公司",
                Slots = 12,
                Occupied = 7,
                Latitude = 47.5743905f,
                Longitude = -122.4023376f
            },
            new Station
            {
                Name = "雪花啤酒西安分公司",
                Slots = 15,
                Occupied = 5,
                Latitude = 47.5766275f,
                Longitude = -122.4217906f
            }
        };

        private static List<Ride> rides = new List<Ride>
        {
            //new Ride
            //{
            //     Id=8001,
            //    EventId = 1,
            //    RideType = RideType.Tradition,
            //    Name = "2018传统普查",
            //    Start = DateTime.Now.AddDays(-7),
            //    Stop = DateTime.Now.AddDays(-7),
            //    Duration = 3600,
            //    Distance = 54577,
            //    From = stations[0].Name,
            //    FromStation = stations[0],
            //    To = stations[2].Name,
            //    ToStation = stations[2],
            //    TraditionInfo=new Tradition()
            //    {
            //        BaseInfo=new TraditionBaseInfo()
            //        {
            //            EndPointStorsName="爱购便利连锁",
            //            EndPointAddress="陕西省西安市经济技术开发区凤城9路38号",
            //            DoorheadPhotos=new List<DoorheadPhoto>(){ new DoorheadPhoto() {  StoragePath= "logo.png" } }
            //        },
            //        BusinessInfo =new TraditionBusinessInfo(){
            //            EndPointType="X"
            //        }
            //    },
            //    ThumbnailPhoto = "shop.jpg"
            //},
            //new Ride
            //{
            //     Id=8002,
            //    EventId = 1,
            //    RideType = RideType.Restaurant,
            //    Name = "2018传统普查",
            //    Start = DateTime.Now.AddDays(-7),
            //    Stop = DateTime.Now.AddDays(-7),
            //    Duration = 3600,
            //    Distance = 1900,
            //    From = stations[0].Name,
            //    FromStation = stations[0],
            //    To = stations[2].Name,
            //    ToStation = stations[2],
            //    TraditionInfo=new Tradition()
            //    {
            //        BaseInfo=new TraditionBaseInfo()
            //        {
            //            EndPointStorsName="互惠超市",
            //            EndPointAddress="陕西省西安市经济技术开发区凤城9路38号",
            //            DoorheadPhotos=new List<DoorheadPhoto>(){ new DoorheadPhoto() {  StoragePath= "logo.png" } }
            //        },
            //        BusinessInfo =new TraditionBusinessInfo(){
            //            EndPointType="X"
            //        }
            //    },
            //    ThumbnailPhoto = "shop.jpg"
            //},
            //new Ride
            //{
            //     Id=8003,
            //    EventId = 1,
            //    RideType = RideType.Tradition,
            //    Name = "2018传统普查",
            //    Start = DateTime.Now.AddDays(-7),
            //    Stop = DateTime.Now.AddDays(-7),
            //    Duration = 3600,
            //    Distance = 2356,
            //    From = stations[0].Name,
            //    FromStation = stations[0],
            //    To = stations[2].Name,
            //    ToStation = stations[2],
            //    TraditionInfo=new Tradition()
            //    {
            //        BaseInfo=new TraditionBaseInfo()
            //        {
            //            EndPointStorsName="华联超市",
            //            EndPointAddress="陕西省西安市经济技术开发区凤城9路38号",
            //            DoorheadPhotos=new List<DoorheadPhoto>(){ new DoorheadPhoto() {  StoragePath= "logo.png" } }
            //        },
            //        BusinessInfo =new TraditionBusinessInfo(){
            //            EndPointType="X"
            //        }
            //    },
            //    ThumbnailPhoto = "shop.jpg"
            //},
            //new Ride
            //{
            //     Id=8004,
            //    EventId = 1,
            //    RideType = RideType.Tradition,
            //    Name = "2018传统普查",
            //    Start = DateTime.Now.AddDays(-7),
            //    Stop = DateTime.Now.AddDays(-7),
            //    Duration = 3600,
            //    Distance = 1254,
            //    From = stations[0].Name,
            //    FromStation = stations[0],
            //    To = stations[2].Name,
            //    ToStation = stations[2],
            //    TraditionInfo=new Tradition()
            //    {
            //        BaseInfo=new TraditionBaseInfo()
            //        {
            //            EndPointStorsName="天下客便利连锁",
            //            EndPointAddress="陕西省西安市经济技术开发区凤城9路38号",
            //            DoorheadPhotos=new List<DoorheadPhoto>(){ new DoorheadPhoto() {  StoragePath= "logo.png" } }
            //        },
            //        BusinessInfo =new TraditionBusinessInfo(){
            //            EndPointType="X"
            //        }
            //    },
            //    ThumbnailPhoto = "shop.jpg"
            //},
            //new Ride
            //{
            //    Id=8005,
            //    EventId = 1,
            //    RideType = RideType.Restaurant,
            //    Name = "2018传统普查",
            //    Start = DateTime.Now.AddDays(-7),
            //    Stop = DateTime.Now.AddDays(-7),
            //    Duration = 3600,
            //    Distance = 2134,
            //    From = stations[0].Name,
            //    FromStation = stations[0],
            //    To = stations[2].Name,
            //    ToStation = stations[2],
            //    TraditionInfo=new Tradition()
            //    {
            //        BaseInfo=new TraditionBaseInfo()
            //        {
            //            EndPointStorsName="海天便利连锁",
            //            EndPointAddress="陕西省西安市经济技术开发区凤城9路38号",
            //            DoorheadPhotos=new List<DoorheadPhoto>(){ new DoorheadPhoto() {  StoragePath= "logo.png" } }
            //        },
            //        BusinessInfo =new TraditionBusinessInfo(){
            //            EndPointType="X"
            //        }
            //    },
            //    ThumbnailPhoto = "noimg.jpg"
            //},
            //new Ride
            //{
            //     Id=8006,
            //    EventId = 1,
            //    RideType = RideType.Restaurant,
            //    Name = "2018传统普查",
            //    Start = DateTime.Now.AddDays(-7),
            //    Stop = DateTime.Now.AddDays(-7),
            //    Duration = 3600,
            //    Distance = 1345,
            //    From = stations[0].Name,
            //    FromStation = stations[0],
            //    To = stations[2].Name,
            //    ToStation = stations[2],
            //    TraditionInfo=new Tradition()
            //    {
            //        BaseInfo=new TraditionBaseInfo()
            //        {
            //            EndPointStorsName="柠檬便利",
            //            EndPointAddress="陕西省西安市经济技术开发区凤城9路38号",
            //            DoorheadPhotos=new List<DoorheadPhoto>(){ new DoorheadPhoto() {  StoragePath= "logo.png" } }
            //        },
            //        BusinessInfo =new TraditionBusinessInfo(){
            //            EndPointType="X"
            //        }
            //    },
            //    ThumbnailPhoto = "noimg.jpg"
            //}

        };

        public RidesService(IRequestProvider requestProvider, IAuthenticationService authenticationService)
        {
            _requestProvider = requestProvider;
            _authenticationService = authenticationService;
        }

        public Task<Booking> RequestBikeBooking(Station station, Event @event)
        {
            return BikeBooking(station, RideType.Event, @event.Id);
        }

        public Task<Booking> RequestBikeBooking(Station station, Suggestion suggestion)
        {
            return BikeBooking(station, RideType.Suggestion, suggestion.Id);
        }

        public Task<Booking> RequestBikeBooking(Station fromStation, Station toStation)
        {
            return BikeBooking(fromStation, RideType.Custom, 0);
        }

        private async Task<Booking> BikeBooking(Station station, RideType type, int id)
        {
            await Task.Delay(500);

            var booking = new Booking
            {
                Id = 222,
                FromStation = station,
                ToStation = stations[0],
                RegistrationDate = DateTime.UtcNow,
                DueDate = DateTime.UtcNow.AddMinutes(3),
                EventId = id,
                BikeId = 2332,
                RideType = type
            };

            Settings.CurrentBookingId = booking.Id;

            return booking;
        }

        public async Task<Booking> GetBooking(int bookingId)
        {
            await Task.Delay(500);

            var booking = new Booking
            {
                Id = bookingId,
                FromStation = stations[0],
                ToStation = stations[1],
                RegistrationDate = DateTime.UtcNow,
                DueDate = DateTime.UtcNow.AddMinutes(3),
                EventId = 1,
                BikeId = 2332,
                RideType = RideType.Event
            };

            return booking;
        }

        public Task<Station> GetNearestStationTo(GeoLocation location)
        {
            var station = stations[StationsCounter++ % stations.Count()];

            return Task.FromResult(station);
        }

        public async Task<IEnumerable<Suggestion>> GetSuggestions()
        {
            await Task.Delay(200);
            return suggestions;
        }

        public async Task<IEnumerable<Ride>> GetUserRides()
        {
            var userId = _authenticationService.GetCurrentUserId();

            //UriBuilder builder = new UriBuilder(GlobalSettings.RidesEndpoint);
            //builder.Path = $"api/rides/user/{userId}";
            //string uri = builder.ToString();
            //IEnumerable<Ride> rides = await _requestProvider.GetAsync<IEnumerable<Ride>>(uri);
            await Task.Delay(50);
            return rides;
        }

        public void RemoveCurrentBooking()
        {
            Settings.RemoveCurrentBookingId();
        }

        public async Task<IEnumerable<Station>> GetNearestStations()
        {
            await Task.Delay(200);

            return stations;
        }

        public async Task<IEnumerable<Station>> GetNearestStationsTo(GeoLocation location)
        {
            try
            {
                const int count = 10;
                var userId = _authenticationService.GetCurrentUserId();

                UriBuilder builder = new UriBuilder(GlobalSettings.RidesEndpoint);
                builder.Path = $"/api/stations/nearto?latitude={location.Latitude.ToString(CultureInfo.InvariantCulture)}&longitude={location.Longitude.ToString(CultureInfo.InvariantCulture)}&count={count}";

                string uri = builder.ToString();

                IEnumerable<Station> stations = await _requestProvider.GetAsync<IEnumerable<Station>>(uri);

                return stations;
            }
            catch
            {
                await Task.Delay(200);

                return stations;
            }
        }

        public async Task<Station> GetInfoForNearestStation()
        {
            await Task.Delay(500);

            return stations.FirstOrDefault();
        }

        public async Task<Station> GetInfoForNearestStationTo(GeoLocation toGeoLocation)
        {
            await Task.Delay(500);

            return stations.FirstOrDefault();
        }

        public async Task<Station> GetStation(int stationId)
        {
            await Task.Delay(500);

            return stations.FirstOrDefault();
        }

        public Task<Booking> RequestBikeBooking(Station fromStation, Station toStation, Event @event)
        {
            return BikeBooking(fromStation, RideType.Event, @event.Id);
        }
    }
}
