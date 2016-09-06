namespace Skeleton.Abstraction.Repository
{
    public interface IAsyncCrudRepository<TEntity, TIdentity, TDto> :
            IAsyncReadRepository<TEntity, TIdentity, TDto>
        where TEntity : class, IEntity<TEntity, TIdentity>
        where TDto : class
    {
        IAsyncEntityPersistor<TEntity, TIdentity> Store { get; }
    }
}