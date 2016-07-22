using Skeleton.Core.Repository;
using Skeleton.Infrastructure.Data;
using Skeleton.Infrastructure.Repository.SqlBuilder;
using Skeleton.Shared.Abstraction;
using Skeleton.Shared.Abstraction.Reflection;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Skeleton.Infrastructure.Repository
{
    public class EntityReader<TEntity, TIdentity> :
        DisposableBase,
        IEntityReader<TEntity, TIdentity>
        where TEntity : class, IEntity<TEntity, TIdentity>
    {
        private readonly IDatabase _database;
        private readonly SelectQueryBuilder<TEntity, TIdentity> _builder;

        public EntityReader(
            IMetadataProvider metadataProvider,
            IDatabase database)
        {
            _database = database;
            _builder = new SelectQueryBuilder<TEntity, TIdentity>(metadataProvider);
        }

        protected IDatabase Database
        {
            get { return _database; }
        }

        internal SelectQueryBuilder<TEntity, TIdentity> Builder
        {
            get { return _builder; }
        }

        public virtual IEnumerable<TEntity> Find()
        {
            return Builder.OnNextQuery(() =>
                Database.Find<TEntity>(
                          Builder.SqlQuery,
                          Builder.Parameters));
        }

        public virtual TEntity FirstOrDefault()
        {
            return Builder.OnNextQuery(() =>
                Database.FirstOrDefault<TEntity>(
                        Builder.SqlQuery,
                        Builder.Parameters));
        }

        public virtual TEntity FirstOrDefault(TIdentity id)
        {
            id.ThrowIfNull(() => id);

            Builder.QueryByPrimaryKey(e => e.Id.Equals(id));

            return FirstOrDefault();
        }

        public virtual IEnumerable<TEntity> GetAll()
        {
            return Builder.OnNextQuery(() =>
                Database.Find<TEntity>(
                        Builder.SqlQuery,
                        Builder.Parameters));
        }

        public virtual IEnumerable<TEntity> Page(int pageSize, int pageNumber)
        {
            return Builder.OnNextQuery(() =>
                 Database.Find<TEntity>(
                    Builder.SqlPagedQuery(pageSize, pageNumber),
                    Builder.Parameters));
        }

        public IEntityReader<TEntity, TIdentity> GroupBy(
           Expression<Func<TEntity, object>> expression)
        {
            expression.ThrowIfNull(() => expression);
            Builder.GroupBy(expression);

            return this;
        }

        public IEntityReader<TEntity, TIdentity> LeftJoin<TEntity2>(
            Expression<Func<TEntity, TEntity2, bool>> expression)
            where TEntity2 : class, IEntity<TEntity2, TIdentity>
        {
            expression.ThrowIfNull(() => expression);
            Builder.Join(expression, JoinType.Left);

            return this;
        }

        public IEntityReader<TEntity, TIdentity> RightJoin<TEntity2>(
            Expression<Func<TEntity, TEntity2, bool>> expression)
            where TEntity2 : class, IEntity<TEntity2, TIdentity>
        {
            expression.ThrowIfNull(() => expression);
            Builder.Join(expression, JoinType.Right);

            return this;
        }

        public IEntityReader<TEntity, TIdentity> InnerJoin<TEntity2>(
            Expression<Func<TEntity, TEntity2, bool>> expression)
            where TEntity2 : class, IEntity<TEntity2, TIdentity>
        {
            expression.ThrowIfNull(() => expression);
            Builder.Join(expression, JoinType.Inner);

            return this;
        }

        public IEntityReader<TEntity, TIdentity> CrossJoin<TEntity2>(
            Expression<Func<TEntity, TEntity2, bool>> expression)
            where TEntity2 : class, IEntity<TEntity2, TIdentity>
        {
            expression.ThrowIfNull(() => expression);
            Builder.Join(expression, JoinType.Cross);

            return this;
        }

        public IEntityReader<TEntity, TIdentity> OrderBy(
            Expression<Func<TEntity, object>> expression)
        {
            expression.ThrowIfNull(() => expression);
            Builder.OrderBy(expression);

            return this;
        }

        public IEntityReader<TEntity, TIdentity> OrderByDescending(
            Expression<Func<TEntity, object>> expression)
        {
            expression.ThrowIfNull(() => expression);
            Builder.OrderByDescending(expression);

            return this;
        }

        public IEntityReader<TEntity, TIdentity> Select(
            params Expression<Func<TEntity, object>>[] expressions)
        {
            expressions.ThrowIfNull(() => expressions);

            foreach (var expression in expressions)
                Builder.Select(expression);

            return this;
        }

        public IEntityReader<TEntity, TIdentity> Distinct(
            Expression<Func<TEntity, object>> expression)
        {
            expression.ThrowIfNull(() => expression);
            Builder.Aggregate(expression, SelectFunction.Distinct);

            return this;
        }

        public IEntityReader<TEntity, TIdentity> Top(int take)
        {
            Builder.Top(take);

            return this;
        }

        public IEntityReader<TEntity, TIdentity> Where(
            Expression<Func<TEntity, bool>> expression)
        {
            expression.ThrowIfNull(() => expression);
            return And(expression);
        }

        public IEntityReader<TEntity, TIdentity> WhereIsIn(
            Expression<Func<TEntity, object>> expression,
            IEnumerable<object> values)
        {
            expression.ThrowIfNull(() => expression);
            Builder.WhereIsIn(expression, values);

            return this;
        }

        public IEntityReader<TEntity, TIdentity> WhereNotIn(
            Expression<Func<TEntity, object>> expression,
            IEnumerable<object> values)
        {
            expression.ThrowIfNull(() => expression);
            Builder.WhereNotIn(expression, values);

            return this;
        }

        private IEntityReader<TEntity, TIdentity> And(
            Expression<Func<TEntity, bool>> expression)
        {
            expression.ThrowIfNull(() => expression);
            Builder.ResolveQuery(expression);

            return this;
        }

        public TResult Average<TResult>(Expression<Func<TEntity, TResult>> expression)
        {
            expression.ThrowIfNull(() => expression);
            Builder.Aggregate(expression, SelectFunction.Avg);

            return AggregateAs<TResult>();
        }

        public int Count()
        {
            Builder.Count();

            return AggregateAs<int>();
        }

        public TResult Count<TResult>(Expression<Func<TEntity, TResult>> expression)
        {
            expression.ThrowIfNull(() => expression);
            Builder.Aggregate(expression, SelectFunction.Count);

            return AggregateAs<TResult>();
        }

        public TResult Max<TResult>(Expression<Func<TEntity, TResult>> expression)
        {
            expression.ThrowIfNull(() => expression);
            Builder.Aggregate(expression, SelectFunction.Max);

            return AggregateAs<TResult>();
        }

        public TResult Min<TResult>(Expression<Func<TEntity, TResult>> expression)
        {
            expression.ThrowIfNull(() => expression);
            Builder.Aggregate(expression, SelectFunction.Min);

            return AggregateAs<TResult>();
        }

        public TResult Sum<TResult>(Expression<Func<TEntity, TResult>> expression)
        {
            expression.ThrowIfNull(() => expression);
            Builder.Aggregate(expression, SelectFunction.Sum);

            return AggregateAs<TResult>();
        }

        private TResult AggregateAs<TResult>()
        {
            return Builder.OnNextQuery(() =>
                 Database.ExecuteScalar<TResult>(
                         Builder.SqlQuery, 
                         Builder.Parameters));
        }

        protected override void DisposeManagedResources()
        {
            _database.Dispose();
        }
    }
}