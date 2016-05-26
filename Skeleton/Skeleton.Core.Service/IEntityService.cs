using System;
using Skeleton.Common;
using Skeleton.Core.Domain;
using Skeleton.Core.Repository;

namespace Skeleton.Core.Service
{
    public interface IEntityService<TEntity, TIdentity> :
        IDisposable,
        IHideObjectMethods
        where TEntity : class, IEntity<TEntity, TIdentity>
    {
    }
}