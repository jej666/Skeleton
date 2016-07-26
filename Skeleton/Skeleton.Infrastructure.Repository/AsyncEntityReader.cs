using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Skeleton.Core.Repository;
using Skeleton.Infrastructure.Data;
using Skeleton.Infrastructure.Repository.SqlBuilder;
using Skeleton.Shared.Abstraction;
using Skeleton.Shared.Abstraction.Reflection;

namespace Skeleton.Infrastructure.Repository
{
    public class AsyncEntityReader<TEntity, TIdentity> :
        DisposableBase,
        IAsyncEntityReader<TEntity, TIdentity>
        where TEntity : class, IEntity<TEntity, TIdentity>
    {
        private readonly IDatabaseAsync _database;
        private readonly SelectQueryBuilder<TEntity, TIdentity> _builder;

        public AsyncEntityReader(
            IMetadataProvider metadataProvider,
            IDatabaseAsync database)
        {
            _database = database;
            _builder = new SelectQueryBuilder<TEntity, TIdentity>(metadataProvider);
        }

        protected IDatabaseAsync Database
        {
            get { return _database; }
        }

        internal SelectQueryBuilder<TEntity, TIdentity> Builder
        {
            get { return _builder; }
        }

        public virtual async Task<IEnumerable<TEntity>> FindAsync()
        {
            try
            {
                return await Database.FindAsync<TEntity>(
                    Builder.SqlQuery,
                    Builder.Parameters)
                    .ConfigureAwait(false);
            }
            finally { Builder.OnNextQuery(); }
        }

        public virtual async Task<TEntity> FirstOrDefaultAsync(TIdentity id)
        {
            try
            {
                id.ThrowIfNull(() => id);

                Builder.QueryByPrimaryKey(
                    e => e.Id.Equals(id));

                return await FirstOrDefaultAsync();
            }
            finally { Builder.OnNextQuery(); }
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
            finally { Builder.OnNextQuery(); }
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
            finally { Builder.OnNextQuery(); }
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
            finally { Builder.OnNextQuery(); }
        }

        public IAsyncEntityReader<TEntity, TIdentity> GroupBy(
           Expression<Func<TEntity, object>> expression)
        {
            expression.ThrowIfNull(() => expression);
            Builder.GroupBy(expression);

            return this;
        }

        public IAsyncEntityReader<TEntity, TIdentity> LeftJoin<TEntity2>(
            Expression<Func<TEntity, TEntity2, bool>> expression)
            where TEntity2 : class, IEntity<TEntity2, TIdentity>
        {
            expression.ThrowIfNull(() => expression);
            Builder.Join(expression, JoinType.Left);

            return this;
        }

        public IAsyncEntityReader<TEntity, TIdentity> RightJoin<TEntity2>(
            Expression<Func<TEntity, TEntity2, bool>> expression)
            where TEntity2 : class, IEntity<TEntity2, TIdentity>
        {
            expression.ThrowIfNull(() => expression);
            Builder.Join(expression, JoinType.Right);

            return this;
        }

        public IAsyncEntityReader<TEntity, TIdentity> InnerJoin<TEntity2>(
            Expression<Func<TEntity, TEntity2, bool>> expression)
            where TEntity2 : class, IEntity<TEntity2, TIdentity>
        {
            expression.ThrowIfNull(() => expression);
            Builder.Join(expression, JoinType.Inner);

            return this;
        }

        public IAsyncEntityReader<TEntity, TIdentity> CrossJoin<TEntity2>(
            Expression<Func<TEntity, TEntity2, bool>> expression)
            where TEntity2 : class, IEntity<TEntity2, TIdentity>
        {
            expression.ThrowIfNull(() => expression);
            Builder.Join(expression, JoinType.Cross);

            return this;
        }

        public IAsyncEntityReader<TEntity, TIdentity> OrderBy(
            Expression<Func<TEntity, object>> expression)
        {
            expression.ThrowIfNull(() => expression);
            Builder.OrderBy(expression);

            return this;
        }

        public IAsyncEntityReader<TEntity, TIdentity> OrderByDescending(
            Expression<Func<TEntity, object>> expression)
        {
            expression.ThrowIfNull(() => expression);
            Builder.OrderByDescending(expression);

            return this;
        }

        public IAsyncEntityReader<TEntity, TIdentity> Select(
            params Expression<Func<TEntity, object>>[] expressions)
        {
            expressions.ThrowIfNull(() => expressions);

            foreach (var expression in expressions)
                Builder.Select(expression);

            return this;
        }

        public IAsyncEntityReader<TEntity, TIdentity> Distinct(
            Expression<Func<TEntity, object>> expression)
        {
            expression.ThrowIfNull(() => expression);
            Builder.Aggregate(expression, SelectFunction.Distinct);

            return this;
        }

        public IAsyncEntityReader<TEntity, TIdentity> Top(int take)
        {
            Builder.Top(take);

            return this;
        }

        public IAsyncEntityReader<TEntity, TIdentity> Where(
            Expression<Func<TEntity, bool>> expression)
        {
            expression.ThrowIfNull(() => expression);
            return And(expression);
        }

        public IAsyncEntityReader<TEntity, TIdentity> WhereIsIn(
            Expression<Func<TEntity, object>> expression,
            IEnumerable<object> values)
        {
            expression.ThrowIfNull(() => expression);
            Builder.WhereIsIn(expression, values);

            return this;
        }

        public IAsyncEntityReader<TEntity, TIdentity> WhereNotIn(
            Expression<Func<TEntity, object>> expression,
            IEnumerable<object> values)
        {
            expression.ThrowIfNull(() => expression);
            Builder.WhereNotIn(expression, values);

            return this;
        }

        private IAsyncEntityReader<TEntity, TIdentity> And(
            Expression<Func<TEntity, bool>> expression)
        {
            expression.ThrowIfNull(() => expression);
            Builder.ResolveQuery(expression);

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
            finally { Builder.OnNextQuery(); }
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

        private async Task<IEnumerable<dynamic>> AggregateAsync()
        {
            try
            {
                return await Database.FindAsync(
                    Builder.SqlQuery,
                    Builder.Parameters)
                    .ConfigureAwait(false);
            }
            finally { Builder.OnNextQuery(); }
        }

        protected override void DisposeManagedResources()
        {
            _database.Dispose();
        }
    }
}