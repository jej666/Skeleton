namespace Skeleton.Core.Repository
{
    using Core.Domain;
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    public interface IExecuteWhereBuilder<TEntity, TIdentity>
        where TEntity : class, IEntity<TEntity, TIdentity>
    {
        IExecuteBuilder<TEntity, TIdentity> Where(
            Expression<Func<TEntity, bool>> expression);

        IExecuteBuilder<TEntity, TIdentity> WhereIsIn(
            Expression<Func<TEntity, object>> expression,
            IEnumerable<object> values);

        IExecuteBuilder<TEntity, TIdentity> WhereNotIn(
            Expression<Func<TEntity, object>> expression,
            IEnumerable<object> values);

        IExecuteBuilder<TEntity, TIdentity> WherePrimaryKey(
            Expression<Func<TEntity, bool>> whereExpression);
    }
}