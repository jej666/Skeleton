using System;
using System.Collections.Generic;
using System.Linq.Expressions;
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
    public abstract class ReadOnlyRepositoryBase<TEntity, TIdentity> :
        DisposableBase,
        IReadOnlyRepository<TEntity, TIdentity>
        where TEntity : class, IEntity<TEntity, TIdentity>
    {
        private readonly IDatabase _database;
        private readonly ITypeAccessor _typeAccessor;

        protected ReadOnlyRepositoryBase(
            ITypeAccessorCache typeAccessorCache,
            IDatabase database)
        {
            typeAccessorCache.ThrowIfNull(() => typeAccessorCache);
            database.ThrowIfNull(() => database);

            _typeAccessor = typeAccessorCache.Get<TEntity>();
            _database = database;
            InitializeBuilder();
        }

        protected ReadOnlyRepositoryBase(
            ITypeAccessorCache typeAccessorCache,
            IDatabaseFactory databaseFactory,
            Func<IDatabaseConfigurationBuilder, IDatabaseConfiguration> configurator) :
            this(typeAccessorCache, databaseFactory.CreateDatabase(configurator))
        {
        }

        public ISqlQuery SqlQuery
        {
            get { return Builder; }
        }

        protected IDatabase Database
        {
            get { return _database; }
        }

        protected ITypeAccessor TypeAccessor
        {
            get { return _typeAccessor; }
        }

        protected SqlBuilderImpl Builder
        {
            get;
            private set;
        }

        public virtual IEnumerable<TEntity> Find()
        {
            try
            {
                return Database.Find<TEntity>(
                    Builder.Query,
                    Builder.Parameters);
            }
            finally
            {
                InitializeBuilder();
            }
        }

        public virtual TEntity FirstOrDefault()
        {
            try
            {
                return Database.FirstOrDefault<TEntity>(
                    Builder.Query,
                    Builder.Parameters);
            }
            finally
            {
                InitializeBuilder();
            }
        }

        public virtual TEntity FirstOrDefault(TIdentity id)
        {
            id.ThrowIfNull(() => id);

            WherePrimaryKey(e => e.Id.Equals(id));

            return FirstOrDefault();
        }

        public virtual IEnumerable<TEntity> GetAll()
        {
            try
            {
                return Database.Find<TEntity>(
                    Builder.Query,
                    Builder.Parameters);
            }
            finally
            {
                InitializeBuilder();
            }
        }

        public virtual IEnumerable<TEntity> Page(int pageSize, int pageNumber)
        {
            try
            {
                return Database.Find<TEntity>(
                    Builder.PagedQuery(pageSize, pageNumber),
                    Builder.Parameters);
            }
            finally
            {
                InitializeBuilder();
            }
        }

        public IReadOnlyRepository<TEntity, TIdentity> GroupBy(
            Expression<Func<TEntity, object>> expression)
        {
            expression.ThrowIfNull(() => expression);
            Builder.GroupBy(expression);

            return this;
        }

        public IReadOnlyRepository<TEntity, TIdentity> LeftJoin<TEntity2>(
            Expression<Func<TEntity, TEntity2, bool>> expression)
            where TEntity2 : class, IEntity<TEntity2, TIdentity>
        {
            expression.ThrowIfNull(() => expression);
            Builder.Join(expression, JoinType.Left);

            return this;
        }

        public IReadOnlyRepository<TEntity, TIdentity> RightJoin<TEntity2>(
            Expression<Func<TEntity, TEntity2, bool>> expression)
            where TEntity2 : class, IEntity<TEntity2, TIdentity>
        {
            expression.ThrowIfNull(() => expression);
            Builder.Join(expression,JoinType.Right);

            return this;
        }

        public IReadOnlyRepository<TEntity, TIdentity> InnerJoin<TEntity2>(
            Expression<Func<TEntity, TEntity2, bool>> expression)
            where TEntity2 : class, IEntity<TEntity2, TIdentity>
        {
            expression.ThrowIfNull(() => expression);
            Builder.Join(expression, JoinType.Inner);

            return this;
        }

        public IReadOnlyRepository<TEntity, TIdentity> CrossJoin<TEntity2>(
            Expression<Func<TEntity, TEntity2, bool>> expression)
            where TEntity2 : class, IEntity<TEntity2, TIdentity>
        {
            expression.ThrowIfNull(() => expression);
            Builder.Join(expression, JoinType.Cross);

            return this;
        }

        public IReadOnlyRepository<TEntity, TIdentity> OrderBy(
            Expression<Func<TEntity, object>> expression)
        {
            expression.ThrowIfNull(() => expression);
            Builder.OrderBy(expression);

            return this;
        }

        public IReadOnlyRepository<TEntity, TIdentity> OrderByDescending(
            Expression<Func<TEntity, object>> expression)
        {
            expression.ThrowIfNull(() => expression);
            Builder.OrderByDescending(expression);

            return this;
        }

        public IReadOnlyRepository<TEntity, TIdentity> Select(
            params Expression<Func<TEntity, object>>[] expressions)
        {
            expressions.ThrowIfNull(() => expressions);

            foreach (var expression in expressions)
                Builder.Select(expression);

            return this;
        }

        public IReadOnlyRepository<TEntity, TIdentity> SelectDistinct(
            Expression<Func<TEntity, object>> expression)
        {
            expression.ThrowIfNull(() => expression);
            Builder.SelectWithFunction(expression, SelectFunction.Distinct);

            return this;
        }

        public IReadOnlyRepository<TEntity, TIdentity> SelectTop(int take)
        {
            Builder.SelectTop(take);

            return this;
        }

        public IReadOnlyRepository<TEntity, TIdentity> Where(
            Expression<Func<TEntity, bool>> expression)
        {
            expression.ThrowIfNull(() => expression);
            return And(expression);
        }

        public IReadOnlyRepository<TEntity, TIdentity> WhereIsIn(
            Expression<Func<TEntity, object>> expression, ISqlQuery sqlQuery)
        {
            expression.ThrowIfNull(() => expression);
            Builder.And();
            Builder.QueryByIsIn(expression, sqlQuery);

            return this;
        }

        public IReadOnlyRepository<TEntity, TIdentity> WhereIsIn(
            Expression<Func<TEntity, object>> expression, IEnumerable<object> values)
        {
            expression.ThrowIfNull(() => expression);
            Builder.And();
            Builder.QueryByIsIn(expression, values);

            return this;
        }

        public IReadOnlyRepository<TEntity, TIdentity> WhereNotIn(
            Expression<Func<TEntity, object>> expression, ISqlQuery sqlQuery)
        {
            expression.ThrowIfNull(() => expression);
            Builder.And();
            Builder.QueryByNotIn(expression, sqlQuery);

            return this;
        }

        public IReadOnlyRepository<TEntity, TIdentity> WhereNotIn(
            Expression<Func<TEntity, object>> expression,
            IEnumerable<object> values)
        {
            expression.ThrowIfNull(() => expression);
            Builder.And();
            Builder.QueryByNotIn(expression, values);

            return this;
        }

        protected IReadOnlyRepository<TEntity, TIdentity> WherePrimaryKey(
            Expression<Func<TEntity, bool>> expression)
        {
            expression.ThrowIfNull(() => expression);
            var instance = _typeAccessor.CreateInstance<TEntity>();

            Builder.And();
            Builder.QueryByPrimaryKey(instance.IdAccessor.Name, expression);

            return this;
        }

        public TResult Average<TResult>(Expression<Func<TEntity, object>> expression)
        {
            expression.ThrowIfNull(() => expression);
            Builder.SelectWithFunction(expression, SelectFunction.Avg);

            return AggregateAs<TResult>();
        }

        public TResult Count<TResult>(Expression<Func<TEntity, object>> expression)
        {
            expression.ThrowIfNull(() => expression);
            Builder.SelectWithFunction(expression, SelectFunction.Count);

            return AggregateAs<TResult>();
        }

        public TResult Max<TResult>(Expression<Func<TEntity, object>> expression)
        {
            expression.ThrowIfNull(() => expression);
            Builder.SelectWithFunction(expression, SelectFunction.Max);

            return AggregateAs<TResult>();
        }

        public TResult Min<TResult>(Expression<Func<TEntity, object>> expression)
        {
            expression.ThrowIfNull(() => expression);
            Builder.SelectWithFunction(expression, SelectFunction.Min);

            return AggregateAs<TResult>();
        }

        public TResult Sum<TResult>(Expression<Func<TEntity, object>> expression)
        {
            expression.ThrowIfNull(() => expression);
            Builder.SelectWithFunction(expression, SelectFunction.Sum);

            return AggregateAs<TResult>();
        }

        private TResult AggregateAs<TResult>()
        {
            try
            {
                return Database.ExecuteScalar<TResult>(
                    Builder.Query,
                    Builder.Parameters);
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

        private IReadOnlyRepository<TEntity, TIdentity> And(
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