using Skeleton.Abstraction.Domain;

namespace Skeleton.Abstraction.Repository
{
    public interface IAsyncCrudRepository<TEntity, TDto> :
            IAsyncReadRepository<TEntity, TDto>
        where TEntity : class, IEntity<TEntity>
        where TDto : class
    {
        IAsyncEntityPersistor<TEntity> Store { get; }
    }
}