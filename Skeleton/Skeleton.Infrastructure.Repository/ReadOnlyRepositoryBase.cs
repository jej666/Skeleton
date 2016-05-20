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
using Skeleton.Infrastructure.Repository.SqlBuilder.ExpressionTree;

namespace Skeleton.Infrastructure.Repository
{
    public abstract class ReadOnlyRepositoryBase<TEntity, TIdentity> :
        DisposableBase,
        IReadOnlyRepository<TEntity, TIdentity>
        where TEntity : class, IEntity<TEntity, TIdentity>
    {
        private readonly IDatabase _database;
        private readonly ITypeAccessor _typeAccessor;
        private SqlBuilderBase _builder;
        
        protected ReadOnlyRepositoryBase(
            ITypeAccessorCache typeAccessorCache,
            IDatabase database)
        {
            typeAccessorCache.ThrowIfNull(() => typeAccessorCache);
            database.ThrowIfNull(() => database);

            _typeAccessor = typeAccessorCache.Get<TEntity>();
            _database = database;
            _builder = CreateBuilder();
        }

        protected ReadOnlyRepositoryBase(
            ITypeAccessorCache typeAccessorCache,
            IDatabaseFactory databaseFactory,
            Func<IDatabaseConfigurationBuilder, IDatabaseConfiguration> configurator) :
                this(typeAccessorCache, databaseFactory.CreateDatabase(configurator))
        {
        }

        internal SqlBuilderBase CreateBuilder()
        {
            return new SqlBuilderBase(TableInfo.GetTableName<TEntity>());
        }

        internal IDatabase Database
        {
            get { return _database; }
        }

        internal ITypeAccessor TypeAccessor
        {
            get { return _typeAccessor; }
        }

        public IEnumerable<TEntity> Find()
        {
            try
            {
                return Database.Find<TEntity>(_builder.Query, _builder.Parameters);
            }
            finally
            {
                _builder = CreateBuilder();
            }
        }

        public TEntity FirstOrDefault()
        {
            try
            {
                return Database.FirstOrDefault<TEntity>(_builder.Query, _builder.Parameters);
            }
            finally
            {
                _builder = CreateBuilder();
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
            return Database.Find<TEntity>(_builder.Query, _builder.Parameters);
        }

        public virtual IEnumerable<TEntity> Page(int pageSize, int pageNumber)
        {
            try
            {
             return Database.Find<TEntity>(
                _builder.PagedQuery(pageSize, pageNumber),
                _builder.Parameters);
            }
            finally
            {
                _builder = CreateBuilder();
            }
        }

        public IReadOnlyRepository<TEntity, TIdentity> GroupBy(Expression<Func<TEntity, object>> expression)
        {
            expression.ThrowIfNull(() => expression);
            _builder.Resolver.GroupBy(expression);

            return this;
        }

        //public IQueryBuilder<TEntity2, TIdentity2> Join<TEntity2, TIdentity2>(
        //    Expression<Func<TEntity, TEntity2, bool>> expression)
        //    where TEntity2 : class, IEntity<TEntity2, TIdentity2>
        //{
        //    expression.ThrowIfNull(() => expression);

        //    var joinQuery = new SqlQueryBuilder<TEntity2, TIdentity2>(Builder, Resolver);
        //    Resolver.Join(expression);

        //    return joinQuery;
        //}

        public IReadOnlyRepository<TEntity, TIdentity> OrderBy(
            Expression<Func<TEntity, object>> expression)
        {
            expression.ThrowIfNull(() => expression);
            _builder.Resolver.OrderBy(expression);

            return this;
        }

        public IReadOnlyRepository<TEntity, TIdentity> OrderByDescending(
            Expression<Func<TEntity, object>> expression)
        {
            expression.ThrowIfNull(() => expression);
            _builder.Resolver.OrderByDescending(expression);

            return this;
        }

        public IReadOnlyRepository<TEntity, TIdentity> Select(
            params Expression<Func<TEntity, object>>[] expressions)
        {
            expressions.ThrowIfNull(() => expressions);

            foreach (var expression in expressions)
                _builder.Resolver.Select(expression);

            return this;
        }

        public IReadOnlyRepository<TEntity, TIdentity> SelectDistinct(
            Expression<Func<TEntity, object>> expression)
        {
            expression.ThrowIfNull(() => expression);
            _builder.Resolver.SelectWithFunction(expression, SelectFunction.DISTINCT);

            return this;
        }

        public IReadOnlyRepository<TEntity, TIdentity> SelectTop(int take)
        {
            _builder.Resolver.SelectTop(take);

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
            _builder.Builder.And();
            _builder.Resolver.QueryByIsIn(expression, sqlQuery);

            return this;
        }

        public IReadOnlyRepository<TEntity, TIdentity> WhereIsIn(
            Expression<Func<TEntity, object>> expression, IEnumerable<object> values)
        {
            expression.ThrowIfNull(() => expression);
            _builder.Builder.And();
            _builder.Resolver.QueryByIsIn(expression, values);

            return this;
        }

        public IReadOnlyRepository<TEntity, TIdentity> WhereNotIn(
            Expression<Func<TEntity, object>> expression, ISqlQuery sqlQuery)
        {
            expression.ThrowIfNull(() => expression);
            _builder.Builder.And();
            _builder.Resolver.QueryByNotIn(expression, sqlQuery);

            return this;
        }

        public IReadOnlyRepository<TEntity, TIdentity> WhereNotIn(
            Expression<Func<TEntity, object>> expression,
            IEnumerable<object> values)
        {
            expression.ThrowIfNull(() => expression);
            _builder.Builder.And();
            _builder.Resolver.QueryByNotIn(expression, values);

            return this;
        }

        public IReadOnlyRepository<TEntity, TIdentity> WherePrimaryKey(
            Expression<Func<TEntity, bool>> expression)
        {
            expression.ThrowIfNull(() => expression);
            var instance = _typeAccessor.CreateInstance<TEntity>();

            _builder.Builder.And();
            _builder.Resolver.QueryByPrimaryKey(instance.IdAccessor.Name, expression);

            return this;
        }

        internal IReadOnlyRepository<TEntity, TIdentity> And(
            Expression<Func<TEntity, bool>> expression)
        {
            expression.ThrowIfNull(() => expression);
            _builder.Builder.And();
            _builder.Resolver.ResolveQuery(expression);

            return this;
        }

        internal IReadOnlyRepository<TEntity, TIdentity> Or(
            Expression<Func<TEntity, bool>> expression)
        {
            expression.ThrowIfNull(() => expression);

            _builder.Builder.Or();
            _builder.Resolver.ResolveQuery(expression);

            return this;
        }

        protected override void DisposeManagedResources()
        {
            _database.Dispose();
        }
    }
}