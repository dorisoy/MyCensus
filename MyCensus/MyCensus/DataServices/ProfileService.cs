using MyCensus.DataServices.Base;
using MyCensus.Helpers;
using MyCensus.Models.Users;
using System;
using System.Threading.Tasks;

namespace MyCensus.DataServices
{
    public class ProfileService : IProfileService
    {
        private readonly IRequestProvider _requestProvider;
        private readonly IAuthenticationService _authenticationService;

        public ProfileService(IRequestProvider requestProvider, IAuthenticationService authenticationService)
        {
            _requestProvider = requestProvider;
            _authenticationService = authenticationService;
        }

        public Task<JH_Auth_User> GetCurrentProfileAsync()
        {
            var userId = _authenticationService.GetCurrentUserId();
            var builder = new UriBuilder(GlobalSettings.AuthenticationEndpoint);
            builder.Path = $"api/user/profiles/{userId}";
            var uri = builder.ToString();
            return _requestProvider.GetAsync<JH_Auth_User>(uri);
        }


        public Task<UserAndProfileModel> SignUp(UserAndProfileModel profile)
        {
            var builder = new UriBuilder(GlobalSettings.AuthenticationEndpoint);
            builder.Path = $"api/Profiles/";
            var uri = builder.ToString();
            return _requestProvider.PostAsync<UserAndProfileModel>(uri, profile);
        }


        public async Task UploadUserImageAsync(JH_Auth_User profile, string imageAsBase64)
        {
            try
            {
                var userId = _authenticationService.GetCurrentUserId();
                var builder = new UriBuilder(GlobalSettings.AuthenticationEndpoint);
                //http://192.168.1.42:8998/api/profiles/image/6917
                builder.Path = $"api/profiles/image/{userId}";
                var uri = builder.ToString();
                var imageModel = new ImageModel
                {
                    Data = imageAsBase64
                };
                var result = await _requestProvider.PutAsync<ImageModel, RequestResult>(uri, imageModel);
                await CacheHelper.RemoveFromCache(profile.tx);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Print(ex.Message);
            }
        }
    }
}