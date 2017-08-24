using Skeleton.Abstraction.Domain;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Skeleton.Abstraction.Orm
{
    public interface IEntityAggregate<TEntity>
        where TEntity : class, IEntity<TEntity>, new()
    {
        int Count();

        IEnumerable<dynamic> Average(
            Expression<Func<TEntity, object>> expression);

        IEnumerable<dynamic> Count(
            Expression<Func<TEntity, object>> expression);

        IEnumerable<dynamic> Max(
            Expression<Func<TEntity, object>> expression);

        IEnumerable<dynamic> Min(
            Expression<Func<TEntity, object>> expression);

        IEnumerable<dynamic> Sum(
            Expression<Func<TEntity, object>> expression);
    }
}