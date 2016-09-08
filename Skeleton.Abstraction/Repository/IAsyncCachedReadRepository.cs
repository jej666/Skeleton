namespace Skeleton.Abstraction.Repository
{
    public interface IAsyncCachedReadRepository<TEntity, TDto> :
            IAsyncReadRepository<TEntity, TDto>
        where TEntity : class, IEntity<TEntity>
        where TDto : class
    {
        new IAsyncCachedEntityReader<TEntity> Query { get; }
    }
}