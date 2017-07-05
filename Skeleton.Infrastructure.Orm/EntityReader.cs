using Skeleton.Abstraction;
using Skeleton.Abstraction.Data;
using Skeleton.Abstraction.Domain;
using Skeleton.Abstraction.Orm;
using Skeleton.Abstraction.Reflection;
using Skeleton.Common;
using Skeleton.Infrastructure.Orm.SqlBuilder;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Skeleton.Infrastructure.Orm
{
    public class EntityReader<TEntity> :
            DisposableBase,
            IEntityReader<TEntity>
        where TEntity : class, IEntity<TEntity>
    {
        private readonly IDatabase _database;

        public EntityReader(
            IMetadataProvider metadataProvider,
            IDatabase database)
        {
            _database = database;
            Builder = new SelectQueryBuilder<TEntity>(metadataProvider);
        }

        internal SelectQueryBuilder<TEntity> Builder { get; }

        public virtual IEnumerable<TEntity> Find()
        {
            return Builder.OnNextQuery(() =>
                _database.Find<TEntity>(
                    Builder.SqlCommand));
        }

        public virtual TEntity FirstOrDefault()
        {
            return FirstOrDefaultCore();
        }

        public virtual TEntity FirstOrDefault(object id)
        {
            id.ThrowIfNull(nameof(id));

            Builder.QueryByPrimaryKey(e => e.Id.Equals(id));

            return FirstOrDefaultCore();
        }

        public virtual IEnumerable<TEntity> Query(IQuery query)
        {
            query.ThrowIfNull(nameof(query));

            var evaluator = new QueryEvaluator<TEntity>(Builder, query);
            evaluator.Evaluate();

            return Builder.OnNextQuery(
                    () => _database.Find<TEntity>(Builder.SqlCommand));
        }

        public IEntityReader<TEntity> GroupBy(
            Expression<Func<TEntity, object>> expression)
        {
            expression.ThrowIfNull(nameof(expression));
            Builder.GroupBy(expression);

            return this;
        }

        public IEntityReader<TEntity> LeftJoin<TEntity2>(
            Expression<Func<TEntity, TEntity2, bool>> expression)
            where TEntity2 : class, IEntity<TEntity2>
        {
            expression.ThrowIfNull(nameof(expression));
            Builder.Join(expression, JoinType.Left);

            return this;
        }

        public IEntityReader<TEntity> RightJoin<TEntity2>(
            Expression<Func<TEntity, TEntity2, bool>> expression)
            where TEntity2 : class, IEntity<TEntity2>
        {
            expression.ThrowIfNull(nameof(expression));
            Builder.Join(expression, JoinType.Right);

            return this;
        }

        public IEntityReader<TEntity> InnerJoin<TEntity2>(
            Expression<Func<TEntity, TEntity2, bool>> expression)
            where TEntity2 : class, IEntity<TEntity2>
        {
            expression.ThrowIfNull(nameof(expression));
            Builder.Join(expression, JoinType.Inner);

            return this;
        }

        public IEntityReader<TEntity> OrderBy(
            Expression<Func<TEntity, object>> expression)
        {
            expression.ThrowIfNull(nameof(expression));
            Builder.OrderBy(expression);

            return this;
        }

        public IEntityReader<TEntity> OrderByDescending(
            Expression<Func<TEntity, object>> expression)
        {
            expression.ThrowIfNull(nameof(expression));
            Builder.OrderByDescending(expression);

            return this;
        }

        public IEntityReader<TEntity> Select(
            params Expression<Func<TEntity, object>>[] expressions)
        {
            expressions.ThrowIfNull(nameof(expressions));

            foreach (var expression in expressions)
                Builder.Select(expression);

            return this;
        }

        public IEntityReader<TEntity> Distinct(
            Expression<Func<TEntity, object>> expression)
        {
            expression.ThrowIfNull(nameof(expression));
            Builder.Aggregate(expression, SelectFunction.Distinct);

            return this;
        }

        public IEntityReader<TEntity> Top(int take)
        {
            Builder.Top(take);

            return this;
        }

        public IEntityReader<TEntity> Where(
            Expression<Func<TEntity, bool>> expression)
        {
            expression.ThrowIfNull(nameof(expression));
            return And(expression);
        }

        public IEntityReader<TEntity> WhereIsIn(
            Expression<Func<TEntity, object>> expression,
            IEnumerable<object> values)
        {
            expression.ThrowIfNull(nameof(expression));
            Builder.WhereIsIn(expression, values);

            return this;
        }

        public IEntityReader<TEntity> WhereNotIn(
            Expression<Func<TEntity, object>> expression,
            IEnumerable<object> values)
        {
            expression.ThrowIfNull(nameof(expression));
            Builder.WhereNotIn(expression, values);

            return this;
        }

        public IEnumerable<dynamic> Average(
            Expression<Func<TEntity, object>> expression)
        {
            expression.ThrowIfNull(nameof(expression));
            Builder.Aggregate(expression, SelectFunction.Avg);

            return Aggregate();
        }

        public int Count()
        {
            Builder.Count();

            return Builder.OnNextQuery(() =>
                _database.ExecuteScalar<int>(
                    Builder.SqlCommand));
        }

        public IEnumerable<dynamic> Count(
            Expression<Func<TEntity, object>> expression)
        {
            expression.ThrowIfNull(nameof(expression));
            Builder.Aggregate(expression, SelectFunction.Count);

            return Aggregate();
        }

        public IEnumerable<dynamic> Max(
            Expression<Func<TEntity, object>> expression)
        {
            expression.ThrowIfNull(nameof(expression));
            Builder.Aggregate(expression, SelectFunction.Max);

            return Aggregate();
        }

        public IEnumerable<dynamic> Min(
            Expression<Func<TEntity, object>> expression)
        {
            expression.ThrowIfNull(nameof(expression));
            Builder.Aggregate(expression, SelectFunction.Min);

            return Aggregate();
        }

        public IEnumerable<dynamic> Sum(
            Expression<Func<TEntity, object>> expression)
        {
            expression.ThrowIfNull(nameof(expression));
            Builder.Aggregate(expression, SelectFunction.Sum);

            return Aggregate();
        }

        private TEntity FirstOrDefaultCore()
        {
            return Builder.OnNextQuery(() =>
                _database.FirstOrDefault<TEntity>(
                    Builder.SqlCommand));
        }

        protected override void DisposeManagedResources()
        {
            _database.Dispose();
            base.DisposeManagedResources();
        }

        private IEntityReader<TEntity> And(
            Expression<Func<TEntity, bool>> expression)
        {
            expression.ThrowIfNull(nameof(expression));
            Builder.ResolveQuery(expression);

            return this;
        }

        private IEnumerable<dynamic> Aggregate()
        {
            return Builder.OnNextQuery(() =>
                _database.Find(
                    Builder.SqlCommand));
        }
    }
}