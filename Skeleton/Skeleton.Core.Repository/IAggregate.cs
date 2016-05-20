using System;
using System.Linq.Expressions;
using Skeleton.Core.Domain;

namespace Skeleton.Core.Repository
{
    public interface IAggregate<TEntity, TIdentity>
        where TEntity : class, IEntity<TEntity, TIdentity>
    {
        TResult Average<TResult>(Expression<Func<TEntity, object>> expression);

        TResult Count<TResult>(Expression<Func<TEntity, object>> expression);

        TResult Max<TResult>(Expression<Func<TEntity, object>> expression);

        TResult Min<TResult>(Expression<Func<TEntity, object>> expression);

        TResult Sum<TResult>(Expression<Func<TEntity, object>> expression);
    }
}