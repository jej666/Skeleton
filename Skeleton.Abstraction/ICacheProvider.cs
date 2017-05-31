using System;

namespace Skeleton.Abstraction
{
    public interface ICacheProvider : IHideObjectMethods
    {
        T GetOrAdd<T>(string key, Func<T> valueFactory, Action<ICacheContext> configurator);

        void Remove(string key);

        bool Contains(string key);
    }
}