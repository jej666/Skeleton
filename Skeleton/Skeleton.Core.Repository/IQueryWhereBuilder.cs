namespace Skeleton.Core.Repository
{
    using Core.Domain;
    using Common;
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    public interface IQueryWhereBuilder<TEntity, TIdentity>:
        IHideObjectMethods
        where TEntity : class, IEntity<TEntity, TIdentity>
    {
        IQueryBuilder<TEntity, TIdentity> Where(
            Expression<Func<TEntity, bool>> expression);

        IQueryBuilder<TEntity, TIdentity> WhereIsIn(
            Expression<Func<TEntity, object>> expression, ISqlQuery sqlQuery);

        IQueryBuilder<TEntity, TIdentity> WhereIsIn(
            Expression<Func<TEntity, object>> expression, IEnumerable<object> values);

        IQueryBuilder<TEntity, TIdentity> WhereNotIn(
            Expression<Func<TEntity, object>> expression, ISqlQuery sqlQuery);

        IQueryBuilder<TEntity, TIdentity> WhereNotIn(
            Expression<Func<TEntity, object>> expression, IEnumerable<object> values);

        IQueryBuilder<TEntity, TIdentity> WherePrimaryKey(
            Expression<Func<TEntity, bool>> expression);
    }
}