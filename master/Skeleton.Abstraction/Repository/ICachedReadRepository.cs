namespace Skeleton.Abstraction.Repository
{
    public interface ICachedReadRepository<TEntity, TIdentity, TDto> :
            IReadRepository<TEntity, TIdentity, TDto>
        where TEntity : class, IEntity<TEntity, TIdentity>
        where TDto : class
    {
        new ICachedEntityReader<TEntity, TIdentity> Query { get; }
    }
}