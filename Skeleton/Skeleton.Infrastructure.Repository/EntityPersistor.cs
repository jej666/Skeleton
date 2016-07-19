using System;
using System.Collections.Generic;
using Skeleton.Core.Repository;
using Skeleton.Infrastructure.Data;
using Skeleton.Shared.Abstraction;
using Skeleton.Shared.Abstraction.Reflection;
using Skeleton.Infrastructure.Repository.SqlBuilder;

namespace Skeleton.Infrastructure.Repository
{
    public class EntityPersistor<TEntity, TIdentity> :
        DisposableBase,
        IEntityPersitor<TEntity, TIdentity>
        where TEntity : class, IEntity<TEntity, TIdentity>
    {
        private readonly QueryBuilder<TEntity, TIdentity> _builder;
        private readonly IDatabase _database;

        public EntityPersistor(
            IMetadataProvider metadataProvider,
            IDatabase database)
        {
            _database = database;
            _builder = new QueryBuilder<TEntity, TIdentity>(metadataProvider);
        }

        protected IDatabase Database
        {
            get { return _database; }
        }

        public virtual bool Add(TEntity entity)
        {
            entity.ThrowIfNull(() => entity);

            return AddCommand(entity) != null;
        }

        public virtual bool Add(IEnumerable<TEntity> entities)
        {
            entities.ThrowIfNullOrEmpty(() => entities);

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
            entity.ThrowIfNull(() => entity);

            return DeleteCommand(entity) > 0;
        }

        public virtual bool Delete(IEnumerable<TEntity> entities)
        {
            entities.ThrowIfNullOrEmpty(() => entities);

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
            entity.ThrowIfNull(() => entity);

            return entity.Id.IsZeroOrEmpty()
                ? Add(entity)
                : Update(entity);
        }

        public virtual bool Save(IEnumerable<TEntity> entities)
        {
            entities.ThrowIfNullOrEmpty(() => entities);

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
            entity.ThrowIfNull(() => entity);

            return UpdateCommand(entity) > 0;
        }

        public virtual bool Update(IEnumerable<TEntity> entities)
        {
            entities.ThrowIfNullOrEmpty(() => entities);

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

        private TIdentity AddCommand(TEntity entity)
        {
            return _builder.Initialize(() =>
            {
                _builder.SetInsertColumns(entity);

                var id = _database.ExecuteScalar<TIdentity>(
                    _builder.InsertQuery,
                    _builder.Parameters);

                if (id != null)
                    entity.IdAccessor.SetValue(entity, id);

                return id;
            });
        }

        private int DeleteCommand(TEntity entity)
        {
            return _builder.Initialize(() =>
            {
                _builder.QueryByPrimaryKey(
                    e => e.Id.Equals(entity.Id));

                return _database.Execute(
                    _builder.DeleteQuery,
                    _builder.Parameters);
            });
        }

        private int UpdateCommand(TEntity entity)
        {
            return _builder.Initialize(() =>
            {
                _builder.SetUpdateColumns(entity);
                _builder.QueryByPrimaryKey(
                    e => e.Id.Equals(entity.Id));

                entity.LastModifiedDateTime = DateTime.Now;

                return _database.Execute(
                    _builder.UpdateQuery,
                    _builder.Parameters);
            });
        }

        protected override void DisposeManagedResources()
        {
            _database.Dispose();
        }
    }
}