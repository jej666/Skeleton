using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Skeleton.Abstraction.Repository
{
    public interface IEntityReader<TEntity, TIdentity> :
            IEntityQueryResult<TEntity, TIdentity>,
            IEntityAggregateResult<TEntity, TIdentity>,
            IDisposable,
            IHideObjectMethods
        where TEntity : class, IEntity<TEntity, TIdentity>
    {
        IEntityReader<TEntity, TIdentity> GroupBy(
            Expression<Func<TEntity, object>> expression);

        IEntityReader<TEntity, TIdentity> LeftJoin<TEntity2>(
            Expression<Func<TEntity, TEntity2, bool>> expression)
            where TEntity2 : class, IEntity<TEntity2, TIdentity>;

        IEntityReader<TEntity, TIdentity> RightJoin<TEntity2>(
            Expression<Func<TEntity, TEntity2, bool>> expression)
            where TEntity2 : class, IEntity<TEntity2, TIdentity>;

        IEntityReader<TEntity, TIdentity> InnerJoin<TEntity2>(
            Expression<Func<TEntity, TEntity2, bool>> expression)
            where TEntity2 : class, IEntity<TEntity2, TIdentity>;

        IEntityReader<TEntity, TIdentity> CrossJoin<TEntity2>(
            Expression<Func<TEntity, TEntity2, bool>> expression)
            where TEntity2 : class, IEntity<TEntity2, TIdentity>;

        IEntityReader<TEntity, TIdentity> OrderBy(
            Expression<Func<TEntity, object>> expression);

        IEntityReader<TEntity, TIdentity> OrderByDescending(
            Expression<Func<TEntity, object>> expression);

        IEntityReader<TEntity, TIdentity> Select(
            params Expression<Func<TEntity, object>>[] expressions);

        IEntityReader<TEntity, TIdentity> Distinct(
            Expression<Func<TEntity, object>> expression);

        IEntityReader<TEntity, TIdentity> Top(int take);

        IEntityReader<TEntity, TIdentity> Where(
            Expression<Func<TEntity, bool>> expression);

        IEntityReader<TEntity, TIdentity> WhereIsIn(
            Expression<Func<TEntity, object>> expression,
            IEnumerable<object> values);

        IEntityReader<TEntity, TIdentity> WhereNotIn(
            Expression<Func<TEntity, object>> expression,
            IEnumerable<object> values);
    }
}