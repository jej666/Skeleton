﻿using Skeleton.Common;
using Skeleton.Core.Domain;

namespace Skeleton.Core.Repository
{
    public interface ICachedRepository<TEntity, TIdentity> :
        IReadOnlyRepository<TEntity, TIdentity>
        where TEntity : class, IEntity<TEntity, TIdentity>
    {
        ICacheProvider Cache { get; }
    }
}