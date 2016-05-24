using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Skeleton.Common.Extensions;
using Skeleton.Common.Reflection;
using Skeleton.Core.Domain;
using Skeleton.Core.Repository;
using Skeleton.Infrastructure.Data;
using Skeleton.Infrastructure.Data.Configuration;
using Skeleton.Infrastructure.Repository.SqlBuilder;

namespace Skeleton.Infrastructure.Repository
{
    public abstract class RepositoryAsyncBase<TEntity, TIdentity> :
        ReadOnlyRepositoryAsyncBase<TEntity, TIdentity>,
        IRepositoryAsync<TEntity, TIdentity>
        where TEntity : class, IEntity<TEntity, TIdentity>
    {
        protected RepositoryAsyncBase(
            ITypeAccessorCache typeAccessorCache,
            IDatabaseAsync database) :
                base(typeAccessorCache, database)
        {
        }

        protected RepositoryAsyncBase(
            ITypeAccessorCache typeAccessorCache,
            IDatabaseFactory databaseFactory,
            Func<IDatabaseConfigurationBuilder, IDatabaseConfiguration> configurator) :
                this(typeAccessorCache, databaseFactory.CreateDatabaseForAsyncOperations(configurator))
        {
        }

        public ISqlExecute SqlExecute
        {
            get { return Builder; }
        }

        public virtual async Task<bool> AddAsync(TEntity entity)
        {
            entity.ThrowIfNull(() => entity);

            return await AddCommand(entity) != null;
        }

        public virtual async Task<bool> AddAsync(IEnumerable<TEntity> entities)
        {
            var enumerable = entities as IList<TEntity> ?? entities.ToList();
            enumerable.ThrowIfNullOrEmpty(() => enumerable);
            var count = 0;

            using (var transaction = Database.Transaction)
            {
                transaction.Begin();

                foreach (var entity in enumerable)
                {
                    await AddCommand(entity);
                    ++count;
                }

                if (count > 0)
                    transaction.Commit();
            }
            return count > 0;
        }

        public virtual async Task<bool> DeleteAsync(TEntity entity)
        {
            entity.ThrowIfNull(() => entity);

            return await DeleteCommand(entity) > 0;
        }

        public virtual async Task<bool> DeleteAsync(IEnumerable<TEntity> entities)
        {
            var enumerable = entities as IList<TEntity> ?? entities.ToList();
            enumerable.ThrowIfNullOrEmpty(() => enumerable);
            int count = 0, result = 0;

            using (var transaction = Database.Transaction)
            {
                transaction.Begin();

                foreach (var entity in enumerable)
                {
                    result += await DeleteCommand(entity);
                    ++count;
                }

                if (result == count)
                    transaction.Commit();
            }
            return result == count;
        }

        public virtual async Task<bool> SaveAsync(TEntity entity)
        {
            if (entity.Id.IsZeroOrEmpty())
                return await AddAsync(entity);

            return await UpdateAsync(entity);
        }

        public virtual async Task<bool> SaveAsync(IEnumerable<TEntity> entities)
        {
            var enumerable = entities as IList<TEntity> ?? entities.ToList();
            enumerable.ThrowIfNullOrEmpty(() => enumerable);
            var result = false;

            using (var transaction = Database.Transaction)
            {
                transaction.Begin();

                foreach (var entity in enumerable)
                {
                    result = await SaveAsync(entity);
                }

                if (result)
                    transaction.Commit();
            }
            return result;
        }

        public virtual async Task<bool> UpdateAsync(TEntity entity)
        {
            entity.ThrowIfNull(() => entity);

            return await UpdateCommand(entity) > 0;
        }

        public virtual async Task<bool> UpdateAsync(IEnumerable<TEntity> entities)
        {
            var enumerable = entities as IList<TEntity> ?? entities.ToList();
            enumerable.ThrowIfNullOrEmpty(() => enumerable);
            int count = 0, result = 0;

            using (var transaction = Database.Transaction)
            {
                transaction.Begin();

                foreach (var entity in enumerable)
                {
                    result += await UpdateCommand(entity);
                    ++count;
                }

                if (result == count)
                    transaction.Commit();
            }
            return result == count;
        }

        private async Task<TIdentity> AddCommand(TEntity entity)
        {
            var columns = TypeAccessor.GetTableColumns();
            Builder.SetInsertColumns<TEntity, TIdentity>(columns, entity);

            var id = await Database.ExecuteScalarAsync<TIdentity>(
                Builder.InsertQuery,
                Builder.Parameters)
                .ConfigureAwait(false);

            if (id != null)
                entity.IdAccessor.SetValue(entity, id);

            return id;
        }

        private async Task<int> DeleteCommand(TEntity entity)
        {
            Builder.QueryByPrimaryKey<TEntity>(
                entity.IdAccessor.Name,
                e => e.Id.Equals(entity.Id));

            return await Database.ExecuteAsync(
                Builder.DeleteQuery,
                Builder.Parameters)
                .ConfigureAwait(false);
        }

        private async Task<int> UpdateCommand(TEntity entity)
        {
            var columns = TypeAccessor.GetTableColumns();
            Builder.SetUpdateColumns<TEntity, TIdentity>(columns, entity);

            Builder.QueryByPrimaryKey<TEntity>(
                entity.IdAccessor.Name,
                e => e.Id.Equals(entity.Id));

            return await Database.ExecuteAsync(
                Builder.UpdateQuery,
                Builder.Parameters)
                .ConfigureAwait(false);
        }
    }
}