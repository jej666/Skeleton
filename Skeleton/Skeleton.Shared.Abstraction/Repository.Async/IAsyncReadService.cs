using System;
using Skeleton.Core.Repository;
using Skeleton.Shared.Abstraction;

namespace Skeleton.Core.Service
{
    public interface IAsyncReadService<TEntity, TIdentity, TDto> :
        IDisposable,
        IHideObjectMethods
        where TEntity : class, IEntity<TEntity, TIdentity>
        where TDto : class
    {
        IAsyncEntityReader<TEntity, TIdentity> Query { get; }

        IEntityMapper<TEntity, TIdentity, TDto> Mapper { get; }
    }
}