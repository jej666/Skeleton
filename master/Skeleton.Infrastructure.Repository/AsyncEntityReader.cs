﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Skeleton.Abstraction;
using Skeleton.Abstraction.Data;
using Skeleton.Abstraction.Repository;
using Skeleton.Common;
using Skeleton.Infrastructure.Repository.SqlBuilder;

namespace Skeleton.Infrastructure.Repository
{
    public class AsyncEntityReader<TEntity> :
            DisposableBase,
            IAsyncEntityReader<TEntity>
        where TEntity : class, IEntity<TEntity>
    {
        public AsyncEntityReader(
            IMetadataProvider metadataProvider,
            IDatabaseAsync database)
        {
            Builder = new SelectQueryBuilder<TEntity>(metadataProvider);
            Database = database;
        }

        protected IDatabaseAsync Database { get; }

        internal SelectQueryBuilder<TEntity> Builder { get; }

        public virtual async Task<IEnumerable<TEntity>> FindAsync()
        {
            try
            {
                return await Database.FindAsync<TEntity>(
                        Builder.SqlQuery,
                        Builder.Parameters)
                    .ConfigureAwait(false);
            }
            finally
            {
                Builder.OnNextQuery();
            }
        }

        public virtual async Task<TEntity> FirstOrDefaultAsync(object id)
        {
            try
            {
                id.ThrowIfNull(() => id);

                Builder.QueryByPrimaryKey(
                    e => e.Id.Equals(id));

                return await FirstOrDefaultAsync();
            }
            finally
            {
                Builder.OnNextQuery();
            }
        }

        public virtual async Task<TEntity> FirstOrDefaultAsync()
        {
            try
            {
                return await Database.FirstOrDefaultAsync<TEntity>(
                        Builder.SqlQuery,
                        Builder.Parameters)
                    .ConfigureAwait(false);
            }
            finally
            {
                Builder.OnNextQuery();
            }
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            try
            {
                return await Database.FindAsync<TEntity>(
                        Builder.SqlQuery,
                        Builder.Parameters)
                    .ConfigureAwait(false);
            }
            finally
            {
                Builder.OnNextQuery();
            }
        }

        public virtual async Task<IEnumerable<TEntity>> PageAsync(
            int pageSize,
            int pageNumber)
        {
            try
            {
                return await Database.FindAsync<TEntity>(
                        Builder.SqlPagedQuery(pageSize, pageNumber),
                        Builder.Parameters)
                    .ConfigureAwait(false);
            }
            finally
            {
                Builder.OnNextQuery();
            }
        }

        public IAsyncEntityReader<TEntity> GroupBy(
            Expression<Func<TEntity, object>> expression)
        {
            expression.ThrowIfNull(() => expression);
            Builder.GroupBy(expression);

            return this;
        }

        public IAsyncEntityReader<TEntity> LeftJoin<TEntity2>(
            Expression<Func<TEntity, TEntity2, bool>> expression)
            where TEntity2 : class, IEntity<TEntity2>
        {
            expression.ThrowIfNull(() => expression);
            Builder.Join(expression, JoinType.Left);

            return this;
        }

        public IAsyncEntityReader<TEntity> RightJoin<TEntity2>(
            Expression<Func<TEntity, TEntity2, bool>> expression)
            where TEntity2 : class, IEntity<TEntity2>
        {
            expression.ThrowIfNull(() => expression);
            Builder.Join(expression, JoinType.Right);

            return this;
        }

        public IAsyncEntityReader<TEntity> InnerJoin<TEntity2>(
            Expression<Func<TEntity, TEntity2, bool>> expression)
            where TEntity2 : class, IEntity<TEntity2>
        {
            expression.ThrowIfNull(() => expression);
            Builder.Join(expression, JoinType.Inner);

            return this;
        }

        public IAsyncEntityReader<TEntity> OrderBy(
            Expression<Func<TEntity, object>> expression)
        {
            expression.ThrowIfNull(() => expression);
            Builder.OrderBy(expression);

            return this;
        }

        public IAsyncEntityReader<TEntity> OrderByDescending(
            Expression<Func<TEntity, object>> expression)
        {
            expression.ThrowIfNull(() => expression);
            Builder.OrderByDescending(expression);

            return this;
        }

        public IAsyncEntityReader<TEntity> Select(
            params Expression<Func<TEntity, object>>[] expressions)
        {
            expressions.ThrowIfNull(() => expressions);

            foreach (var expression in expressions)
                Builder.Select(expression);

            return this;
        }

        public IAsyncEntityReader<TEntity> Distinct(
            Expression<Func<TEntity, object>> expression)
        {
            expression.ThrowIfNull(() => expression);
            Builder.Aggregate(expression, SelectFunction.Distinct);

            return this;
        }

        public IAsyncEntityReader<TEntity> Top(int take)
        {
            Builder.Top(take);

            return this;
        }

        public IAsyncEntityReader<TEntity> Where(
            Expression<Func<TEntity, bool>> expression)
        {
            expression.ThrowIfNull(() => expression);
            return And(expression);
        }

        public IAsyncEntityReader<TEntity> WhereIsIn(
            Expression<Func<TEntity, object>> expression,
            IEnumerable<object> values)
        {
            expression.ThrowIfNull(() => expression);
            Builder.WhereIsIn(expression, values);

            return this;
        }

        public IAsyncEntityReader<TEntity> WhereNotIn(
            Expression<Func<TEntity, object>> expression,
            IEnumerable<object> values)
        {
            expression.ThrowIfNull(() => expression);
            Builder.WhereNotIn(expression, values);

            return this;
        }

        public async Task<IEnumerable<dynamic>> AverageAsync(
            Expression<Func<TEntity, object>> expression)
        {
            expression.ThrowIfNull(() => expression);
            Builder.Aggregate(expression, SelectFunction.Avg);

            return await AggregateAsync();
        }

        public async Task<int> CountAsync()
        {
            try
            {
                Builder.Count();

                return await Database.ExecuteScalarAsync<int>(
                        Builder.SqlQuery,
                        Builder.Parameters)
                    .ConfigureAwait(false);
            }
            finally
            {
                Builder.OnNextQuery();
            }
        }

        public async Task<IEnumerable<dynamic>> CountAsync(
            Expression<Func<TEntity, object>> expression)
        {
            expression.ThrowIfNull(() => expression);
            Builder.Aggregate(expression, SelectFunction.Count);

            return await AggregateAsync();
        }

        public async Task<IEnumerable<dynamic>> MaxAsync(
            Expression<Func<TEntity, object>> expression)
        {
            expression.ThrowIfNull(() => expression);
            Builder.Aggregate(expression, SelectFunction.Max);

            return await AggregateAsync();
        }

        public async Task<IEnumerable<dynamic>> MinAsync(
            Expression<Func<TEntity, object>> expression)
        {
            expression.ThrowIfNull(() => expression);
            Builder.Aggregate(expression, SelectFunction.Min);

            return await AggregateAsync();
        }

        public async Task<IEnumerable<dynamic>> SumAsync(
            Expression<Func<TEntity, object>> expression)
        {
            expression.ThrowIfNull(() => expression);
            Builder.Aggregate(expression, SelectFunction.Sum);

            return await AggregateAsync();
        }

        protected override void DisposeManagedResources()
        {
            Database.Dispose();
        }

        private IAsyncEntityReader<TEntity> And(
            Expression<Func<TEntity, bool>> expression)
        {
            expression.ThrowIfNull(() => expression);
            Builder.ResolveQuery(expression);

            return this;
        }

        private async Task<IEnumerable<dynamic>> AggregateAsync()
        {
            try
            {
                return await Database.FindAsync(
                        Builder.SqlQuery,
                        Builder.Parameters)
                    .ConfigureAwait(false);
            }
            finally
            {
                Builder.OnNextQuery();
            }
        }
    }
}