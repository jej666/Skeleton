using System;
using System.Threading.Tasks;

namespace Skeleton.Abstraction
{
    public interface IAsyncCacheProvider : IHideObjectMethods
    {
        Task<T> GetOrAddAsync<T>(string key, Func<Task<T>> valueFactory, Action<ICacheContext> configurator);

        void Remove(string key);

        bool Contains(string key);
    }
}