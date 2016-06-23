using Skeleton.Abstraction;
using Skeleton.Abstraction.Reflection;
using Skeleton.Core.Repository;
using Skeleton.Infrastructure.Data;
using Skeleton.Infrastructure.Repository.SqlBuilder;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Skeleton.Infrastructure.Repository
{
    public class Repository<TEntity, TIdentity> :
        ReadOnlyRepository<TEntity, TIdentity>,
        IRepository<TEntity, TIdentity>
        where TEntity : class, IEntity<TEntity, TIdentity>
    {
        public Repository(
            IMetadataProvider metadataProvider, 
            IDatabase database) 
            : base(metadataProvider, database)
        {
        }

        public ISqlExecute SqlExecute
        {
            get { return Builder; }
        }

        public virtual bool Add(TEntity entity)
        {
            entity.ThrowIfNull(() => entity);

            return AddCommand(entity) != null;
        }

        public virtual bool Add(IEnumerable<TEntity> entities)
        {
            var enumerable = entities as IList<TEntity> ?? entities.ToList();
            enumerable.ThrowIfNullOrEmpty(() => enumerable);
            var count = 0;

            using (var transaction = Database.Transaction)
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
            var enumerable = entities as IList<TEntity> ?? entities.ToList();
            enumerable.ThrowIfNullOrEmpty(() => enumerable);
            int count = 0, result = 0;

            using (var transaction = Database.Transaction)
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
            return entity.Id.IsZeroOrEmpty()
                ? Add(entity)
                : Update(entity);
        }

        public virtual bool Save(IEnumerable<TEntity> entities)
        {
            var enumerable = entities as IList<TEntity> ?? entities.ToList();
            enumerable.ThrowIfNullOrEmpty(() => enumerable);
            var result = false;

            using (var transaction = Database.Transaction)
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
            var enumerable = entities as IList<TEntity> ?? entities.ToList();
            enumerable.ThrowIfNullOrEmpty(() => enumerable);
            int count = 0, result = 0;

            using (var transaction = Database.Transaction)
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
            return HandleSqlBuilderInitialization(() =>
            {
                Builder.SetInsertColumns<TEntity, TIdentity>(entity);

                var id = Database.ExecuteScalar<TIdentity>(
                    Builder.InsertQuery,
                    Builder.Parameters);

                if (id != null)
                    entity.IdAccessor.SetValue(entity, id);

                return id;
            });
        }

        private int DeleteCommand(TEntity entity)
        {
            return HandleSqlBuilderInitialization(() =>
            {
                Builder.QueryByPrimaryKey<TEntity>(
                    EntityIdName,
                    e => e.Id.Equals(entity.Id));

                return Database.Execute(
                    Builder.DeleteQuery,
                    Builder.Parameters);
            });
        }

        private int UpdateCommand(TEntity entity)
        {
            return HandleSqlBuilderInitialization(() =>
            {
                Builder.SetUpdateColumns<TEntity, TIdentity>(entity);
                Builder.QueryByPrimaryKey<TEntity>(
                    EntityIdName,
                    e => e.Id.Equals(entity.Id));

                entity.LastModifiedDateTime = DateTime.Now;

                return Database.Execute(
                    Builder.UpdateQuery,
                    Builder.Parameters);
            });
        }

        //    Builder.And();
        //    Resolver.ResolveQuery(expression);

        //    return this;
        //}

        //public IExecuteBuilder<TEntity, TIdentity> WhereIsIn(
        //    Expression<Func<TEntity, object>> expression,
        //    IEnumerable<object> values)
        //{
        //    Builder.And();
        //    Resolver.QueryByIsIn(expression, values);

        //    return this;
        //public IExecuteBuilder<TEntity, TIdentity> Where(Expression<Func<TEntity, bool>> expression)

        //}
        //{
        //public IExecuteBuilder<TEntity, TIdentity> WhereNotIn(
        //    Expression<Func<TEntity, object>> expression,
        //    IEnumerable<object> values)
        //{
        //    Builder.And();
        //    Resolver.QueryByNotIn(expression, values);

        //    return this;
        //}

        //public IExecuteBuilder<TEntity, TIdentity> WherePrimaryKey(
        //    Expression<Func<TEntity, bool>> whereExpression)
        //{
        //    Builder.And();
        //    Resolver.QueryByPrimaryKey(_entity.IdAccessor.Name, whereExpression);

        //    return this;
        //}
    }
}