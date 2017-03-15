using Skeleton.Abstraction.Domain;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace Skeleton.Abstraction.Orm
{
    public interface IEntityReader<TEntity> :
            IEntityQuery<TEntity>,
            IEntityAggregate<TEntity>,
            IDisposable,
            IHideObjectMethods
        where TEntity : class, IEntity<TEntity>
    {
        IEntityReader<TEntity> GroupBy(
            Expression<Func<TEntity, object>> expression);

        IEntityReader<TEntity> LeftJoin<TEntity2>(
            Expression<Func<TEntity, TEntity2, bool>> expression)
            where TEntity2 : class, IEntity<TEntity2>;

        IEntityReader<TEntity> RightJoin<TEntity2>(
            Expression<Func<TEntity, TEntity2, bool>> expression)
            where TEntity2 : class, IEntity<TEntity2>;

        IEntityReader<TEntity> InnerJoin<TEntity2>(
            Expression<Func<TEntity, TEntity2, bool>> expression)
            where TEntity2 : class, IEntity<TEntity2>;

        IEntityReader<TEntity> OrderBy(
            Expression<Func<TEntity, object>> expression);

        IEntityReader<TEntity> OrderByDescending(
            Expression<Func<TEntity, object>> expression);

        [SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Select")]
        IEntityReader<TEntity> Select(
            params Expression<Func<TEntity, object>>[] expressions);

        IEntityReader<TEntity> Distinct(
            Expression<Func<TEntity, object>> expression);

        IEntityReader<TEntity> Top(int take);

        IEntityReader<TEntity> Where(
            Expression<Func<TEntity, bool>> expression);

        IEntityReader<TEntity> WhereIsIn(
            Expression<Func<TEntity, object>> expression,
            IEnumerable<object> values);

        IEntityReader<TEntity> WhereNotIn(
            Expression<Func<TEntity, object>> expression,
            IEnumerable<object> values);
    }
}