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
        private readonly SqlQueryBuilder<TEntity, TIdentity> _queryBuilder;
        private readonly ITypeAccessorCache _typeAccessorCache;

        protected ReadOnlyRepositoryBase(
            ITypeAccessorCache typeAccessorCache,
            IDatabase database)
        {
            typeAccessorCache.ThrowIfNull(() => typeAccessorCache);
            database.ThrowIfNull(() => database);

            _typeAccessorCache = typeAccessorCache;
            _database = database;
            _queryBuilder = new SqlQueryBuilder<TEntity, TIdentity>(database, typeAccessorCache);
        }

        protected ReadOnlyRepositoryBase(
            ITypeAccessorCache typeAccessorCache,
            IDatabaseFactory databaseFactory,
            Func<IDatabaseConfigurationBuilder, IDatabaseConfiguration> configurator) :
            this(typeAccessorCache, databaseFactory.CreateDatabase(configurator))
        { }

        public IQueryBuilder<TEntity, TIdentity> Query
        {
            get { return _queryBuilder; }
        }

        public IAggregateBuilder<TEntity, TIdentity> Aggregate
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

            var sql = Query.Where(where)
                                  .OrderBy(orderBy)
                                  .AsSql();

            return Database.Find<TEntity>(sql.Query, sql.Parameters);
        }

        public virtual TEntity FirstOrDefault(TIdentity id)
        {
            id.ThrowIfNull(() => id);

            return Query.WherePrimaryKey(e => e.Id.Equals(id))
                        .FirstOrDefault();
        }

        public virtual TEntity FirstOrDefault(Expression<Func<TEntity, bool>> where)
        {
            where.ThrowIfNull(() => where);

            var sql = Query.Where(where)
                                  .AsSql();

            return Database.FirstOrDefault<TEntity>(sql.Query, sql.Parameters);
        }

        public virtual IEnumerable<TEntity> GetAll()
        {
            var sql = Query.AsSql();

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

            var sql = Query.Where(where)
                           .OrderBy(orderBy)
                           .AsSql();

            return Database.Find<TEntity>(
                sql.PagedQuery(pageSize, pageNumber),
                sql.Parameters);
        }

        public virtual IEnumerable<TEntity> PageAll(int pageSize, int pageNumber)
        {
            var sql = Query.AsSql();

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