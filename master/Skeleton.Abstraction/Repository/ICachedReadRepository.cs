namespace Skeleton.Abstraction.Repository
{
    public interface ICachedReadRepository<TEntity, TDto> :
            IReadRepository<TEntity, TDto>
        where TEntity : class, IEntity<TEntity>
        where TDto : class
    {
        new ICachedEntityReader<TEntity> Query { get; }
    }
}