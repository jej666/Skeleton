using System;
using System.Collections.Concurrent;
using Skeleton.Common.Extensions;

namespace Skeleton.Common.Reflection
{
    public class TypeAccessorCache : ITypeAccessorCache
    {
        private static readonly ConcurrentDictionary<Type, ITypeAccessor> TypeCache =
            new ConcurrentDictionary<Type, ITypeAccessor>();

        public ITypeAccessor Get<T>()
        {
            return Get(typeof(T));
        }

        public ITypeAccessor Get(Type type)
        {
            type.ThrowIfNull(() => type);

            return TypeCache.GetOrAdd(type, new TypeAccessor(type));
        }

        public void Remove(Type type)
        {
            ITypeAccessor value;
            TypeCache.TryRemove(type, out value);
        }
    }
}