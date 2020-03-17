using MyCensus.Models;
using System.Threading.Tasks;

namespace MyCensus.Services
{
    public interface ILocationProvider
    {
        Task<ILocationResponse> GetPositionAsync();
    }
}
