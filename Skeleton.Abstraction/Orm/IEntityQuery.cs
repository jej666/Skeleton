using Skeleton.Abstraction.Domain;
using System.Collections.Generic;

namespace Skeleton.Abstraction.Orm
{
    public interface IEntityQuery<TEntity>
        where TEntity : class, IEntity<TEntity>, new()
    {
        TEntity FirstOrDefault();

        TEntity FirstOrDefault(object id);

        IEnumerable<TEntity> Find();

        IEnumerable<TEntity> Query(IQuery query);
    }
}