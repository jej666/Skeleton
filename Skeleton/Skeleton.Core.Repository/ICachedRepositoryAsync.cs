﻿using Skeleton.Common;
using Skeleton.Core.Domain;

namespace Skeleton.Core.Repository
{
    public interface ICachedRepositoryAsync<TEntity, TIdentity> :
        IReadOnlyRepositoryAsync<TEntity, TIdentity>
        where TEntity : class, IEntity<TEntity, TIdentity>
    {
        ICacheProvider Cache { get; }
    }
}