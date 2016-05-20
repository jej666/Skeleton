﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Skeleton.Core.Domain;

namespace Skeleton.Core.Repository
{
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