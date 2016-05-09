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
    using System.Threading.Tasks;

    public abstract class ReadOnlyRepositoryAsyncBase<TEntity, TIdentity> :
        DisposableBase,
        IReadOnlyRepositoryAsync<TEntity, TIdentity>
        where TEntity : class, IEntity<TEntity, TIdentity>
    {
        private readonly IDatabaseAsync _database;
        private readonly ISqlQueryBuilder<TEntity, TIdentity> _queryBuilder;
        private readonly ITypeAccessorCache _typeAccessorCache;

        protected ReadOnlyRepositoryAsyncBase(
            ITypeAccessorCache typeAccessorCache,
            IDatabaseAsync database)
        {
            typeAccessorCache.ThrowIfNull(() => typeAccessorCache);
            database.ThrowIfNull(() => database);

            _typeAccessorCache = typeAccessorCache;
            _database = database;
            _queryBuilder = new SqlQueryBuilder<TEntity, TIdentity>(typeAccessorCache);
        }

        protected ReadOnlyRepositoryAsyncBase(
            ITypeAccessorCache typeAccessorCache,
            IDatabaseFactory databaseFactory,
            Func<IDatabaseConfigurationBuilder, IDatabaseConfiguration> configurator) :
            this(typeAccessorCache, 
                 databaseFactory.CreateDatabaseForAsyncOperations(configurator))
        { }

        public ISqlQueryBuilder<TEntity, TIdentity> QueryBuilder
        {
            get { return _queryBuilder; }
        }

        protected IDatabaseAsync Database
        {
            get { return _database; }
        }

        protected ITypeAccessorCache TypeAccessorCache
        {
            get { return _typeAccessorCache; }
        }

        public async virtual Task<IEnumerable<TEntity>> FindAsync(
            Expression<Func<TEntity, bool>> where,
            Expression<Func<TEntity, object>> orderBy)
        {
            where.ThrowIfNull(() => where);
            orderBy.ThrowIfNull(() => orderBy);

            var sql = QueryBuilder.Where(where)
                                   .OrderBy(orderBy)
                                   .AsSql();

            return await Database.FindAsync<TEntity>(
                    sql.Query, 
                    sql.Parameters)
                .ConfigureAwait(false);
        }

        public async virtual Task<IEnumerable<TEntity>> FindAsync(ISqlQuery query)
        {
            query.ThrowIfNull(() => query);

            return await Database.FindAsync<TEntity>(
                    query.Query, 
                    query.Parameters)
                .ConfigureAwait(false);
        }

        public async virtual Task<TEntity> FirstOrDefaultAsync(TIdentity id)
        {
            id.ThrowIfNull(() => id);

            var sql = QueryBuilder.WherePrimaryKey(e => e.Id.Equals(id))
                                   .AsSql();

            return await Database.FirstOrDefaultAsync<TEntity>(
                    sql.Query, 
                    sql.Parameters)
                .ConfigureAwait(false);
        }

        public async virtual Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> where)
        {
            where.ThrowIfNull(() => where);

            var sql = QueryBuilder.Where(where)
                                  .AsSql();

            return await Database.FirstOrDefaultAsync<TEntity>(
                    sql.Query, 
                    sql.Parameters)
                .ConfigureAwait(false);
        }

        public async virtual Task<IEnumerable<TEntity>> GetAllAsync()
        {
            var sql = QueryBuilder.AsSql();

            return await Database.FindAsync<TEntity>(
                    sql.Query, 
                    sql.Parameters)
                .ConfigureAwait(false);
        }

        public async virtual Task<IEnumerable<TEntity>> PageAsync(
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

            return await Database.FindAsync<TEntity>(
                    sql.PagedQuery(pageSize, pageNumber),
                    sql.Parameters)
                .ConfigureAwait(false);
        }

        public async virtual Task<IEnumerable<TEntity>> PageAllAsync(int pageSize, int pageNumber)
        {
            var sql = QueryBuilder.AsSql();

            return await Database.FindAsync<TEntity>(
                    sql.PagedQuery(pageSize, pageNumber),
                    sql.Parameters)
                .ConfigureAwait(false);
        }

        protected override void DisposeManagedResources()
        {
            _database.Dispose();
        }
    }
}