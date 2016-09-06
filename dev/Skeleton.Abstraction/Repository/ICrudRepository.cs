namespace Skeleton.Abstraction.Repository
{
    public interface ICrudRepository<TEntity, TIdentity, TDto> :
            IReadRepository<TEntity, TIdentity, TDto>
        where TEntity : class, IEntity<TEntity, TIdentity>
        where TDto : class
    {
        IEntityPersitor<TEntity, TIdentity> Store { get; }
    }
}