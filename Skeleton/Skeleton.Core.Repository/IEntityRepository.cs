using System;
using Skeleton.Abstraction;

namespace Skeleton.Core.Repository
{
    public interface IEntityRepository<TEntity, TIdentity> :
        IDisposable,
        IHideObjectMethods
        where TEntity : class, IEntity<TEntity, TIdentity>
    {
        Type EntityType { get; }
    }
}