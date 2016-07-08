using System;
using System.Linq.Expressions;
using Skeleton.Shared.Abstraction;

namespace Skeleton.Core.Repository
{
    public interface IAggregate<TEntity, TIdentity>
        where TEntity : class, IEntity<TEntity, TIdentity>
    {
        TResult Average<TResult>(Expression<Func<TEntity, TResult>> expression);

        int Count();

        TResult Count<TResult>(Expression<Func<TEntity, TResult>> expression);

        TResult Max<TResult>(Expression<Func<TEntity, TResult>> expression);

        TResult Min<TResult>(Expression<Func<TEntity, TResult>> expression);

        TResult Sum<TResult>(Expression<Func<TEntity, TResult>> expression);
    }
}