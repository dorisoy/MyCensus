using MyCensus.DataServices.Base;
using MyCensus.Helpers;
using MyCensus.Models.Users;
using System;
using System.Threading.Tasks;
using MyCensus.DataServices.Interfaces;

namespace MyCensus.DataServices
{

    public class SettingService : ISettingService
    {
        private readonly IRequestProvider _requestProvider;
        private readonly IAuthenticationService _authenticationService;

        public SettingService(IRequestProvider requestProvider, IAuthenticationService authenticationService)
        {
            _requestProvider = requestProvider;
            _authenticationService = authenticationService;
        }


        /// <summary>
        /// 获取传统普查配置
        /// </summary>
        /// <returns></returns>
        public Task<TraditionSetting> GetTraditionSetting()
        {
            var userId = _authenticationService.GetCurrentUserId();
            var builder = new UriBuilder(GlobalSettings.AuthenticationEndpoint);
            builder.Path = $"api/app/android/traditionsetting";
            var uri = builder.ToString();
            return _requestProvider.GetAsync<TraditionSetting>(uri);
        }


        /// <summary>
        /// 获取餐饮普查配置
        /// </summary>
        /// <returns></returns>
        public Task<RestaurantSetting> GetRestaurantSetting()
        {
            var userId = _authenticationService.GetCurrentUserId();
            var builder = new UriBuilder(GlobalSettings.AuthenticationEndpoint);
            builder.Path = $"api/app/android/restaurantsetting";
            var uri = builder.ToString();
            return _requestProvider.GetAsync<RestaurantSetting>(uri);
        }


        /// <summary>
        /// 获取销售普查配置
        /// </summary>
        /// <returns></returns>
        public Task<SalesProductSetting> GetSalesProductSetting()
        {
            var userId = _authenticationService.GetCurrentUserId();
            var builder = new UriBuilder(GlobalSettings.AuthenticationEndpoint);
            builder.Path = $"api/app/android/salesproductsetting";
            var uri = builder.ToString();
            return _requestProvider.GetAsync<SalesProductSetting>(uri);
        }

    }


    public class TraditionSetting 
    {
        public string EndPointType { get; set; } = "|";
        public string ModeOfCooperation { get; set; } = "|";
        public string EndPointStates { get; set; } = "|";

    }


    public class RestaurantSetting 
    {
        public string EndPointType { get; set; } = "|";
        public string Characteristics { get; set; } = "|";
        public string ModeOfCooperation { get; set; } = "|";
        public string EndPointStates { get; set; } = "|";
        public string PerConsumptions { get; set; } = "|";
    }


    public class SalesProductSetting 
    {
        public string Brand { get; set; } = "|";
        public string PackingForm { get; set; } = "|";
        public string ChannelAttributes { get; set; } = "|";
        public string Specification { get; set; } = "|";
    }

}

