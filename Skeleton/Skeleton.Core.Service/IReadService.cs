using System;
using Skeleton.Core.Repository;
using Skeleton.Shared.Abstraction;

namespace Skeleton.Core.Service
{
    public interface IReadService<TEntity, TIdentity, TDto> :
        IDisposable,
        IHideObjectMethods
        where TEntity : class, IEntity<TEntity, TIdentity>
        where TDto : class
    {
        IEntityReader<TEntity, TIdentity> Query { get; }

        IEntityMapper<TEntity, TIdentity, TDto> Mapper { get; }
    }
}