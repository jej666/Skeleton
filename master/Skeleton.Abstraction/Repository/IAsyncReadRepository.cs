using System;

namespace Skeleton.Abstraction.Repository
{
    public interface IAsyncReadRepository<TEntity, TDto> :
            IDisposable,
            IHideObjectMethods
        where TEntity : class, IEntity<TEntity>
        where TDto : class
    {
        IAsyncEntityReader<TEntity> Query { get; }

        IEntityMapper<TEntity, TDto> Mapper { get; }
    }
}