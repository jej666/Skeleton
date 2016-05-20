using System;
using System.Linq.Expressions;
using Skeleton.Common;
using Skeleton.Core.Domain;

namespace Skeleton.Core.Repository
{
    public interface IAggregateBuilder<TEntity, TIdentity> :
        IHideObjectMethods
        where TEntity : class, IEntity<TEntity, TIdentity>
    {
        IAggregateExecutor Average(Expression<Func<TEntity, object>> expression);

        IAggregateExecutor Count(Expression<Func<TEntity, object>> expression);

        IAggregateExecutor Max(Expression<Func<TEntity, object>> expression);

        IAggregateExecutor Min(Expression<Func<TEntity, object>> expression);

        IAggregateExecutor Sum(Expression<Func<TEntity, object>> expression);
    }
}