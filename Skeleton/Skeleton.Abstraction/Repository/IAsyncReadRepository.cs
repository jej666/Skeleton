using System;

namespace Skeleton.Abstraction.Repository
{
    public interface IAsyncReadRepository<TEntity, TIdentity, TDto> :
            IDisposable,
            IHideObjectMethods
        where TEntity : class, IEntity<TEntity, TIdentity>
        where TDto : class
    {
        IAsyncEntityReader<TEntity, TIdentity> Query { get; }

        IEntityMapper<TEntity, TIdentity, TDto> Mapper { get; }
    }
}