using Skeleton.Abstraction.Domain;

namespace Skeleton.Abstraction.Repository
{
    public interface ICrudRepository<TEntity, TDto> :
            IReadRepository<TEntity, TDto>
        where TEntity : class, IEntity<TEntity>
        where TDto : class
    {
        IEntityPersitor<TEntity> Store { get; }
    }
}