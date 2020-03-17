using MyCensus.Models.Users;
using System.Threading.Tasks;

namespace MyCensus.DataServices
{
    public interface IProfileService
    {
        Task<JH_Auth_User> GetCurrentProfileAsync();

        Task<UserAndProfileModel> SignUp(UserAndProfileModel profile);

        Task UploadUserImageAsync(JH_Auth_User profile, string imageAsBase64);
    }
}