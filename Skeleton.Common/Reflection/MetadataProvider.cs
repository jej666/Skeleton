using Skeleton.Abstraction.Reflection;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Skeleton.Core.Reflection
{
    public sealed class MetadataProvider :
        HideObjectMethodsBase,
        IMetadataProvider
    {
        private static readonly ConcurrentDictionary<Type, IMetadata> TypeCache =
            new ConcurrentDictionary<Type, IMetadata>();

        public IMetadata GetMetadata<T>()
        {
            return GetMetadata(typeof(T));
        }

        public IMetadata GetMetadata(Type type)
        {
            type.ThrowIfNull(nameof(type));

            return TypeCache.GetOrAdd(type, new Metadata(type));
        }

        public void RemoveMetadata(Type type)
        {
            IMetadata value;
            TypeCache.TryRemove(type, out value);
        }

        public IEnumerable<Type> Types => TypeCache.Keys;
    }
}