using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Skeleton.Core.Repository;
using Skeleton.Infrastructure.Data;
using Skeleton.Infrastructure.Repository.SqlBuilder;
using Skeleton.Shared.Abstraction;
using Skeleton.Shared.Abstraction.Reflection;

namespace Skeleton.Infrastructure.Repository
{
    public class CrudRepositoryAsync<TEntity, TIdentity> :
        ReadRepositoryAsync<TEntity, TIdentity>,
        ICrudRepositoryAsync<TEntity, TIdentity>
        where TEntity : class, IEntity<TEntity, TIdentity>
    {
        public CrudRepositoryAsync(
            IMetadataProvider metadataProvider,
            IDatabaseAsync database)
            : base(metadataProvider, database)
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
            try
            {
                Builder.SetInsertColumns<TEntity, TIdentity>(entity);

                var id = await Database.ExecuteScalarAsync<TIdentity>(
                    Builder.InsertQuery,
                    Builder.Parameters)
                    .ConfigureAwait(false);

                if (id != null)
                    entity.IdAccessor.SetValue(entity, id);

                return id;
            }
            finally
            {
                InitializeSqlBuilder();
            }
        }

        private async Task<int> DeleteCommand(TEntity entity)
        {
            try
            {
                Builder.QueryByPrimaryKey<TEntity>(
                    EntityIdName,
                    e => e.Id.Equals(entity.Id));

                return await Database.ExecuteAsync(
                    Builder.DeleteQuery,
                    Builder.Parameters)
                    .ConfigureAwait(false);
            }
            finally
            {
                InitializeSqlBuilder();
            }
        }

        private async Task<int> UpdateCommand(TEntity entity)
        {
            try
            {
                Builder.SetUpdateColumns<TEntity, TIdentity>(entity);

                Builder.QueryByPrimaryKey<TEntity>(
                    EntityIdName,
                    e => e.Id.Equals(entity.Id));

                return await Database.ExecuteAsync(
                    Builder.UpdateQuery,
                    Builder.Parameters)
                    .ConfigureAwait(false);
            }
            finally
            {
                InitializeSqlBuilder();
            }
        }
    }
}