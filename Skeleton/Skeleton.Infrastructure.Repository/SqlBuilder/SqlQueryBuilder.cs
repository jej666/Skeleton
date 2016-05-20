////https://github.com/base33/lambda-sql-builder

//using System;
//using System.Collections.Generic;
//using System.Linq.Expressions;
//using Skeleton.Common.Extensions;
//using Skeleton.Common.Reflection;
//using Skeleton.Core.Domain;
//using Skeleton.Core.Repository;
//using Skeleton.Infrastructure.Data;
//using Skeleton.Infrastructure.Repository.SqlBuilder.ExpressionTree;

//namespace Skeleton.Infrastructure.Repository.SqlBuilder
//{
//    public sealed class SqlQueryBuilder<TEntity, TIdentity> :
//        SqlBuilderBase,
//        IQueryBuilder<TEntity, TIdentity>,
//        IAggregateBuilder<TEntity, TIdentity>,
//        IAggregateExecutor
//        where TEntity : class, IEntity<TEntity, TIdentity>
//    {
//        private readonly ITypeAccessor _accessor;
//        private readonly IDatabase _database;

//        public SqlQueryBuilder(IDatabase database, ITypeAccessorCache accessorCache)
//            : base(TableInfo.GetTableName<TEntity>())
//        {
//            accessorCache.ThrowIfNull(() => accessorCache);

//            _accessor = accessorCache.Get<TEntity>();
//            _database = database;
//        }

//        private SqlQueryBuilder(InternalQueryBuilder builder, LambdaExpressionResolver resolver)
//            : base(builder, resolver)
//        {
//        }

//        public IAggregateExecutor Average(Expression<Func<TEntity, object>> expression)
//        {
//            expression.ThrowIfNull(() => expression);
//            Resolver.SelectWithFunction(expression, SelectFunction.AVG);

//            return this;
//        }

//        public IAggregateExecutor Count(Expression<Func<TEntity, object>> expression)
//        {
//            expression.ThrowIfNull(() => expression);
//            Resolver.SelectWithFunction(expression, SelectFunction.COUNT);

//            return this;
//        }

//        public IAggregateExecutor Max(Expression<Func<TEntity, object>> expression)
//        {
//            expression.ThrowIfNull(() => expression);
//            Resolver.SelectWithFunction(expression, SelectFunction.MAX);

//            return this;
//        }

//        public IAggregateExecutor Min(Expression<Func<TEntity, object>> expression)
//        {
//            expression.ThrowIfNull(() => expression);
//            Resolver.SelectWithFunction(expression, SelectFunction.MIN);

//            return this;
//        }

//        public IAggregateExecutor Sum(Expression<Func<TEntity, object>> expression)
//        {
//            expression.ThrowIfNull(() => expression);
//            Resolver.SelectWithFunction(expression, SelectFunction.SUM);

//            return this;
//        }

//        public TResult As<TResult>()
//        {
//            return _database.ExecuteScalar<TResult>(Builder.Query, Builder.Parameters);
//        }

//        public ISqlQuery AsSql()
//        {
//            return this;
//        }

//        //public IQueryBuilder<TEntity, TIdentity> GroupBy(Expression<Func<TEntity, object>> expression)
//        //{
//        //    expression.ThrowIfNull(() => expression);
//        //    Resolver.GroupBy(expression);

//        //    return this;
//        //}

//        //public IQueryBuilder<TEntity2, TIdentity2> Join<TEntity2, TIdentity2>(
//        //    Expression<Func<TEntity, TEntity2, bool>> expression)
//        //    where TEntity2 : class, IEntity<TEntity2, TIdentity2>
//        //{
//        //    expression.ThrowIfNull(() => expression);

//        //    var joinQuery = new SqlQueryBuilder<TEntity2, TIdentity2>(Builder, Resolver);
//        //    Resolver.Join(expression);

//        //    return joinQuery;
//        //}

//        //public IQueryBuilder<TEntity, TIdentity> OrderBy(
//        //    Expression<Func<TEntity, object>> expression)
//        //{
//        //    expression.ThrowIfNull(() => expression);
//        //    Resolver.OrderBy(expression);

//        //    return this;
//        //}

//        //public IQueryBuilder<TEntity, TIdentity> OrderByDescending(
//        //    Expression<Func<TEntity, object>> expression)
//        //{
//        //    expression.ThrowIfNull(() => expression);
//        //    Resolver.OrderByDescending(expression);

//        //    return this;
//        //}

//        //public IQueryBuilder<TEntity, TIdentity> Select(
//        //    params Expression<Func<TEntity, object>>[] expressions)
//        //{
//        //    expressions.ThrowIfNull(() => expressions);

//        //    foreach (var expression in expressions)
//        //        Resolver.Select(expression);

//        //    return this;
//        //}

//        //public IQueryBuilder<TEntity, TIdentity> SelectDistinct(
//        //    Expression<Func<TEntity, object>> expression)
//        //{
//        //    expression.ThrowIfNull(() => expression);
//        //    Resolver.SelectWithFunction(expression, SelectFunction.DISTINCT);

//        //    return this;
//        //}

//        //public IQueryBuilder<TEntity, TIdentity> SelectTop(int take)
//        //{
//        //    Resolver.SelectTop(take);

//        //    return this;
//        //}

//        //public IQueryBuilder<TEntity, TIdentity> Where(
//        //    Expression<Func<TEntity, bool>> expression)
//        //{
//        //    expression.ThrowIfNull(() => expression);
//        //    return And(expression);
//        //}

//        //public IQueryBuilder<TEntity, TIdentity> WhereIsIn(
//        //    Expression<Func<TEntity, object>> expression, ISqlQuery sqlQuery)
//        //{
//        //    expression.ThrowIfNull(() => expression);
//        //    Builder.And();
//        //    Resolver.QueryByIsIn(expression, sqlQuery);

//        //    return this;
//        //}

//        //public IQueryBuilder<TEntity, TIdentity> WhereIsIn(
//        //    Expression<Func<TEntity, object>> expression, IEnumerable<object> values)
//        //{
//        //    expression.ThrowIfNull(() => expression);
//        //    Builder.And();
//        //    Resolver.QueryByIsIn(expression, values);

//        //    return this;
//        //}

//        //public IQueryBuilder<TEntity, TIdentity> WhereNotIn(
//        //    Expression<Func<TEntity, object>> expression, ISqlQuery sqlQuery)
//        //{
//        //    expression.ThrowIfNull(() => expression);
//        //    Builder.And();
//        //    Resolver.QueryByNotIn(expression, sqlQuery);

//        //    return this;
//        //}

//        //public IQueryBuilder<TEntity, TIdentity> WhereNotIn(
//        //    Expression<Func<TEntity, object>> expression,
//        //    IEnumerable<object> values)
//        //{
//        //    expression.ThrowIfNull(() => expression);
//        //    Builder.And();
//        //    Resolver.QueryByNotIn(expression, values);

//        //    return this;
//        //}

//        //public IQueryBuilder<TEntity, TIdentity> WherePrimaryKey(
//        //    Expression<Func<TEntity, bool>> expression)
//        //{
//        //    expression.ThrowIfNull(() => expression);
//        //    var instance = _accessor.CreateInstance<TEntity>();

//        //    Builder.And();
//        //    Resolver.QueryByPrimaryKey(instance.IdAccessor.Name, expression);

//        //    return this;
//        //}
        
//        //public IQueryBuilder<TEntity, TIdentity> And(
//        //    Expression<Func<TEntity, bool>> expression)
//        //{
//        //    expression.ThrowIfNull(() => expression);
//        //    Builder.And();
//        //    Resolver.ResolveQuery(expression);

//        //    return this;
//        //}

//        //public IQueryBuilder<TEntity, TIdentity> Or(
//        //    Expression<Func<TEntity, bool>> expression)
//        //{
//        //    expression.ThrowIfNull(() => expression);

//        //    Builder.Or();
//        //    Resolver.ResolveQuery(expression);

//        //    return this;
//        //}
//    }
//}