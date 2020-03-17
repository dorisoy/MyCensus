using MyCensus.Models;
using System.Threading.Tasks;

namespace MyCensus.DataServices.Interfaces
{
    public interface IWeatherService
    {
        Task<IWeatherResponse> GetWeatherInfoAsync();
        Task<IWeatherResponse> GetDemoWeatherInfoAsync();
    }
}
