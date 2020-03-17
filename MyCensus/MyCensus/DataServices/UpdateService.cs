using MyCensus.DataServices.Base;
using MyCensus.Helpers;
using MyCensus.Models.Users;
using System;
using System.Threading.Tasks;
using MyCensus.DataServices.Interfaces;

namespace MyCensus.DataServices
{
    public class UpdateService: IUpdateService
    {
        private readonly IRequestProvider _requestProvider;
        private readonly IAuthenticationService _authenticationService;

        public UpdateService(IRequestProvider requestProvider, IAuthenticationService authenticationService)
        {
            _requestProvider = requestProvider;
            _authenticationService = authenticationService;
        }

        public Task<UpdateInfo> GetCurrentVersion()
        {
            //GET /api/app/android/checkupdate
            var userId = _authenticationService.GetCurrentUserId();
            var builder = new UriBuilder(GlobalSettings.AuthenticationEndpoint);
            builder.Path = $"api/app/android/checkupdate";
            var uri = builder.ToString();
            return _requestProvider.GetAsync<UpdateInfo>(uri);
        }

    }
}