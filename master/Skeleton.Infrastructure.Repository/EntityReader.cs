﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Skeleton.Abstraction;
using Skeleton.Abstraction.Data;
using Skeleton.Abstraction.Repository;
using Skeleton.Common;
using Skeleton.Infrastructure.Repository.SqlBuilder;

namespace Skeleton.Infrastructure.Repository
{
    public class EntityReader<TEntity> :
            DisposableBase,
            IEntityReader<TEntity>
        where TEntity : class, IEntity<TEntity>
    {
        public EntityReader(
            IMetadataProvider metadataProvider,
            IDatabase database)
        {
            Builder = new SelectQueryBuilder<TEntity>(metadataProvider);
            Database = database;
        }

        protected IDatabase Database { get; }

        internal SelectQueryBuilder<TEntity> Builder { get; }

        public virtual IEnumerable<TEntity> Find()
        {
            return Builder.OnNextQuery(() =>
                Database.Find<TEntity>(
                    Builder.SqlQuery,
                    Builder.Parameters));
        }

        public virtual TEntity FirstOrDefault()
        {
            return FirstOrDefaultCore();
        }

        public virtual TEntity FirstOrDefault(object id)
        {
            id.ThrowIfNull(() => id);

            Builder.QueryByPrimaryKey(e => e.Id.Equals(id));

            return FirstOrDefaultCore();
        }

        private TEntity FirstOrDefaultCore()
        {
            return Builder.OnNextQuery(() =>
                Database.FirstOrDefault<TEntity>(
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

        public IEntityReader<TEntity> GroupBy(
            Expression<Func<TEntity, object>> expression)
        {
            expression.ThrowIfNull(() => expression);
            Builder.GroupBy(expression);

            return this;
        }

        public IEntityReader<TEntity> LeftJoin<TEntity2>(
            Expression<Func<TEntity, TEntity2, bool>> expression)
            where TEntity2 : class, IEntity<TEntity2>
        {
            expression.ThrowIfNull(() => expression);
            Builder.Join(expression, JoinType.Left);

            return this;
        }

        public IEntityReader<TEntity> RightJoin<TEntity2>(
            Expression<Func<TEntity, TEntity2, bool>> expression)
            where TEntity2 : class, IEntity<TEntity2>
        {
            expression.ThrowIfNull(() => expression);
            Builder.Join(expression, JoinType.Right);

            return this;
        }

        public IEntityReader<TEntity> InnerJoin<TEntity2>(
            Expression<Func<TEntity, TEntity2, bool>> expression)
            where TEntity2 : class, IEntity<TEntity2>
        {
            expression.ThrowIfNull(() => expression);
            Builder.Join(expression, JoinType.Inner);

            return this;
        }

        public IEntityReader<TEntity> OrderBy(
            Expression<Func<TEntity, object>> expression)
        {
            expression.ThrowIfNull(() => expression);
            Builder.OrderBy(expression);

            return this;
        }

        public IEntityReader<TEntity> OrderByDescending(
            Expression<Func<TEntity, object>> expression)
        {
            expression.ThrowIfNull(() => expression);
            Builder.OrderByDescending(expression);

            return this;
        }

        public IEntityReader<TEntity> Select(
            params Expression<Func<TEntity, object>>[] expressions)
        {
            expressions.ThrowIfNull(() => expressions);

            foreach (var expression in expressions)
                Builder.Select(expression);

            return this;
        }

        public IEntityReader<TEntity> Distinct(
            Expression<Func<TEntity, object>> expression)
        {
            expression.ThrowIfNull(() => expression);
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
            expression.ThrowIfNull(() => expression);
            return And(expression);
        }

        public IEntityReader<TEntity> WhereIsIn(
            Expression<Func<TEntity, object>> expression,
            IEnumerable<object> values)
        {
            expression.ThrowIfNull(() => expression);
            Builder.WhereIsIn(expression, values);

            return this;
        }

        public IEntityReader<TEntity> WhereNotIn(
            Expression<Func<TEntity, object>> expression,
            IEnumerable<object> values)
        {
            expression.ThrowIfNull(() => expression);
            Builder.WhereNotIn(expression, values);

            return this;
        }

        public IEnumerable<dynamic> Average(
            Expression<Func<TEntity, object>> expression)
        {
            expression.ThrowIfNull(() => expression);
            Builder.Aggregate(expression, SelectFunction.Avg);

            return Aggregate();
        }

        public int Count()
        {
            Builder.Count();

            return Builder.OnNextQuery(() =>
                Database.ExecuteScalar<int>(
                    Builder.SqlQuery,
                    Builder.Parameters));
        }

        public IEnumerable<dynamic> Count(
            Expression<Func<TEntity, object>> expression)
        {
            expression.ThrowIfNull(() => expression);
            Builder.Aggregate(expression, SelectFunction.Count);

            return Aggregate();
        }

        public IEnumerable<dynamic> Max(
            Expression<Func<TEntity, object>> expression)
        {
            expression.ThrowIfNull(() => expression);
            Builder.Aggregate(expression, SelectFunction.Max);

            return Aggregate();
        }

        public IEnumerable<dynamic> Min(
            Expression<Func<TEntity, object>> expression)
        {
            expression.ThrowIfNull(() => expression);
            Builder.Aggregate(expression, SelectFunction.Min);

            return Aggregate();
        }

        public IEnumerable<dynamic> Sum(
            Expression<Func<TEntity, object>> expression)
        {
            expression.ThrowIfNull(() => expression);
            Builder.Aggregate(expression, SelectFunction.Sum);

            return Aggregate();
        }

        protected override void DisposeManagedResources()
        {
            Database.Dispose();
        }

        private IEntityReader<TEntity> And(
            Expression<Func<TEntity, bool>> expression)
        {
            expression.ThrowIfNull(() => expression);
            Builder.ResolveQuery(expression);

            return this;
        }

        private IEnumerable<dynamic> Aggregate()
        {
            return Builder.OnNextQuery(() =>
                Database.Find(
                    Builder.SqlQuery,
                    Builder.Parameters));
        }
    }
}