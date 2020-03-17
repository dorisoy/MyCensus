using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyCensus.Cache
{
    public interface ICacheManager
    {
        /// <summary>
        /// 根据key 从缓存获取对象
        /// </summary>
        Task<T> Get<T>(string key);

        /// <summary>
        /// 保存缓存对象
        /// </summary>
        Task Set<T>(string key, T value);

        /// <summary>
        /// 删除指定key 的缓存对象
        /// </summary>
        Task<bool> Delete(string key);

        /// <summary>
        /// 删除全部缓存
        /// </summary>
        Task<bool> DeleteAll();
    }
}
