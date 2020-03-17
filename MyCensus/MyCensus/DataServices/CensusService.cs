using System;
using System.Collections.Generic;
using MyCensus.DataServices.Base;
using MyCensus.Models.Users;
using System.Threading.Tasks;
using MyCensus.Models;
using MyCensus.DataServices.Interfaces;
using MyCensus.Models.Census;


using MyCensus.DataServices;
using Xamarin.Forms;


[assembly: Dependency(typeof(CensusService))]
namespace MyCensus.DataServices
{
    public class CensusService : ICensusService
    {
        private readonly IRequestProvider _requestProvider;
        private readonly IAuthenticationService _authenticationService;

        public CensusService(IRequestProvider requestProvider, IAuthenticationService authenticationService)
        {
            _requestProvider = requestProvider;
            _authenticationService = authenticationService;
        }

        public Task<List<Tradition>> GetTraditions(int limit, int pagenumber)
        {
            var userId = _authenticationService.GetCurrentUserId();
            var builder = new UriBuilder(GlobalSettings.AuthenticationEndpoint);
            builder.Path = $"api/census/traditions/{limit}/{pagenumber}/{userId}";
            var uri = builder.ToString();
            return _requestProvider.GetAsync<List<Tradition>>(uri);
        }

        public Task<Tradition> GetTradition(int id)
        {
            var userId = _authenticationService.GetCurrentUserId();
            var builder = new UriBuilder(GlobalSettings.AuthenticationEndpoint);
            builder.Path = $"api/census/get/tradition/{userId}/{id}";
            var uri = builder.ToString();
            return _requestProvider.GetAsync<Tradition>(uri);
        }

        public Task<List<Restaurant>> GetRestaurants(int limit, int pagenumber)
        {
            var userId = _authenticationService.GetCurrentUserId();
            var builder = new UriBuilder(GlobalSettings.AuthenticationEndpoint);
            builder.Path = $"api/census/restaurants/{limit}/{pagenumber}/{userId}";
            var uri = builder.ToString();
            return _requestProvider.GetAsync<List<Restaurant>>(uri);
        }

        public Task<Restaurant> GetRestaurant(int id)
        {
            var userId = _authenticationService.GetCurrentUserId();
            var builder = new UriBuilder(GlobalSettings.AuthenticationEndpoint);
            builder.Path = $"api/census/get/restaurant/{userId}/{id}";
            var uri = builder.ToString();
            return _requestProvider.GetAsync<Restaurant>(uri);
        }

        public Task<List<Statistics>> GetStatistics()
        {
            var userId = _authenticationService.GetCurrentUserId();
            var builder = new UriBuilder(GlobalSettings.AuthenticationEndpoint);
            builder.Path = $"api/census/get/statistics/{userId}";
            var uri = builder.ToString();
            return _requestProvider.GetAsync<List<Statistics>>(uri);
        }

        public Task<RequestResult> AddTradition(Tradition tradition)
        {
            var builder = new UriBuilder(GlobalSettings.AuthenticationEndpoint);
            builder.Path = $"api/Census/AddTradition";
            var uri = builder.ToString();
            return _requestProvider.PostAsync<Tradition, RequestResult>(uri, tradition);
        }

        public Task<RequestResult> AddRestaurant(Restaurant restaurant)
        {
            var builder = new UriBuilder(GlobalSettings.AuthenticationEndpoint);
            builder.Path = $"api/Census/AddRestaurant";
            var uri = builder.ToString();
            return _requestProvider.PostAsync<Restaurant, RequestResult>(uri, restaurant);
        }


        public Task<List<TrackTimeLine>> GetTrackTimeLines()
        {
            var userId = _authenticationService.GetCurrentUserId();
            var builder = new UriBuilder(GlobalSettings.AuthenticationEndpoint);
            builder.Path = $"api/census/get/tracktimelines/{userId}";
            builder.Query = "?date=" + DateTime.Now.ToString("yyyy-MM-dd");
            var uri = builder.ToString();
            return _requestProvider.GetAsync<List<TrackTimeLine>>(uri);
        }

        public Task<RequestResult> AddTrackLocation(TrackLocation trackLocation)
        {
            var builder = new UriBuilder(GlobalSettings.AuthenticationEndpoint);
            builder.Path = $"api/census/add/tracklocation";
            var uri = builder.ToString();
            return _requestProvider.PostAsync<TrackLocation, RequestResult>(uri, trackLocation);
        }
    }


}