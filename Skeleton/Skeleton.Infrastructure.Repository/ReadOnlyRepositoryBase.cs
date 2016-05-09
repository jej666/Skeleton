namespace Skeleton.Infrastructure.Repository
{
    using Common;
    using Common.Extensions;
    using Common.Reflection;
    using Core.Domain;
    using Core.Repository;
    using Data;
    using Data.Configuration;
    using SqlBuilder;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq.Expressions;

    public abstract class ReadOnlyRepositoryBase<TEntity, TIdentity> :
        DisposableBase,
        IReadOnlyRepository<TEntity, TIdentity>
        where TEntity : class, IEntity<TEntity, TIdentity>
    {
        private readonly IDatabase _database;
        private readonly ISqlQueryBuilder<TEntity, TIdentity> _queryBuilder;
        private readonly ITypeAccessorCache _typeAccessorCache;

        protected ReadOnlyRepositoryBase(
            ITypeAccessorCache typeAccessorCache,
            IDatabase database)
        {
            typeAccessorCache.ThrowIfNull(() => typeAccessorCache);
            database.ThrowIfNull(() => database);

            _typeAccessorCache = typeAccessorCache;
            _database = database;
            _queryBuilder = new SqlQueryBuilder<TEntity, TIdentity>(typeAccessorCache);
        }

        protected ReadOnlyRepositoryBase(
            ITypeAccessorCache typeAccessorCache,
            IDatabaseFactory databaseFactory,
            Func<IDatabaseConfigurationBuilder, IDatabaseConfiguration> configurator) :
            this(typeAccessorCache, databaseFactory.CreateDatabase(configurator))
        { }

        public ISqlQueryBuilder<TEntity, TIdentity> QueryBuilder
        {
            get { return _queryBuilder; }
        }

        protected IDatabase Database
        {
            get { return _database; }
        }

        protected ITypeAccessorCache TypeAccessorCache
        {
            get { return _typeAccessorCache; }
        }

        public virtual IEnumerable<TEntity> Find(
            Expression<Func<TEntity, bool>> where,
            Expression<Func<TEntity, object>> orderBy)
        {
            where.ThrowIfNull(() => where);
            orderBy.ThrowIfNull(() => orderBy);

            var sql = QueryBuilder.Where(where)
                                  .OrderBy(orderBy)
                                  .AsSql();

            return Database.Find<TEntity>(sql.Query, sql.Parameters);
        }

        public virtual IEnumerable<TEntity> Find(ISqlQuery query)
        {
            query.ThrowIfNull(() => query);

            return Database.Find<TEntity>(query.Query, query.Parameters);
        }

        public virtual TEntity FirstOrDefault(TIdentity id)
        {
            id.ThrowIfNull(() => id);

            var sql = QueryBuilder.WherePrimaryKey(e => e.Id.Equals(id))
                                  .AsSql();

            return Database.FirstOrDefault<TEntity>(sql.Query, sql.Parameters);
        }

        public virtual TEntity FirstOrDefault(Expression<Func<TEntity, bool>> where)
        {
            where.ThrowIfNull(() => where);

            var sql = QueryBuilder.Where(where)
                                  .AsSql();

            return Database.FirstOrDefault<TEntity>(sql.Query, sql.Parameters);
        }

        public virtual IEnumerable<TEntity> GetAll()
        {
            var sql = QueryBuilder.AsSql();

            return Database.Find<TEntity>(sql.Query, sql.Parameters);
        }

        public virtual IEnumerable<TEntity> Page(
            int pageSize,
            int pageNumber,
            Expression<Func<TEntity, bool>> where,
            Expression<Func<TEntity, object>> orderBy)
        {
            where.ThrowIfNull(() => where);
            orderBy.ThrowIfNull(() => orderBy);

            var sql = QueryBuilder.Where(where)
                                  .OrderBy(orderBy)
                                  .AsSql();

            return Database.Find<TEntity>(
                sql.PagedQuery(pageSize, pageNumber),
                sql.Parameters);
        }

        public virtual IEnumerable<TEntity> PageAll(int pageSize, int pageNumber)
        {
            var sql = QueryBuilder.AsSql();

            return Database.Find<TEntity>(
                sql.PagedQuery(pageSize, pageNumber),
                sql.Parameters);
        }

        protected override void DisposeManagedResources()
        {
            _database.Dispose();
        }
    }
}