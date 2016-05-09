namespace Skeleton.Core.Repository
{
    using Core.Domain;
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    public interface ISqlExecuteWhereBuilder<TEntity, TIdentity>
        where TEntity : class, IEntity<TEntity, TIdentity>
    {
        ISqlExecuteBuilder<TEntity, TIdentity> Where(
            Expression<Func<TEntity, bool>> expression);

        ISqlExecuteBuilder<TEntity, TIdentity> WhereIsIn(
            Expression<Func<TEntity, object>> expression,
            IEnumerable<object> values);

        ISqlExecuteBuilder<TEntity, TIdentity> WhereNotIn(
            Expression<Func<TEntity, object>> expression,
            IEnumerable<object> values);

        ISqlExecuteBuilder<TEntity, TIdentity> WherePrimaryKey(
            Expression<Func<TEntity, bool>> whereExpression);
    }
}