namespace Skeleton.Core.Repository
{
    using Common;
    using Domain;

    public interface ICachedRepositoryAsync<TEntity, TIdentity> :
        IReadOnlyRepositoryAsync<TEntity, TIdentity>
        where TEntity : class, IEntity<TEntity, TIdentity>
    {
        ICacheProvider Cache { get; }
    }
}