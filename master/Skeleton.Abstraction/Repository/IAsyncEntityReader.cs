using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Skeleton.Abstraction.Repository
{
    public interface IAsyncEntityReader<TEntity> :
            IAsyncEntityQueryResult<TEntity>,
            IAsyncEntityAggregateResult<TEntity>,
            IDisposable,
            IHideObjectMethods
        where TEntity : class, IEntity<TEntity>
    {
        IAsyncEntityReader<TEntity> GroupBy(
            Expression<Func<TEntity, object>> expression);

        IAsyncEntityReader<TEntity> LeftJoin<TEntity2>(
            Expression<Func<TEntity, TEntity2, bool>> expression)
            where TEntity2 : class, IEntity<TEntity2>;

        IAsyncEntityReader<TEntity> RightJoin<TEntity2>(
            Expression<Func<TEntity, TEntity2, bool>> expression)
            where TEntity2 : class, IEntity<TEntity2>;

        IAsyncEntityReader<TEntity> InnerJoin<TEntity2>(
            Expression<Func<TEntity, TEntity2, bool>> expression)
            where TEntity2 : class, IEntity<TEntity2>;

        IAsyncEntityReader<TEntity> CrossJoin<TEntity2>(
            Expression<Func<TEntity, TEntity2, bool>> expression)
            where TEntity2 : class, IEntity<TEntity2>;

        IAsyncEntityReader<TEntity> OrderBy(
            Expression<Func<TEntity, object>> expression);

        IAsyncEntityReader<TEntity> OrderByDescending(
            Expression<Func<TEntity, object>> expression);

        IAsyncEntityReader<TEntity> Select(
            params Expression<Func<TEntity, object>>[] expressions);

        IAsyncEntityReader<TEntity> Distinct(
            Expression<Func<TEntity, object>> expression);

        IAsyncEntityReader<TEntity> Top(int take);

        IAsyncEntityReader<TEntity> Where(
            Expression<Func<TEntity, bool>> expression);

        IAsyncEntityReader<TEntity> WhereIsIn(
            Expression<Func<TEntity, object>> expression,
            IEnumerable<object> values);

        IAsyncEntityReader<TEntity> WhereNotIn(
            Expression<Func<TEntity, object>> expression,
            IEnumerable<object> values);
    }
}