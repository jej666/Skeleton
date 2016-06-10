using System;
using System.Threading.Tasks;

namespace Skeleton.Abstraction
{
    public interface ICacheProvider :
        IHideObjectMethods
    {
        T GetOrAdd<T>(string key, Func<T> valueFactory, Action<ICacheContext> configurator);

        Task<T> GetOrAddAsync<T>(string key, Func<Task<T>> valueFactory, Action<ICacheContext> configurator);

        void Remove(string key);

        bool Contains<T>(string key);
    }
}