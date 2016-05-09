//namespace Skeleton.Infrastructure.Repository
//{
//    using Common.Extensions;
//    using Common.Reflection;
//    using Core.Domain;
//    using Core.Repository;
//    using Data;
//    using Data.Configuration;
//    using SqlBuilder;
//    using System;
//    using System.Collections.Generic;
//    using System.Diagnostics.CodeAnalysis;
//    using System.Threading.Tasks;

//    public abstract class RepositoryAsyncBase<TEntity, TIdentity> :
//        ReadOnlyRepositoryAsyncBase<TEntity, TIdentity>,
//        IRepositoryAsync<TEntity, TIdentity>
//        where TEntity : class, IEntity<TEntity, TIdentity>
//    {
//        protected RepositoryAsyncBase(
//            ITypeAccessorCache typeAccessorCache,
//            IDatabaseAsync database) :
//            base(typeAccessorCache, database)
//        { }

//        protected RepositoryAsyncBase(
//            ITypeAccessorCache typeAccessorCache,
//            IDatabaseFactory databaseFactory,
//            Func<IDatabaseConfigurationBuilder, IDatabaseConfiguration> configurator) :
//            this(typeAccessorCache, databaseFactory.CreateDatabaseForAsyncOperations(configurator))
//        { }

//        private ISqlExecuteBuilder<TEntity, TIdentity> ExecuteBuilder
//        {
//            get
//            {
//                return new SqlExecuteBuilder<TEntity, TIdentity>(TypeAccessorCache);
//            }
//        }

//        public async virtual Task<bool> AddAsync(TEntity entity)
//        {
//            entity.ThrowIfNull(() => entity);

//            return await AddCommand(entity) != null;
//        }

//        public async virtual Task<bool> AddAsync(IEnumerable<TEntity> entities)
//        {
//            entities.ThrowIfNullOrEmpty(() => entities);
//            int count = 0;

//            using (var transaction = Database.Transaction)
//            {
//                transaction.Begin();

//                foreach (var entity in entities)
//                {
//                    await AddCommand(entity);
//                    ++count;
//                }

//                if (count > 0)
//                    transaction.Commit();
//            }
//            return count > 0;
//        }

//        public async virtual Task<bool> DeleteAsync(TEntity entity)
//        {
//            entity.ThrowIfNull(() => entity);

//            return await DeleteCommand(entity) > 0;
//        }

//        public async virtual Task<bool> DeleteAsync(IEnumerable<TEntity> entities)
//        {
//            entities.ThrowIfNullOrEmpty(() => entities);
//            int count = 0, result = 0;

//            using (var transaction = Database.Transaction)
//            {
//                transaction.Begin();

//                foreach (var entity in entities)
//                {
//                    result += await DeleteCommand(entity);
//                    ++count;
//                }

//                if (result == count)
//                    transaction.Commit();
//            }
//            return result == count;
//        }

//        public async virtual Task<bool> SaveAsync(TEntity entity)
//        {
//            if (entity.Id.IsZeroOrEmpty())
//                return await AddAsync(entity);
//            else
//                return await UpdateAsync(entity);
//        }

//        public async virtual Task<bool> SaveAsync(IEnumerable<TEntity> entities)
//        {
//            entities.ThrowIfNullOrEmpty(() => entities);
//            bool result = false;

//            using (var transaction = Database.Transaction)
//            {
//                transaction.Begin();

//                foreach (var entity in entities)
//                {
//                    result = await SaveAsync(entity);
//                }

//                if (result)
//                    transaction.Commit();
//            }
//            return result;
//        }

//        public async virtual Task<bool> UpdateAsync(TEntity entity)
//        {
//            entity.ThrowIfNull(() => entity);

//            return await UpdateCommand(entity) > 0;
//        }

//        public async virtual Task<bool> UpdateAsync(IEnumerable<TEntity> entities)
//        {
//            entities.ThrowIfNullOrEmpty(() => entities);
//            int count = 0, result = 0;

//            using (var transaction = Database.Transaction)
//            {
//                transaction.Begin();

//                foreach (var entity in entities)
//                {
//                    result += await UpdateCommand(entity);
//                    ++count;
//                }

//                if (result == count)
//                    transaction.Commit();
//            }
//            return result == count;
//        }

//        private async Task<TIdentity> AddCommand(TEntity entity)
//        {
//            var sql = ExecuteBuilder.Insert(entity)
//                                    .AsSql();

//            var id = await Database.ExecuteScalarAsync<TIdentity>(
//                                        sql.InsertQuery, sql.Parameters)
//                                   .ConfigureAwait(false);

//            if (id != null)
//                entity.IdAccessor.SetValue(entity, id);

//            return id;
//        }

//        private async Task<int> DeleteCommand(TEntity entity)
//        {
//            var sql = ExecuteBuilder.Delete(entity)
//                                    .WherePrimaryKey(e => e.Id.Equals(entity.Id))
//                                    .AsSql();

//            return await Database.ExecuteAsync(sql.DeleteQuery, sql.Parameters)
//                                 .ConfigureAwait(false);
//        }

//        private async Task<int> UpdateCommand(TEntity entity)
//        {
//            var sql = ExecuteBuilder.Update(entity)
//                                    .WherePrimaryKey(e => e.Id.Equals(entity.Id))
//                                    .AsSql();

//            return await Database.ExecuteAsync(sql.UpdateQuery, sql.Parameters)
//                                 .ConfigureAwait(false);
//        }
//    }
//}