using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

using Newtonsoft.Json;
using SQLite;
using MyCensus.Database;

namespace MyCensus.Cache
{

    public class CacheManager : ICacheManager
    {
 
        private IDatabaseConnection _databaseConnection;
        protected SQLiteAsyncConnection database;

        public CacheManager(IDatabaseConnection databaseConn)
        {
            //_databaseConnection = databaseConn ?? DependencyService.Get<IDatabaseConnection>();
            _databaseConnection = databaseConn;
            database = _databaseConnection.GetConnection("cache-sqlite_db.db3");
            InitDatabase();
        }

        private void InitDatabase()
        {
            if (database != null)
            {
                Task.Run(() => { database.CreateTableAsync<CacheItem>(); });
            }
        }

        /// <summary>
        ///  根据key 从缓存获取对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<T> Get<T>(string key)
        {
            var item = new CacheItem();
            try
            {
                item = await database.Table<CacheItem>()
                   .Where(x => x.Key == key)
                   .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Print(ex.Message);
            }

            if (item != null)
            {
                try
                {
                    var result = JsonConvert.DeserializeObject<T>(item.Value);
                    return result;
                }
                catch (Exception)
                {
                    return default(T);
                }
            }
            return default(T);
        }

        /// <summary>
        /// 保存缓存对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task Set<T>(string key, T value)
        {
            var item = await database.Table<CacheItem>()
                .Where(x => x.Key == key)
                .FirstOrDefaultAsync();

            var json = JsonConvert.SerializeObject(value);

            if (item == null)
            {
                item = new CacheItem { Key = key, Value = json };
                await database.InsertAsync(item);
                return;
            }

            item.Value = json;
            await database.UpdateAsync(item);
        }

        /// <summary>
        /// 删除指定key 的缓存对象
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<bool> Delete(string key)
        {
            var item = await Get<CacheItem>(key);
            if (item != null)
                return await database.DeleteAsync(item) > 0;
            else
                return false;
        }

        /// <summary>
        /// 删除全部缓存
        /// </summary>
        /// <returns></returns>
        public async Task<bool> DeleteAll()
        {
            string deleteQuery = string.Format("DELETE from [{0}] ", typeof(CacheItem).Name);
            var result = await database.QueryAsync<CacheItem>(deleteQuery);
            return result.Count == 0;
        }
    }
}
