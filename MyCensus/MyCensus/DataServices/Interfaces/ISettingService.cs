using System.Threading.Tasks;

namespace MyCensus.DataServices
{
    public interface ISettingService
    {
        Task<RestaurantSetting> GetRestaurantSetting();
        Task<SalesProductSetting> GetSalesProductSetting();
        Task<TraditionSetting> GetTraditionSetting();
    }
}