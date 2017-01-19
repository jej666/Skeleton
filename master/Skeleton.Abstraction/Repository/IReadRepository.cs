using Skeleton.Abstraction.Domain;
using System;

namespace Skeleton.Abstraction.Repository
{
    public interface IReadRepository<TEntity, TDto> :
            IDisposable,
            IHideObjectMethods
        where TEntity : class, IEntity<TEntity>
        where TDto : class
    {
        IEntityReader<TEntity> Query { get; }

        IEntityMapper<TEntity, TDto> Mapper { get; }
    }
}