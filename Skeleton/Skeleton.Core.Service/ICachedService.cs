using System;
using Skeleton.Common;
using Skeleton.Core.Domain;
using Skeleton.Core.Repository;

namespace Skeleton.Core.Service
{
    public interface ICachedService<TEntity, TIdentity> :
        IEntityService<TEntity, TIdentity>
        where TEntity : class, IEntity<TEntity, TIdentity>
    {
        ICachedRepository<TEntity, TIdentity> CachedRepository { get; }
    }
}