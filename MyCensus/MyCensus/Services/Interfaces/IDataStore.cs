using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace MyCensus.Services
{
    public interface IDataStore<T>
    {
        Task Init();
        Task<IEnumerable<T>> GetStoresAsync(string id);
        Task<T> AddStoreAsync(T store);
        Task<bool> RemoveStoreAsync(string id);
        Task<T> UpdateStoreAsync(T store);
        //Task SyncStoresAsync();

    }
}

