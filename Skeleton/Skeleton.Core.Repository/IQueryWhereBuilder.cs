using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Skeleton.Core.Domain;

namespace Skeleton.Core.Repository
{
    public interface IQueryWhereBuilder<TEntity, TIdentity> 
        where TEntity : class, IEntity<TEntity, TIdentity>
    {
        IQueryBuilder<TEntity, TIdentity> Where(
            Expression<Func<TEntity, bool>> expression);

        IQueryBuilder<TEntity, TIdentity> WhereIsIn(
            Expression<Func<TEntity, object>> expression, 
            ISqlQuery sqlQuery);

        IQueryBuilder<TEntity, TIdentity> WhereIsIn(
            Expression<Func<TEntity, object>> expression, 
            IEnumerable<object> values);

        IQueryBuilder<TEntity, TIdentity> WhereNotIn(
            Expression<Func<TEntity, object>> expression, 
            ISqlQuery sqlQuery);

        IQueryBuilder<TEntity, TIdentity> WhereNotIn(
            Expression<Func<TEntity, object>> expression, 
            IEnumerable<object> values);

        IQueryBuilder<TEntity, TIdentity> WherePrimaryKey(
            Expression<Func<TEntity, bool>> expression);
    }
}