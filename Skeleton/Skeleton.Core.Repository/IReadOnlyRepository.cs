using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Skeleton.Common;
using Skeleton.Core.Domain;

namespace Skeleton.Core.Repository
{
    public interface IReadOnlyRepository<TEntity, TIdentity> :
        IDisposable,
        IHideObjectMethods,
        IQuery<TEntity, TIdentity>,
        IAggregate<TEntity, TIdentity>
        where TEntity : class, IEntity<TEntity, TIdentity>
    {
        ISqlQuery SqlQuery { get; }

        IReadOnlyRepository<TEntity, TIdentity> GroupBy(
            Expression<Func<TEntity, object>> expression);

        //IReadOnlyRepository<TEntity2, TIdentity2> Join<TEntity2, TIdentity2>(
        //    Expression<Func<TEntity, TEntity2, bool>> expression)
        //    where TEntity2 : class, IEntity<TEntity2, TIdentity2>;

        IReadOnlyRepository<TEntity, TIdentity> OrderBy(
            Expression<Func<TEntity, object>> expression);

        IReadOnlyRepository<TEntity, TIdentity> OrderByDescending(
            Expression<Func<TEntity, object>> expression);

        IReadOnlyRepository<TEntity, TIdentity> Select(
            params Expression<Func<TEntity, object>>[] expressions);

        IReadOnlyRepository<TEntity, TIdentity> SelectDistinct(
            Expression<Func<TEntity, object>> expression);

        IReadOnlyRepository<TEntity, TIdentity> SelectTop(int take);

        IReadOnlyRepository<TEntity, TIdentity> Where(
            Expression<Func<TEntity, bool>> expression);

        //IReadOnlyRepository<TEntity, TIdentity> WhereIsIn(
        //    Expression<Func<TEntity, object>> expression,
        //    ISqlQuery sqlQuery);

        IReadOnlyRepository<TEntity, TIdentity> WhereIsIn(
            Expression<Func<TEntity, object>> expression,
            IEnumerable<object> values);

        //IReadOnlyRepository<TEntity, TIdentity> WhereNotIn(
        //    Expression<Func<TEntity, object>> expression,
        //    ISqlQuery sqlQuery);

        IReadOnlyRepository<TEntity, TIdentity> WhereNotIn(
            Expression<Func<TEntity, object>> expression,
            IEnumerable<object> values);

        IReadOnlyRepository<TEntity, TIdentity> WherePrimaryKey(
            Expression<Func<TEntity, bool>> expression);
    }
}