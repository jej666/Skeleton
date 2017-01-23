﻿using Skeleton.Abstraction.Data;
using Skeleton.Abstraction.Domain;
using Skeleton.Abstraction.Reflection;
using Skeleton.Abstraction.Repository;
using Skeleton.Common;
using Skeleton.Infrastructure.Repository.SqlBuilder;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Skeleton.Infrastructure.Repository
{
    public class AsyncEntityPersistor<TEntity> :
            DisposableBase,
            IAsyncEntityPersistor<TEntity>
        where TEntity : class, IEntity<TEntity>
    {
        private readonly IMetadataProvider _metadataProvider;
        private readonly IDatabaseAsync _database;

        public AsyncEntityPersistor(
            IMetadataProvider metadataProvider,
            IDatabaseAsync database)
        {
            _metadataProvider = metadataProvider;
            _database = database;
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

            using (var transaction = _database.Transaction)
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

            using (var transaction = _database.Transaction)
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

            using (var transaction = _database.Transaction)
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

            using (var transaction = _database.Transaction)
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

        protected override void DisposeManagedResources()
        {
            _database.Dispose();
            base.DisposeManagedResources();
        }

        private async Task<object> AddCommand(TEntity entity)
        {
            var builder = new InsertCommandBuilder<TEntity>(
                _metadataProvider, entity);

            var id = await _database.ExecuteScalarAsync(
                    builder.SqlCommand)
                .ConfigureAwait(false);

            if (id != null)
            {
                var destinationType = entity.IdAccessor.MemberType;
                var convertedId = id.ChangeType(destinationType, CultureInfo.CurrentCulture);
                entity.IdAccessor.Setter(entity, convertedId);
            }

            return id;
        }

        private async Task<int> DeleteCommand(TEntity entity)
        {
            var builder = new DeleteCommandBuilder<TEntity>(
                _metadataProvider, entity);

            return await _database.ExecuteAsync(
                    builder.SqlCommand)
                .ConfigureAwait(false);
        }

        private async Task<int> UpdateCommand(TEntity entity)
        {
            var builder = new UpdateCommandBuilder<TEntity>(
                _metadataProvider, entity);

            return await _database.ExecuteAsync(
                    builder.SqlCommand)
                .ConfigureAwait(false);
        }
    }
}