using FFImageLoading.Cache;
using FFImageLoading.Forms;
using System.Threading.Tasks;

namespace MyCensus.Helpers
{
    /// <summary>
    /// 用于图片缓存辅助
    /// </summary>
    public class CacheHelper
    {
        public static async Task RemoveFromCache(string url)
        {
            await CachedImage.InvalidateCache(url, CacheType.All, true);
        }
    }
}
