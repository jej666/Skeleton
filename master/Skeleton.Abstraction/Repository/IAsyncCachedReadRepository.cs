namespace Skeleton.Abstraction.Repository
{
    public interface IAsyncCachedReadRepository<TEntity, TIdentity, TDto> :
            IAsyncReadRepository<TEntity, TIdentity, TDto>
        where TEntity : class, IEntity<TEntity, TIdentity>
        where TDto : class
    {
        new IAsyncCachedEntityReader<TEntity, TIdentity> Query { get; }
    }
}