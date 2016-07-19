using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Skeleton.Core.Repository;
using Skeleton.Infrastructure.Data;
using Skeleton.Infrastructure.Repository.SqlBuilder;
using Skeleton.Shared.Abstraction;
using Skeleton.Shared.Abstraction.Reflection;

namespace Skeleton.Infrastructure.Repository
{
    public class EntityReader<TEntity, TIdentity> :
        DisposableBase,
        IEntityReader<TEntity, TIdentity>
        where TEntity : class, IEntity<TEntity, TIdentity>
    {
        private readonly IDatabase _database;
        private readonly QueryBuilder<TEntity, TIdentity> _builder;

        public EntityReader(
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

        internal QueryBuilder<TEntity, TIdentity> Builder
        {
            get { return _builder; }
        }

        public virtual IEnumerable<TEntity> Find()
        {
            return _builder.Initialize(() =>
                Database.Find<TEntity>(
                    _builder.SelectQuery,
                    _builder.Parameters));
        }

        public virtual TEntity FirstOrDefault()
        {
            return _builder.Initialize(() =>
                Database.FirstOrDefault<TEntity>(
                    _builder.SelectQuery,
                    _builder.Parameters));
        }

        public virtual TEntity FirstOrDefault(TIdentity id)
        {
            id.ThrowIfNull(() => id);

            _builder.QueryByPrimaryKey(
                e => e.Id.Equals(id));

            return FirstOrDefault();
        }

        public virtual IEnumerable<TEntity> GetAll()
        {
            return _builder.Initialize(() =>
                Database.Find<TEntity>(
                    _builder.SelectQuery,
                    _builder.Parameters));
        }

        public virtual IEnumerable<TEntity> Page(int pageSize, int pageNumber)
        {
            return _builder.Initialize(() =>
                Database.Find<TEntity>(
                    _builder.PagedQuery(pageSize, pageNumber),
                    _builder.Parameters));
        }

        public IEntityReader<TEntity, TIdentity> GroupBy(
           Expression<Func<TEntity, object>> expression)
        {
            expression.ThrowIfNull(() => expression);
            _builder.GroupBy(expression);

            return this;
        }

        public IEntityReader<TEntity, TIdentity> LeftJoin<TEntity2>(
            Expression<Func<TEntity, TEntity2, bool>> expression)
            where TEntity2 : class, IEntity<TEntity2, TIdentity>
        {
            expression.ThrowIfNull(() => expression);
            _builder.Join(expression, JoinType.Left);

            return this;
        }

        public IEntityReader<TEntity, TIdentity> RightJoin<TEntity2>(
            Expression<Func<TEntity, TEntity2, bool>> expression)
            where TEntity2 : class, IEntity<TEntity2, TIdentity>
        {
            expression.ThrowIfNull(() => expression);
            _builder.Join(expression, JoinType.Right);

            return this;
        }

        public IEntityReader<TEntity, TIdentity> InnerJoin<TEntity2>(
            Expression<Func<TEntity, TEntity2, bool>> expression)
            where TEntity2 : class, IEntity<TEntity2, TIdentity>
        {
            expression.ThrowIfNull(() => expression);
            _builder.Join(expression, JoinType.Inner);

            return this;
        }

        public IEntityReader<TEntity, TIdentity> CrossJoin<TEntity2>(
            Expression<Func<TEntity, TEntity2, bool>> expression)
            where TEntity2 : class, IEntity<TEntity2, TIdentity>
        {
            expression.ThrowIfNull(() => expression);
            _builder.Join(expression, JoinType.Cross);

            return this;
        }

        public IEntityReader<TEntity, TIdentity> OrderBy(
            Expression<Func<TEntity, object>> expression)
        {
            expression.ThrowIfNull(() => expression);
            _builder.OrderBy(expression);

            return this;
        }

        public IEntityReader<TEntity, TIdentity> OrderByDescending(
            Expression<Func<TEntity, object>> expression)
        {
            expression.ThrowIfNull(() => expression);
            _builder.OrderByDescending(expression);

            return this;
        }

        public IEntityReader<TEntity, TIdentity> Select(
            params Expression<Func<TEntity, object>>[] expressions)
        {
            expressions.ThrowIfNull(() => expressions);

            foreach (var expression in expressions)
                _builder.Select(expression);

            return this;
        }

        public IEntityReader<TEntity, TIdentity> SelectDistinct(
            Expression<Func<TEntity, object>> expression)
        {
            expression.ThrowIfNull(() => expression);
            _builder.SelectWithFunction(expression, SelectFunction.Distinct);

            return this;
        }

        public IEntityReader<TEntity, TIdentity> SelectTop(int take)
        {
            _builder.SelectTop(take);

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
            _builder.QueryByIsIn(expression, values);

            return this;
        }

        public IEntityReader<TEntity, TIdentity> WhereNotIn(
            Expression<Func<TEntity, object>> expression,
            IEnumerable<object> values)
        {
            expression.ThrowIfNull(() => expression);
            _builder.QueryByNotIn(expression, values);

            return this;
        }

        private IEntityReader<TEntity, TIdentity> And(
            Expression<Func<TEntity, bool>> expression)
        {
            expression.ThrowIfNull(() => expression);
            _builder.ResolveQuery(expression);

            return this;
        }

        public TResult Average<TResult>(Expression<Func<TEntity, TResult>> expression)
        {
            expression.ThrowIfNull(() => expression);
            _builder.SelectWithFunction(expression, SelectFunction.Avg);

            return AggregateAs<TResult>();
        }

        public int Count()
        {
            _builder.SelectCount();

            return AggregateAs<int>();
        }

        public TResult Count<TResult>(Expression<Func<TEntity, TResult>> expression)
        {
            expression.ThrowIfNull(() => expression);
            _builder.SelectWithFunction(expression, SelectFunction.Count);

            return AggregateAs<TResult>();
        }

        public TResult Max<TResult>(Expression<Func<TEntity, TResult>> expression)
        {
            expression.ThrowIfNull(() => expression);
            _builder.SelectWithFunction(expression, SelectFunction.Max);

            return AggregateAs<TResult>();
        }

        public TResult Min<TResult>(Expression<Func<TEntity, TResult>> expression)
        {
            expression.ThrowIfNull(() => expression);
            _builder.SelectWithFunction(expression, SelectFunction.Min);

            return AggregateAs<TResult>();
        }

        public TResult Sum<TResult>(Expression<Func<TEntity, TResult>> expression)
        {
            expression.ThrowIfNull(() => expression);
            _builder.SelectWithFunction(expression, SelectFunction.Sum);

            return AggregateAs<TResult>();
        }

        private TResult AggregateAs<TResult>()
        {
            return _builder.Initialize(() =>
                Database.ExecuteScalar<TResult>(
                    _builder.SelectQuery,
                    _builder.Parameters));
        }

        protected override void DisposeManagedResources()
        {
            _database.Dispose();
        }
    }
}