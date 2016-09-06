using System.Collections.Generic;

namespace Skeleton.Abstraction.Repository
{
    public interface IEntityQueryResult<out TEntity, in TIdentity>
        where TEntity : class, IEntity<TEntity, TIdentity>
    {
        TEntity FirstOrDefault();

        TEntity FirstOrDefault(TIdentity id);

        IEnumerable<TEntity> Find();

        IEnumerable<TEntity> GetAll();

        IEnumerable<TEntity> Page(int pageSize, int pageNumber);
    }
}