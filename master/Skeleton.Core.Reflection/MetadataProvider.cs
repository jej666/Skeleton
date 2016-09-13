﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Skeleton.Abstraction;
using Skeleton.Common;

namespace Skeleton.Core.Reflection
{
    public sealed class MetadataProvider :
        HideObjectMethods,
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
            type.ThrowIfNull(() => type);

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