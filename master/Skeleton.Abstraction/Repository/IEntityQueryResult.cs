using System.Collections.Generic;

namespace Skeleton.Abstraction.Repository
{
    public interface IEntityQueryResult<out TEntity>
        where TEntity : class, IEntity<TEntity>
    {
        TEntity FirstOrDefault();

        TEntity FirstOrDefault(object id);

        IEnumerable<TEntity> Find();

        IEnumerable<TEntity> Page(int pageSize, int pageNumber);
    }
}