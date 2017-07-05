using System;
using System.Threading.Tasks;

namespace Skeleton.Abstraction
{
    public interface IAsyncCacheProvider : IHideObjectMethods
    {
        Task<T> GetOrAddAsync<T>(string key, Func<Task<T>> valueFactory, Action<ICacheConfiguration> configurator);

        Task RemoveAsync(string key);

        Task<bool> ContainsAsync(string key);
    }
}