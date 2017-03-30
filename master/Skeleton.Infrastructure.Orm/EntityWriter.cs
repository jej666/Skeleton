using Skeleton.Abstraction.Data;
using Skeleton.Abstraction.Domain;
using Skeleton.Abstraction.Reflection;
using Skeleton.Abstraction.Orm;
using Skeleton.Common;
using Skeleton.Infrastructure.Orm.SqlBuilder;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace Skeleton.Infrastructure.Orm
{
    public class EntityWriter<TEntity> :
            DisposableBase,
            IEntityWriter<TEntity>
        where TEntity : class, IEntity<TEntity>
    {
        private readonly IMetadataProvider _metadataProvider;
        private readonly IDatabase _database;
        private const string Error = "Entity Id is not null. {0} is canceled";

        public EntityWriter(
            IMetadataProvider metadataProvider,
            IDatabase database)
        {
            _metadataProvider = metadataProvider;
            _database = database;
        }

        public virtual bool Add(TEntity entity)
        {
            entity.ThrowIfNull(nameof(entity));

            return AddCommand(entity) != null;
        }

        public virtual bool Add(IEnumerable<TEntity> entities)
        {
            entities.ThrowIfNullOrEmpty(nameof(entities));

            var enumerable = entities.AsList();
            var count = 0;

            using (var transaction = _database.Transaction)
            {
                transaction.Begin();

                enumerable.ForEach(entity =>
                {
                    AddCommand(entity);
                    ++count;
                });

                if (count > 0)
                    transaction.Commit();
            }
            return count > 0;
        }

        public virtual bool Delete(TEntity entity)
        {
            entity.ThrowIfNull(nameof(entity));

            return DeleteCommand(entity) > 0;
        }

        public virtual bool Delete(IEnumerable<TEntity> entities)
        {
            entities.ThrowIfNullOrEmpty(nameof(entities));

            var enumerable = entities.AsList();
            int count = 0, result = 0;

            using (var transaction = _database.Transaction)
            {
                transaction.Begin();

                enumerable.ForEach(entity =>
                {
                    result += DeleteCommand(entity);
                    ++count;
                });

                if (result == count)
                    transaction.Commit();
            }
            return result == count;
        }

        public virtual bool Save(TEntity entity)
        {
            entity.ThrowIfNull(nameof(entity));

            return entity.Id.IsZeroOrEmpty()
                ? Add(entity)
                : Update(entity);
        }

        public virtual bool Save(IEnumerable<TEntity> entities)
        {
            entities.ThrowIfNullOrEmpty(nameof(entities));

            var enumerable = entities.AsList();
            var result = false;

            using (var transaction = _database.Transaction)
            {
                transaction.Begin();

                enumerable.ForEach(entity => { result = Save(entity); });

                if (result)
                    transaction.Commit();
            }
            return result;
        }

        public virtual bool Update(TEntity entity)
        {
            entity.ThrowIfNull(nameof(entity));

            return UpdateCommand(entity) > 0;
        }

        public virtual bool Update(IEnumerable<TEntity> entities)
        {
            entities.ThrowIfNullOrEmpty(nameof(entities));

            var enumerable = entities.AsList();
            int count = 0, result = 0;

            using (var transaction = _database.Transaction)
            {
                transaction.Begin();

                enumerable.ForEach(entity =>
                {
                    result += UpdateCommand(entity);
                    ++count;
                });

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

        private object AddCommand(TEntity entity)
        {
            if (entity.Id.IsNotZeroOrEmpty())
                throw new ArgumentException(Error.FormatWith(nameof(AddCommand)));

            var builder = new InsertCommandBuilder<TEntity>(
                _metadataProvider, entity);

            var id = _database.ExecuteScalar(builder.SqlCommand);

            if (id != null)
            {
                var destinationType = entity.IdAccessor.MemberType;
                var convertedId = id.ChangeType(destinationType, CultureInfo.CurrentCulture);
                entity.IdAccessor.Setter(entity, convertedId);
            }

            return id;
        }

        private int DeleteCommand(TEntity entity)
        {
            EnsureEntityIdExists(entity);

            var builder = new DeleteCommandBuilder<TEntity>(
                _metadataProvider, entity);

            return _database.Execute(builder.SqlCommand);
        }

        private int UpdateCommand(TEntity entity)
        {
            EnsureEntityIdExists(entity);

            var builder = new UpdateCommandBuilder<TEntity>(
                _metadataProvider, entity);

            entity.LastModifiedDateTime = DateTime.Now;

            return _database.Execute(builder.SqlCommand);
        }

        private static void EnsureEntityIdExists(TEntity entity, [CallerMemberName] string name = "")
        {
            if (entity.Id.IsZeroOrEmpty())
                throw new ArgumentException(Error.FormatWith(name));
        }
    }
}