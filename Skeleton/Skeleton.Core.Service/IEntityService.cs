using System;
using Skeleton.Shared.Abstraction;

namespace Skeleton.Core.Service
{
    public interface IEntityService<TEntity, TIdentity> :
        IDisposable,
        IHideObjectMethods
        where TEntity : class, IEntity<TEntity, TIdentity>
    {
        ILogger Logger { get; }
    }
}