﻿using Skeleton.Abstraction;
using Skeleton.Core.Repository;

namespace Skeleton.Core.Service
{
    public interface IReadServiceAsync<TEntity, TIdentity> :
        IEntityService<TEntity, TIdentity>
        where TEntity : class, IEntity<TEntity, TIdentity>
    {
        IReadRepositoryAsync<TEntity, TIdentity> Repository { get; }
    }
}