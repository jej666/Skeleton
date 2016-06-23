using System;
using Skeleton.Abstraction;

namespace Skeleton.Core.Repository
{
    public interface IEntityRepository<TEntity, TIdentity> :
        IEntityRepository
        where TEntity : class, IEntity<TEntity, TIdentity>
    {
    }

    public interface IEntityRepository :
        IDisposable,
        IHideObjectMethods
    {
    }
}