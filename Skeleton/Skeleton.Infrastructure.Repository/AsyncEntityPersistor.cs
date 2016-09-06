using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Skeleton.Abstraction;
using Skeleton.Abstraction.Data;
using Skeleton.Abstraction.Repository;
using Skeleton.Common;
using Skeleton.Infrastructure.Repository.SqlBuilder;

namespace Skeleton.Infrastructure.Repository
{
    public class AsyncEntityPersistor<TEntity, TIdentity> :
            DisposableBase,
            IAsyncEntityPersistor<TEntity, TIdentity>
        where TEntity : class, IEntity<TEntity, TIdentity>
    {
        private readonly IMetadataProvider _metadataProvider;

        public AsyncEntityPersistor(
            IMetadataProvider metadataProvider,
            IDatabaseAsync database)
        {
            Database = database;
            _metadataProvider = metadataProvider;
        }

        protected IDatabaseAsync Database { get; }

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
                    result = await SaveAsync(entity);

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
            var builder = new InsertCommandBuilder<TEntity, TIdentity>(
                _metadataProvider, entity);

            var id = await Database.ExecuteScalarAsync<TIdentity>(
                    builder.SqlQuery,
                    builder.Parameters)
                .ConfigureAwait(false);

            if (id != null)
                entity.IdAccessor.SetValue(entity, id);

            return id;
        }

        private async Task<int> DeleteCommand(TEntity entity)
        {
            var builder = new DeleteCommandBuilder<TEntity, TIdentity>(
                _metadataProvider, entity);

            return await Database.ExecuteAsync(
                    builder.SqlQuery,
                    builder.Parameters)
                .ConfigureAwait(false);
        }

        private async Task<int> UpdateCommand(TEntity entity)
        {
            var builder = new UpdateCommandBuilder<TEntity, TIdentity>(
                _metadataProvider, entity);

            return await Database.ExecuteAsync(
                    builder.SqlQuery,
                    builder.Parameters)
                .ConfigureAwait(false);
        }

        protected override void DisposeManagedResources()
        {
            Database.Dispose();
        }
    }
}