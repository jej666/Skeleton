using Skeleton.Abstraction;
using Skeleton.Abstraction.Data;
using Skeleton.Abstraction.Domain;
using Skeleton.Abstraction.Orm;
using Skeleton.Abstraction.Reflection;
using Skeleton.Common;
using Skeleton.Infrastructure.Orm.SqlBuilder;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Skeleton.Infrastructure.Orm
{
    public class AsyncEntityReader<TEntity> :
            DisposableBase,
            IAsyncEntityReader<TEntity>
        where TEntity : class, IEntity<TEntity>
    {
        private readonly IMetadataProvider _metadataProvider;
        private readonly IAsyncDatabase _database;

        public AsyncEntityReader(
            IMetadataProvider metadataProvider,
            IAsyncDatabase database)
        {
            _metadataProvider = metadataProvider;
            _database = database;
            Builder = new SelectQueryBuilder<TEntity>(metadataProvider);
        }

        internal SelectQueryBuilder<TEntity> Builder { get; }

        public virtual async Task<IEnumerable<TEntity>> FindAsync()
        {
            try
            {
                return await _database.FindAsync<TEntity>(
                    Builder.SqlCommand)
                    .ConfigureAwait(false);
            }
            finally
            {
                Builder.OnNextQuery();
            }
        }

        public virtual async Task<TEntity> FirstOrDefaultAsync(object id)
        {
            id.ThrowIfNull(nameof(id));

            Builder.QueryByPrimaryKey(
                e => e.Id.Equals(id));

            return await FirstOrDefaultCoreAsync();
        }

        public virtual async Task<TEntity> FirstOrDefaultAsync()
        {
            return await FirstOrDefaultCoreAsync();
        }

        public virtual async Task<IEnumerable<TEntity>> QueryAsync(IQuery query)
        {
            try
            {
                var evaluator = new QueryEvaluator<TEntity>(Builder, query);
                evaluator.Evaluate();

                return await _database.FindAsync<TEntity>(
                        Builder.SqlCommand)
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
            expression.ThrowIfNull(nameof(expression));
            Builder.GroupBy(expression);

            return this;
        }

        public IAsyncEntityReader<TEntity> LeftJoin<TEntity2>(
            Expression<Func<TEntity, TEntity2, bool>> expression)
            where TEntity2 : class, IEntity<TEntity2>
        {
            expression.ThrowIfNull(nameof(expression));
            Builder.Join(expression, JoinType.Left);

            return this;
        }

        public IAsyncEntityReader<TEntity> RightJoin<TEntity2>(
            Expression<Func<TEntity, TEntity2, bool>> expression)
            where TEntity2 : class, IEntity<TEntity2>
        {
            expression.ThrowIfNull(nameof(expression));
            Builder.Join(expression, JoinType.Right);

            return this;
        }

        public IAsyncEntityReader<TEntity> InnerJoin<TEntity2>(
            Expression<Func<TEntity, TEntity2, bool>> expression)
            where TEntity2 : class, IEntity<TEntity2>
        {
            expression.ThrowIfNull(nameof(expression));
            Builder.Join(expression, JoinType.Inner);

            return this;
        }

        public IAsyncEntityReader<TEntity> OrderBy(
            Expression<Func<TEntity, object>> expression)
        {
            expression.ThrowIfNull(nameof(expression));
            Builder.OrderBy(expression);

            return this;
        }

        public IAsyncEntityReader<TEntity> OrderByDescending(
            Expression<Func<TEntity, object>> expression)
        {
            expression.ThrowIfNull(nameof(expression));
            Builder.OrderByDescending(expression);

            return this;
        }

        public IAsyncEntityReader<TEntity> Select(
            params Expression<Func<TEntity, object>>[] expressions)
        {
            expressions.ThrowIfNull(nameof(expressions));

            foreach (var expression in expressions)
                Builder.Select(expression);

            return this;
        }

        public IAsyncEntityReader<TEntity> Distinct(
            Expression<Func<TEntity, object>> expression)
        {
            expression.ThrowIfNull(nameof(expression));
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
            expression.ThrowIfNull(nameof(expression));
            return And(expression);
        }

        public IAsyncEntityReader<TEntity> WhereIsIn(
            Expression<Func<TEntity, object>> expression,
            IEnumerable<object> values)
        {
            expression.ThrowIfNull(nameof(expression));
            Builder.WhereIsIn(expression, values);

            return this;
        }

        public IAsyncEntityReader<TEntity> WhereNotIn(
            Expression<Func<TEntity, object>> expression,
            IEnumerable<object> values)
        {
            expression.ThrowIfNull(nameof(expression));
            Builder.WhereNotIn(expression, values);

            return this;
        }

        public async Task<IEnumerable<dynamic>> AverageAsync(
            Expression<Func<TEntity, object>> expression)
        {
            expression.ThrowIfNull(nameof(expression));
            Builder.Aggregate(expression, SelectFunction.Avg);

            return await AggregateAsync();
        }

        public async Task<int> CountAsync()
        {
            try
            {
                Builder.Count();

                return await _database.ExecuteScalarAsync<int>(
                        Builder.SqlCommand)
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
            expression.ThrowIfNull(nameof(expression));
            Builder.Aggregate(expression, SelectFunction.Count);

            return await AggregateAsync();
        }

        public async Task<IEnumerable<dynamic>> MaxAsync(
            Expression<Func<TEntity, object>> expression)
        {
            expression.ThrowIfNull(nameof(expression));
            Builder.Aggregate(expression, SelectFunction.Max);

            return await AggregateAsync();
        }

        public async Task<IEnumerable<dynamic>> MinAsync(
            Expression<Func<TEntity, object>> expression)
        {
            expression.ThrowIfNull(nameof(expression));
            Builder.Aggregate(expression, SelectFunction.Min);

            return await AggregateAsync();
        }

        public async Task<IEnumerable<dynamic>> SumAsync(
            Expression<Func<TEntity, object>> expression)
        {
            expression.ThrowIfNull(nameof(expression));
            Builder.Aggregate(expression, SelectFunction.Sum);

            return await AggregateAsync();
        }

        private async Task<TEntity> FirstOrDefaultCoreAsync()
        {
            try
            {
                return await _database.FirstOrDefaultAsync<TEntity>(
                        Builder.SqlCommand)
                    .ConfigureAwait(false);
            }
            finally
            {
                Builder.OnNextQuery();
            }
        }

        protected override void DisposeManagedResources()
        {
            _database.Dispose();
            base.DisposeManagedResources();
        }

        private IAsyncEntityReader<TEntity> And(
            Expression<Func<TEntity, bool>> expression)
        {
            expression.ThrowIfNull(nameof(expression));
            Builder.ResolveQuery(expression);

            return this;
        }

        private async Task<IEnumerable<dynamic>> AggregateAsync()
        {
            try
            {
                return await _database.FindAsync(
                        Builder.SqlCommand)
                    .ConfigureAwait(false);
            }
            finally
            {
                Builder.OnNextQuery();
            }
        }
    }
}