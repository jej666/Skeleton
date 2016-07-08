﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Skeleton.Core.Repository;
using Skeleton.Infrastructure.Data;
using Skeleton.Infrastructure.Repository.SqlBuilder;
using Skeleton.Shared.Abstraction;
using Skeleton.Shared.Abstraction.Reflection;

namespace Skeleton.Infrastructure.Repository
{
    public class ReadRepository<TEntity, TIdentity> :
        EntityRepository<TEntity, TIdentity>,
        IReadRepository<TEntity, TIdentity>
        where TEntity : class, IEntity<TEntity, TIdentity>
    {
        private readonly IDatabase _database;

        public ReadRepository(
            IMetadataProvider metadataProvider,
            IDatabase database)
            : base(metadataProvider)
        {
            database.ThrowIfNull(() => database);

            _database = database;
        }

        protected IDatabase Database
        {
            get { return _database; }
        }

        public ISqlQuery SqlQuery
        {
            get { return Builder; }
        }

        public virtual IEnumerable<TEntity> Find()
        {
            return HandleSqlBuilderInitialization(() =>
                Database.Find<TEntity>(
                    Builder.Query,
                    Builder.Parameters));
        }

        public virtual TEntity FirstOrDefault()
        {
            return HandleSqlBuilderInitialization(() =>
                Database.FirstOrDefault<TEntity>(
                    Builder.Query,
                    Builder.Parameters));
        }

        public virtual TEntity FirstOrDefault(TIdentity id)
        {
            id.ThrowIfNull(() => id);

            Builder.And();
            Builder.QueryByPrimaryKey<TEntity>(
                EntityIdName,
                e => e.Id.Equals(id));

            return FirstOrDefault();
        }

        public virtual IEnumerable<TEntity> GetAll()
        {
            return HandleSqlBuilderInitialization(() =>
                Database.Find<TEntity>(
                    Builder.Query,
                    Builder.Parameters));
        }

        public virtual IEnumerable<TEntity> Page(int pageSize, int pageNumber)
        {
            Builder.OrderBy<TEntity>(EntityIdName);

            return HandleSqlBuilderInitialization(() =>
                Database.Find<TEntity>(
                    Builder.PagedQuery(pageSize, pageNumber),
                    Builder.Parameters));
        }

        public IReadRepository<TEntity, TIdentity> GroupBy(
            Expression<Func<TEntity, object>> expression)
        {
            expression.ThrowIfNull(() => expression);
            Builder.GroupBy(expression);

            return this;
        }

        public IReadRepository<TEntity, TIdentity> LeftJoin<TEntity2>(
            Expression<Func<TEntity, TEntity2, bool>> expression)
            where TEntity2 : class, IEntity<TEntity2, TIdentity>
        {
            expression.ThrowIfNull(() => expression);
            Builder.Join(expression, JoinType.Left);

            return this;
        }

        public IReadRepository<TEntity, TIdentity> RightJoin<TEntity2>(
            Expression<Func<TEntity, TEntity2, bool>> expression)
            where TEntity2 : class, IEntity<TEntity2, TIdentity>
        {
            expression.ThrowIfNull(() => expression);
            Builder.Join(expression, JoinType.Right);

            return this;
        }

        public IReadRepository<TEntity, TIdentity> InnerJoin<TEntity2>(
            Expression<Func<TEntity, TEntity2, bool>> expression)
            where TEntity2 : class, IEntity<TEntity2, TIdentity>
        {
            expression.ThrowIfNull(() => expression);
            Builder.Join(expression, JoinType.Inner);

            return this;
        }

        public IReadRepository<TEntity, TIdentity> CrossJoin<TEntity2>(
            Expression<Func<TEntity, TEntity2, bool>> expression)
            where TEntity2 : class, IEntity<TEntity2, TIdentity>
        {
            expression.ThrowIfNull(() => expression);
            Builder.Join(expression, JoinType.Cross);

            return this;
        }

        public IReadRepository<TEntity, TIdentity> OrderBy(
            Expression<Func<TEntity, object>> expression)
        {
            expression.ThrowIfNull(() => expression);
            Builder.OrderBy(expression);

            return this;
        }

        public IReadRepository<TEntity, TIdentity> OrderByDescending(
            Expression<Func<TEntity, object>> expression)
        {
            expression.ThrowIfNull(() => expression);
            Builder.OrderByDescending(expression);

            return this;
        }

        public IReadRepository<TEntity, TIdentity> Select(
            params Expression<Func<TEntity, object>>[] expressions)
        {
            expressions.ThrowIfNull(() => expressions);

            foreach (var expression in expressions)
                Builder.Select(expression);

            return this;
        }

        public IReadRepository<TEntity, TIdentity> SelectDistinct(
            Expression<Func<TEntity, object>> expression)
        {
            expression.ThrowIfNull(() => expression);
            Builder.SelectWithFunction(expression, SelectFunction.Distinct);

            return this;
        }

        public IReadRepository<TEntity, TIdentity> SelectTop(int take)
        {
            Builder.SelectTop(take);

            return this;
        }

        public IReadRepository<TEntity, TIdentity> Where(
            Expression<Func<TEntity, bool>> expression)
        {
            expression.ThrowIfNull(() => expression);
            return And(expression);
        }

        public IReadRepository<TEntity, TIdentity> WhereIsIn(
            Expression<Func<TEntity, object>> expression,
            ISqlQuery sqlQuery)
        {
            expression.ThrowIfNull(() => expression);
            Builder.And();
            Builder.QueryByIsIn(expression, sqlQuery);

            return this;
        }

        public IReadRepository<TEntity, TIdentity> WhereIsIn(
            Expression<Func<TEntity, object>> expression,
            IEnumerable<object> values)
        {
            expression.ThrowIfNull(() => expression);
            Builder.And();
            Builder.QueryByIsIn(expression, values);

            return this;
        }

        public IReadRepository<TEntity, TIdentity> WhereNotIn(
            Expression<Func<TEntity, object>> expression,
            ISqlQuery sqlQuery)
        {
            expression.ThrowIfNull(() => expression);
            Builder.And();
            Builder.QueryByNotIn(expression, sqlQuery);

            return this;
        }

        public IReadRepository<TEntity, TIdentity> WhereNotIn(
            Expression<Func<TEntity, object>> expression,
            IEnumerable<object> values)
        {
            expression.ThrowIfNull(() => expression);
            Builder.And();
            Builder.QueryByNotIn(expression, values);

            return this;
        }

        public TResult Average<TResult>(Expression<Func<TEntity, TResult>> expression)
        {
            expression.ThrowIfNull(() => expression);
            Builder.SelectWithFunction(expression, SelectFunction.Avg);

            return AggregateAs<TResult>();
        }

        public int Count()
        {
            Builder.SelectCount();

            return AggregateAs<int>();
        }

        public TResult Count<TResult>(Expression<Func<TEntity, TResult>> expression)
        {
            expression.ThrowIfNull(() => expression);
            Builder.SelectWithFunction(expression, SelectFunction.Count);

            return AggregateAs<TResult>();
        }

        public TResult Max<TResult>(Expression<Func<TEntity, TResult>> expression)
        {
            expression.ThrowIfNull(() => expression);
            Builder.SelectWithFunction(expression, SelectFunction.Max);

            return AggregateAs<TResult>();
        }

        public TResult Min<TResult>(Expression<Func<TEntity, TResult>> expression)
        {
            expression.ThrowIfNull(() => expression);
            Builder.SelectWithFunction(expression, SelectFunction.Min);

            return AggregateAs<TResult>();
        }

        public TResult Sum<TResult>(Expression<Func<TEntity, TResult>> expression)
        {
            expression.ThrowIfNull(() => expression);
            Builder.SelectWithFunction(expression, SelectFunction.Sum);

            return AggregateAs<TResult>();
        }

        private TResult AggregateAs<TResult>()
        {
            return HandleSqlBuilderInitialization(() =>
                Database.ExecuteScalar<TResult>(
                    Builder.Query,
                    Builder.Parameters));
        }

        private IReadRepository<TEntity, TIdentity> And(
            Expression<Func<TEntity, bool>> expression)
        {
            expression.ThrowIfNull(() => expression);
            Builder.And();
            Builder.ResolveQuery(expression);

            return this;
        }

        protected override void DisposeManagedResources()
        {
            _database.Dispose();
        }
    }
}