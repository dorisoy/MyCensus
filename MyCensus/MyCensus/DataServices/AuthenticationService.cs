using MyCensus.DataServices.Base;
using MyCensus.Helpers;
using MyCensus.Models;
using MyCensus.Models.Users;
using System;
using System.Threading.Tasks;

namespace MyCensus.DataServices
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IRequestProvider _requestProvider;

        public bool IsAuthenticated => !string.IsNullOrEmpty(Settings.AccessToken);

        public AuthenticationService(IRequestProvider requestProvider)
        {
            _requestProvider = requestProvider;
        }

        public async Task<bool> LoginAsync(string userName, string password)
        {
            var auth = new AuthenticationRequest
            {
                UserName = userName,
                Credentials = password,
                GrantType = "password"
            };

            UriBuilder builder = new UriBuilder(GlobalSettings.AuthenticationEndpoint);
            builder.Path = "api/user/login";
            string uri = builder.ToString();
            AuthenticationResponse authenticationInfo = await _requestProvider.PostAsync<AuthenticationRequest, AuthenticationResponse>(uri, auth);

            if (authenticationInfo != null && authenticationInfo.User != null)
            {
                Settings.UserId = authenticationInfo.User.ID;
                Settings.ProfileId = authenticationInfo.User.ComId.Value;
                Settings.AccessToken = authenticationInfo.AccessToken;
                Settings.SaleRegion = authenticationInfo.User.MinRegion;
                Settings.SalesDepartment = authenticationInfo.User.SalesDepartment;
                Settings.RegionCode = authenticationInfo.User.BranchCode;
                return true;
            }
            else
                return false;

        }

        public Task LogoutAsync()
        {
            Settings.RemoveUserId();
            Settings.RemoveProfileId();
            Settings.RemoveAccessToken();
            Settings.RemoveSaleRegion();
            Settings.RemoveSalesDepartment();
            Settings.RemoveRegionCode();
            return Task.FromResult(false);
        }

        public int GetCurrentUserId()
        {
            return Settings.UserId;
        }
    }
}

