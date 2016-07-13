using System;
using System.Linq.Expressions;
using Skeleton.Shared.Abstraction;

namespace Skeleton.Core.Repository
{
    public interface IEntityAggregateResult<TEntity, TIdentity>
        where TEntity : class, IEntity<TEntity, TIdentity>
    {
        int Count();

        TResult Average<TResult>(Expression<Func<TEntity, TResult>> expression);

        TResult Count<TResult>(Expression<Func<TEntity, TResult>> expression);

        TResult Max<TResult>(Expression<Func<TEntity, TResult>> expression);

        TResult Min<TResult>(Expression<Func<TEntity, TResult>> expression);

        TResult Sum<TResult>(Expression<Func<TEntity, TResult>> expression);
    }
}