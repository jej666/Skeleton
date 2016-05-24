using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Skeleton.Common;
using Skeleton.Common.Extensions;
using Skeleton.Common.Reflection;
using Skeleton.Core.Domain;
using Skeleton.Core.Repository;
using Skeleton.Infrastructure.Data;
using Skeleton.Infrastructure.Data.Configuration;
using Skeleton.Infrastructure.Repository.SqlBuilder;

namespace Skeleton.Infrastructure.Repository
{
    public abstract class ReadOnlyRepositoryAsyncBase<TEntity, TIdentity> :
        DisposableBase,
        IReadOnlyRepositoryAsync<TEntity, TIdentity>
        where TEntity : class, IEntity<TEntity, TIdentity>
    {
        private readonly IDatabaseAsync _database;
        private readonly ITypeAccessor _typeAccessor;

        protected ReadOnlyRepositoryAsyncBase(
            ITypeAccessorCache typeAccessorCache,
            IDatabaseAsync database)
        {
            typeAccessorCache.ThrowIfNull(() => typeAccessorCache);
            database.ThrowIfNull(() => database);

            _typeAccessor = typeAccessorCache.Get<TEntity>();
            _database = database;
        }

        protected ReadOnlyRepositoryAsyncBase(
            ITypeAccessorCache typeAccessorCache,
            IDatabaseFactory databaseFactory,
            Func<IDatabaseConfigurationBuilder, IDatabaseConfiguration> configurator) :
                this(typeAccessorCache,
                    databaseFactory.CreateDatabaseForAsyncOperations(configurator))
        {
        }

        protected IDatabaseAsync Database
        {
            get { return _database; }
        }

        protected ITypeAccessor TypeAccessor
        {
            get { return _typeAccessor; }
        }

        protected SqlBuilderImpl Builder
        {
            get; private set;
        }

        public ISqlQuery SqlQuery
        {
            get { return Builder; }
        }

        public virtual async Task<IEnumerable<TEntity>> FindAsync()
        {
            try
            {
                return await Database.FindAsync<TEntity>(
                    Builder.Query,
                    Builder.Parameters)
                    .ConfigureAwait(false);
            }
            finally
            {
                InitializeBuilder();
            }
        }

        public virtual async Task<TEntity> FirstOrDefaultAsync(TIdentity id)
        {
            try
            {
                id.ThrowIfNull(() => id);

                WherePrimaryKey(e => e.Id.Equals(id));

                return await FirstOrDefaultAsync();
            }
            finally
            {
                InitializeBuilder();
            }
        }

        public virtual async Task<TEntity> FirstOrDefaultAsync()
        {
            try
            {
                return await Database.FirstOrDefaultAsync<TEntity>(
                    Builder.Query,
                    Builder.Parameters)
                    .ConfigureAwait(false);
            }
            finally
            {
                InitializeBuilder();
            }
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            try
            {
                return await Database.FindAsync<TEntity>(
                    Builder.Query,
                    Builder.Parameters)
                    .ConfigureAwait(false);
            }
            finally
            {
                InitializeBuilder();
            }
        }

        public virtual async Task<IEnumerable<TEntity>> PageAsync(
            int pageSize,
            int pageNumber)
        {
            try
            {
                return await Database.FindAsync<TEntity>(
                    Builder.PagedQuery(pageSize, pageNumber),
                    Builder.Parameters)
                    .ConfigureAwait(false);
            }
            finally
            {
                InitializeBuilder();
            }
        }

        public IReadOnlyRepositoryAsync<TEntity, TIdentity> GroupBy(
            Expression<Func<TEntity, object>> expression)
        {
            expression.ThrowIfNull(() => expression);
            Builder.GroupBy(expression);

            return this;
        }

        public IReadOnlyRepositoryAsync<TEntity, TIdentity> LeftJoin<TEntity2>(
            Expression<Func<TEntity, TEntity2, bool>> expression)
            where TEntity2 : class, IEntity<TEntity2, TIdentity>
        {
            expression.ThrowIfNull(() => expression);
            Builder.Join(expression, JoinType.Left);

            return this;
        }

        public IReadOnlyRepositoryAsync<TEntity, TIdentity> RightJoin<TEntity2>(
            Expression<Func<TEntity, TEntity2, bool>> expression)
            where TEntity2 : class, IEntity<TEntity2, TIdentity>
        {
            expression.ThrowIfNull(() => expression);
            Builder.Join(expression, JoinType.Right);

            return this;
        }

        public IReadOnlyRepositoryAsync<TEntity, TIdentity> InnerJoin<TEntity2>(
            Expression<Func<TEntity, TEntity2, bool>> expression)
            where TEntity2 : class, IEntity<TEntity2, TIdentity>
        {
            expression.ThrowIfNull(() => expression);
            Builder.Join(expression, JoinType.Inner);

            return this;
        }

        public IReadOnlyRepositoryAsync<TEntity, TIdentity> CrossJoin<TEntity2>(
            Expression<Func<TEntity, TEntity2, bool>> expression)
            where TEntity2 : class, IEntity<TEntity2, TIdentity>
        {
            expression.ThrowIfNull(() => expression);
            Builder.Join(expression, JoinType.Cross);

            return this;
        }

        public IReadOnlyRepositoryAsync<TEntity, TIdentity> OrderBy(
            Expression<Func<TEntity, object>> expression)
        {
            expression.ThrowIfNull(() => expression);
            Builder.OrderBy(expression);

            return this;
        }

        public IReadOnlyRepositoryAsync<TEntity, TIdentity> OrderByDescending(
            Expression<Func<TEntity, object>> expression)
        {
            expression.ThrowIfNull(() => expression);
            Builder.OrderByDescending(expression);

            return this;
        }

        public IReadOnlyRepositoryAsync<TEntity, TIdentity> Select(
            params Expression<Func<TEntity, object>>[] expressions)
        {
            expressions.ThrowIfNull(() => expressions);

            foreach (var expression in expressions)
                Builder.Select(expression);

            return this;
        }

        public IReadOnlyRepositoryAsync<TEntity, TIdentity> SelectDistinct(
            Expression<Func<TEntity, object>> expression)
        {
            expression.ThrowIfNull(() => expression);
            Builder.SelectWithFunction(expression, SelectFunction.Distinct);

            return this;
        }

        public IReadOnlyRepositoryAsync<TEntity, TIdentity> SelectTop(int take)
        {
            Builder.SelectTop(take);

            return this;
        }

        public IReadOnlyRepositoryAsync<TEntity, TIdentity> Where(
            Expression<Func<TEntity, bool>> expression)
        {
            expression.ThrowIfNull(() => expression);
            return And(expression);
        }

        public IReadOnlyRepositoryAsync<TEntity, TIdentity> WhereIsIn(
            Expression<Func<TEntity, object>> expression, ISqlQuery sqlQuery)
        {
            expression.ThrowIfNull(() => expression);
            Builder.And();
            Builder.QueryByIsIn(expression, sqlQuery);

            return this;
        }

        public IReadOnlyRepositoryAsync<TEntity, TIdentity> WhereIsIn(
            Expression<Func<TEntity, object>> expression,
            IEnumerable<object> values)
        {
            expression.ThrowIfNull(() => expression);
            Builder.And();
            Builder.QueryByIsIn(expression, values);

            return this;
        }

        public IReadOnlyRepositoryAsync<TEntity, TIdentity> WhereNotIn(
            Expression<Func<TEntity, object>> expression,
            ISqlQuery sqlQuery)
        {
            expression.ThrowIfNull(() => expression);
            Builder.And();
            Builder.QueryByNotIn(expression, sqlQuery);

            return this;
        }

        public IReadOnlyRepositoryAsync<TEntity, TIdentity> WhereNotIn(
            Expression<Func<TEntity, object>> expression,
            IEnumerable<object> values)
        {
            expression.ThrowIfNull(() => expression);
            Builder.And();
            Builder.QueryByNotIn(expression, values);

            return this;
        }

        public async Task<TResult> AverageAsync<TResult>(
            Expression<Func<TEntity, object>> expression)
        {
            expression.ThrowIfNull(() => expression);
            Builder.SelectWithFunction(expression, SelectFunction.Avg);

            return await AggregateAsAsync<TResult>();
        }

        public async Task<TResult> CountAsync<TResult>(
            Expression<Func<TEntity, object>> expression)
        {
            expression.ThrowIfNull(() => expression);
            Builder.SelectWithFunction(expression, SelectFunction.Count);

            return await AggregateAsAsync<TResult>();
        }

        public async Task<TResult> MaxAsync<TResult>(
            Expression<Func<TEntity, object>> expression)
        {
            expression.ThrowIfNull(() => expression);
            Builder.SelectWithFunction(expression, SelectFunction.Max);

            return await AggregateAsAsync<TResult>();
        }

        public async Task<TResult> MinAsync<TResult>(
            Expression<Func<TEntity, object>> expression)
        {
            expression.ThrowIfNull(() => expression);
            Builder.SelectWithFunction(expression, SelectFunction.Min);

            return await AggregateAsAsync<TResult>();
        }

        public async Task<TResult> SumAsync<TResult>(
            Expression<Func<TEntity, object>> expression)
        {
            expression.ThrowIfNull(() => expression);
            Builder.SelectWithFunction(expression, SelectFunction.Sum);

            return await AggregateAsAsync<TResult>();
        }

        protected IReadOnlyRepositoryAsync<TEntity, TIdentity> WherePrimaryKey(
            Expression<Func<TEntity, bool>> expression)
        {
            expression.ThrowIfNull(() => expression);
            var instance = _typeAccessor.CreateInstance<TEntity>();

            Builder.And();
            Builder.QueryByPrimaryKey(instance.IdAccessor.Name, expression);

            return this;
        }

        private async Task<TResult> AggregateAsAsync<TResult>()
        {
            try
            {
                return await Database.ExecuteScalarAsync<TResult>(
                    Builder.Query,
                    Builder.Parameters)
                    .ConfigureAwait(false);
            }
            finally
            {
                InitializeBuilder();
            }
        }

        protected void InitializeBuilder()
        {
            Builder = new SqlBuilderImpl(TableInfo.GetTableName<TEntity>());
        }

        private IReadOnlyRepositoryAsync<TEntity, TIdentity> And(
            Expression<Func<TEntity, bool>> expression)
        {
            expression.ThrowIfNull(() => expression);
            Builder.And();
            Builder.ResolveQuery(expression);

            return this;
        }

        protected override void DisposeManagedResources()
        {
            _database.Dispose();
        }
    }
}