using System;
using System.Linq.Expressions;
using Skeleton.Shared.Abstraction;
using System.Collections.Generic;

namespace Skeleton.Core.Repository
{
    public interface IEntityAggregateResult<TEntity, TIdentity>
        where TEntity : class, IEntity<TEntity, TIdentity>
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