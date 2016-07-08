﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Skeleton.Abstraction;
using Skeleton.Abstraction.Reflection;
using Skeleton.Core.Repository;
using Skeleton.Infrastructure.Data;
using Skeleton.Infrastructure.Repository.SqlBuilder;

namespace Skeleton.Infrastructure.Repository
{
    public class ReadRepositoryAsync<TEntity, TIdentity> :
        EntityRepository<TEntity, TIdentity>,
        IReadRepositoryAsync<TEntity, TIdentity>
        where TEntity : class, IEntity<TEntity, TIdentity>
    {
        private readonly IDatabaseAsync _database;

        public ReadRepositoryAsync(
            IMetadataProvider metadataProvider,
            IDatabaseAsync database)
            : base(metadataProvider)
        {
            database.ThrowIfNull(() => database);

            _database = database;
        }

        protected IDatabaseAsync Database
        {
            get { return _database; }
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
                InitializeSqlBuilder();
            }
        }

        public virtual async Task<TEntity> FirstOrDefaultAsync(TIdentity id)
        {
            try
            {
                id.ThrowIfNull(() => id);

                SetWherePrimaryKey(e => e.Id.Equals(id));

                return await FirstOrDefaultAsync();
            }
            finally
            {
                InitializeSqlBuilder();
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
                InitializeSqlBuilder();
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
                InitializeSqlBuilder();
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
                InitializeSqlBuilder();
            }
        }

        public IReadRepositoryAsync<TEntity, TIdentity> GroupBy(
            Expression<Func<TEntity, object>> expression)
        {
            expression.ThrowIfNull(() => expression);
            Builder.GroupBy(expression);

            return this;
        }

        public IReadRepositoryAsync<TEntity, TIdentity> LeftJoin<TEntity2>(
            Expression<Func<TEntity, TEntity2, bool>> expression)
            where TEntity2 : class, IEntity<TEntity2, TIdentity>
        {
            expression.ThrowIfNull(() => expression);
            Builder.Join(expression, JoinType.Left);

            return this;
        }

        public IReadRepositoryAsync<TEntity, TIdentity> RightJoin<TEntity2>(
            Expression<Func<TEntity, TEntity2, bool>> expression)
            where TEntity2 : class, IEntity<TEntity2, TIdentity>
        {
            expression.ThrowIfNull(() => expression);
            Builder.Join(expression, JoinType.Right);

            return this;
        }

        public IReadRepositoryAsync<TEntity, TIdentity> InnerJoin<TEntity2>(
            Expression<Func<TEntity, TEntity2, bool>> expression)
            where TEntity2 : class, IEntity<TEntity2, TIdentity>
        {
            expression.ThrowIfNull(() => expression);
            Builder.Join(expression, JoinType.Inner);

            return this;
        }

        public IReadRepositoryAsync<TEntity, TIdentity> CrossJoin<TEntity2>(
            Expression<Func<TEntity, TEntity2, bool>> expression)
            where TEntity2 : class, IEntity<TEntity2, TIdentity>
        {
            expression.ThrowIfNull(() => expression);
            Builder.Join(expression, JoinType.Cross);

            return this;
        }

        public IReadRepositoryAsync<TEntity, TIdentity> OrderBy(
            Expression<Func<TEntity, object>> expression)
        {
            expression.ThrowIfNull(() => expression);
            Builder.OrderBy(expression);

            return this;
        }

        public IReadRepositoryAsync<TEntity, TIdentity> OrderByDescending(
            Expression<Func<TEntity, object>> expression)
        {
            expression.ThrowIfNull(() => expression);
            Builder.OrderByDescending(expression);

            return this;
        }

        public IReadRepositoryAsync<TEntity, TIdentity> Select(
            params Expression<Func<TEntity, object>>[] expressions)
        {
            expressions.ThrowIfNull(() => expressions);

            foreach (var expression in expressions)
                Builder.Select(expression);

            return this;
        }

        public IReadRepositoryAsync<TEntity, TIdentity> SelectDistinct(
            Expression<Func<TEntity, object>> expression)
        {
            expression.ThrowIfNull(() => expression);
            Builder.SelectWithFunction(expression, SelectFunction.Distinct);

            return this;
        }

        public IReadRepositoryAsync<TEntity, TIdentity> SelectTop(int take)
        {
            Builder.SelectTop(take);

            return this;
        }

        public IReadRepositoryAsync<TEntity, TIdentity> Where(
            Expression<Func<TEntity, bool>> expression)
        {
            expression.ThrowIfNull(() => expression);
            return And(expression);
        }

        public IReadRepositoryAsync<TEntity, TIdentity> WhereIsIn(
            Expression<Func<TEntity, object>> expression, ISqlQuery sqlQuery)
        {
            expression.ThrowIfNull(() => expression);
            Builder.And();
            Builder.QueryByIsIn(expression, sqlQuery);

            return this;
        }

        public IReadRepositoryAsync<TEntity, TIdentity> WhereIsIn(
            Expression<Func<TEntity, object>> expression,
            IEnumerable<object> values)
        {
            expression.ThrowIfNull(() => expression);
            Builder.And();
            Builder.QueryByIsIn(expression, values);

            return this;
        }

        public IReadRepositoryAsync<TEntity, TIdentity> WhereNotIn(
            Expression<Func<TEntity, object>> expression,
            ISqlQuery sqlQuery)
        {
            expression.ThrowIfNull(() => expression);
            Builder.And();
            Builder.QueryByNotIn(expression, sqlQuery);

            return this;
        }

        public IReadRepositoryAsync<TEntity, TIdentity> WhereNotIn(
            Expression<Func<TEntity, object>> expression,
            IEnumerable<object> values)
        {
            expression.ThrowIfNull(() => expression);
            Builder.And();
            Builder.QueryByNotIn(expression, values);

            return this;
        }

        public async Task<TResult> AverageAsync<TResult>(
            Expression<Func<TEntity, TResult>> expression)
        {
            expression.ThrowIfNull(() => expression);
            Builder.SelectWithFunction(expression, SelectFunction.Avg);

            return await AggregateAsAsync<TResult>();
        }

        public async Task<int> CountAsync()
        {
            Builder.SelectCount();

            return await AggregateAsAsync<int>();
        }

        public async Task<TResult> CountAsync<TResult>(
            Expression<Func<TEntity, TResult>> expression)
        {
            expression.ThrowIfNull(() => expression);
            Builder.SelectWithFunction(expression, SelectFunction.Count);

            return await AggregateAsAsync<TResult>();
        }

        public async Task<TResult> MaxAsync<TResult>(
            Expression<Func<TEntity, TResult>> expression)
        {
            expression.ThrowIfNull(() => expression);
            Builder.SelectWithFunction(expression, SelectFunction.Max);

            return await AggregateAsAsync<TResult>();
        }

        public async Task<TResult> MinAsync<TResult>(
            Expression<Func<TEntity, TResult>> expression)
        {
            expression.ThrowIfNull(() => expression);
            Builder.SelectWithFunction(expression, SelectFunction.Min);

            return await AggregateAsAsync<TResult>();
        }

        public async Task<TResult> SumAsync<TResult>(
            Expression<Func<TEntity, TResult>> expression)
        {
            expression.ThrowIfNull(() => expression);
            Builder.SelectWithFunction(expression, SelectFunction.Sum);

            return await AggregateAsAsync<TResult>();
        }

        private void SetWherePrimaryKey(
            Expression<Func<TEntity, bool>> expression)
        {
            expression.ThrowIfNull(() => expression);

            Builder.And();
            Builder.QueryByPrimaryKey(EntityIdName, expression);
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
                InitializeSqlBuilder();
            }
        }

        private IReadRepositoryAsync<TEntity, TIdentity> And(
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