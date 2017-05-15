using System;
using System.Threading.Tasks;

namespace Skeleton.Abstraction
{
    public interface IAsyncCacheProvider : IHideObjectMethods
    {
        Task<T> GetOrAddAsync<T>(string key, Func<Task<T>> valueFactory, Action<ICacheContext> configurator);

        Task RemoveAsync(string key);

        Task<bool> ContainsAsync(string key);
    }
}