﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Skeleton.Abstraction.Repository
{
    public interface IAsyncEntityReader<TEntity, TIdentity> :
            IAsyncEntityQueryResult<TEntity, TIdentity>,
            IAsyncEntityAggregateResult<TEntity, TIdentity>,
            IDisposable,
            IHideObjectMethods
        where TEntity : class, IEntity<TEntity, TIdentity>
    {
        IAsyncEntityReader<TEntity, TIdentity> GroupBy(
            Expression<Func<TEntity, object>> expression);

        IAsyncEntityReader<TEntity, TIdentity> LeftJoin<TEntity2>(
            Expression<Func<TEntity, TEntity2, bool>> expression)
            where TEntity2 : class, IEntity<TEntity2, TIdentity>;

        IAsyncEntityReader<TEntity, TIdentity> RightJoin<TEntity2>(
            Expression<Func<TEntity, TEntity2, bool>> expression)
            where TEntity2 : class, IEntity<TEntity2, TIdentity>;

        IAsyncEntityReader<TEntity, TIdentity> InnerJoin<TEntity2>(
            Expression<Func<TEntity, TEntity2, bool>> expression)
            where TEntity2 : class, IEntity<TEntity2, TIdentity>;

        IAsyncEntityReader<TEntity, TIdentity> CrossJoin<TEntity2>(
            Expression<Func<TEntity, TEntity2, bool>> expression)
            where TEntity2 : class, IEntity<TEntity2, TIdentity>;

        IAsyncEntityReader<TEntity, TIdentity> OrderBy(
            Expression<Func<TEntity, object>> expression);

        IAsyncEntityReader<TEntity, TIdentity> OrderByDescending(
            Expression<Func<TEntity, object>> expression);

        IAsyncEntityReader<TEntity, TIdentity> Select(
            params Expression<Func<TEntity, object>>[] expressions);

        IAsyncEntityReader<TEntity, TIdentity> Distinct(
            Expression<Func<TEntity, object>> expression);

        IAsyncEntityReader<TEntity, TIdentity> Top(int take);

        IAsyncEntityReader<TEntity, TIdentity> Where(
            Expression<Func<TEntity, bool>> expression);

        IAsyncEntityReader<TEntity, TIdentity> WhereIsIn(
            Expression<Func<TEntity, object>> expression,
            IEnumerable<object> values);

        IAsyncEntityReader<TEntity, TIdentity> WhereNotIn(
            Expression<Func<TEntity, object>> expression,
            IEnumerable<object> values);
    }
}