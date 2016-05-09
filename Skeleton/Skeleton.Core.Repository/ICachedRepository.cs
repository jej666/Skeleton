namespace Skeleton.Core.Repository
{
    using Common;
    using Domain;

    public interface ICachedRepository<TEntity, TIdentity> :
        IReadOnlyRepository<TEntity, TIdentity>
        where TEntity : class, IEntity<TEntity, TIdentity>
    {
        ICacheProvider Cache { get; }
    }
}