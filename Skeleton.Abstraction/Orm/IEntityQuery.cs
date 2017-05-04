using Skeleton.Abstraction.Domain;
using System.Collections.Generic;

namespace Skeleton.Abstraction.Orm
{
    public interface IEntityQuery<out TEntity>
        where TEntity : class, IEntity<TEntity>
    {
        TEntity FirstOrDefault();

        TEntity FirstOrDefault(object id);

        IEnumerable<TEntity> Find();

        IEnumerable<TEntity> Query(IQuery query);
    }
}