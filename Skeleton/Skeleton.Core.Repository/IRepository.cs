namespace Skeleton.Core.Repository
{
    using Core.Domain;
    using System.Collections.Generic;

    public interface IRepository<TEntity, TIdentity> :
        IReadOnlyRepository<TEntity, TIdentity>
        where TEntity : class, IEntity<TEntity, TIdentity>
    {
        bool Add(TEntity entity);

        bool Add(IEnumerable<TEntity> entities);

        bool Delete(TEntity entity);

        bool Delete(IEnumerable<TEntity> entities);

        bool Save(TEntity entity);

        bool Save(IEnumerable<TEntity> entities);

        bool Update(TEntity entity);

        bool Update(IEnumerable<TEntity> entities);
    }
}