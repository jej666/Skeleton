namespace Skeleton.Infrastructure.Repository
{
    using Common.Extensions;
    using Common.Reflection;
    using Core.Domain;
    using Core.Repository;
    using Data;
    using Data.Configuration;
    using SqlBuilder;
    using System;
    using System.Collections.Generic;

    public abstract class RepositoryBase<TEntity, TIdentity> :
        ReadOnlyRepositoryBase<TEntity, TIdentity>,
        IRepository<TEntity, TIdentity>
        where TEntity : class, IEntity<TEntity, TIdentity>
    {
        protected RepositoryBase(
            ITypeAccessorCache typeAccessorCache,
            IDatabase database) :
            base(typeAccessorCache, database)
        { }

        protected RepositoryBase(
            ITypeAccessorCache typeAccessorCache,
            IDatabaseFactory databaseFactory,
            Func<IDatabaseConfigurationBuilder, IDatabaseConfiguration> configurator) :
            this(typeAccessorCache, databaseFactory.CreateDatabase(configurator))
        { }

        private IExecuteBuilder<TEntity, TIdentity> ExecuteBuilder
        {
            get
            {
                return new SqlExecuteBuilder<TEntity, TIdentity>(TypeAccessorCache);
            }
        }

        public virtual bool Add(TEntity entity)
        {
            entity.ThrowIfNull(() => entity);

            return AddCommand(entity) != null;
        }

        public virtual bool Add(IEnumerable<TEntity> entities)
        {
            entities.ThrowIfNullOrEmpty(() => entities);
            int count = 0;

            using (var transaction = Database.Transaction)
            {
                transaction.Begin();

                entities.ForEach(entity =>
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
            int count = 0, result = 0;

            using (var transaction = Database.Transaction)
            {
                transaction.Begin();

                entities.ForEach(entity =>
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
            if (entity.Id.IsZeroOrEmpty())
                return Add(entity);
            else
                return Update(entity);
        }

        public virtual bool Save(IEnumerable<TEntity> entities)
        {
            entities.ThrowIfNullOrEmpty(() => entities);
            bool result = false;

            using (var transaction = Database.Transaction)
            {
                transaction.Begin();

                entities.ForEach(entity =>
                {
                    result = Save(entity);
                });

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
            int count = 0, result = 0;

            using (var transaction = Database.Transaction)
            {
                transaction.Begin();

                entities.ForEach(entity =>
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
            var sql = ExecuteBuilder.Insert(entity)
                                     .AsSql();

            var id = Database.ExecuteScalar<TIdentity>(
                        sql.InsertQuery,
                        sql.Parameters);

            if (id != null)
                entity.IdAccessor.SetValue(entity, id);

            return id;
        }

        private int DeleteCommand(TEntity entity)
        {
            var sql = ExecuteBuilder.Delete(entity)
                                     .WherePrimaryKey(e => e.Id.Equals(entity.Id))
                                     .AsSql();

            return Database.Execute(sql.DeleteQuery, sql.Parameters);
        }

        private int UpdateCommand(TEntity entity)
        {
            var sql = ExecuteBuilder.Update(entity)
                                    .WherePrimaryKey(e => e.Id.Equals(entity.Id))
                                    .AsSql();

            return Database.Execute(sql.UpdateQuery, sql.Parameters);
        }
    }
}