using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using Skeleton.Infrastructure.Repository.SqlBuilder;
using Skeleton.Shared.Abstraction;
using Skeleton.Shared.Abstraction.Reflection;

namespace Skeleton.Infrastructure.Repository
{
    [DebuggerDisplay("EntityName = {EntityType.Name}")]
    internal class EntitySqlBuilder<TEntity, TIdentity>
        where TEntity : class, IEntity<TEntity, TIdentity>
    {
        private readonly IMetadata _metadata;
        private string _cacheIdName;

        internal EntitySqlBuilder(IMetadataProvider metadataProvider)
        {
            metadataProvider.ThrowIfNull(() => metadataProvider);

            _metadata = metadataProvider.GetMetadata<TEntity>();
            Initialize();
        }

        internal SqlQueryBuilder Builder { get; private set; }

        internal Type EntityType
        {
            get { return _metadata.Type; }
        }

        internal string EntityIdName
        {
            get
            {
                if (!_cacheIdName.IsNullOrEmpty())
                    return _cacheIdName;

                var instance = _metadata.CreateInstance<TEntity>();

                if (instance == null)
                    return string.Empty;

                _cacheIdName = instance.IdAccessor.Name;

                return _cacheIdName;
            }
        }

        internal string DeleteQuery
        {
            get { return Builder.DeleteQuery; }
        }

        internal string InsertQuery
        {
            get { return Builder.InsertQuery; }
        }

        internal string UpdateQuery
        {
            get { return Builder.UpdateQuery; }
        }

        internal IDictionary<string, object> Parameters
        {
            get { return Builder.Parameters; }
        }

        internal string Query
        {
            get { return Builder.Query; }
        }

        internal string PagedQuery(int pageSize, int pageNumber)
        {
            return Builder.PagedQuery(pageSize, pageNumber);
        }

        internal void And()
        {
            Builder.And();
        }

        internal void Initialize()
        {
            Builder = new SqlQueryBuilder(EntityType.Name);
        }

        internal T Initialize<T>(Func<T> func)
        {
            try
            {
                return func();
            }
            finally
            {
                Initialize();
            }
        }

        internal void GroupBy<T>(Expression<Func<T, object>> expression)
        {
            GroupBy<T>(expression.Body.GetMemberExpression());
        }

        internal void OrderBy<T>(string idColumnName)
        {
            Builder.OrderBy(TableInfo.GetTableName<T>(), idColumnName);
        }

        internal void OrderBy<T>(Expression<Func<T, object>> expression)
        {
            var fieldName = TableInfo.GetColumnName(expression.Body.GetMemberExpression());
            Builder.OrderBy(TableInfo.GetTableName<T>(), fieldName);
        }

        internal void OrderByDescending<T>(Expression<Func<T, object>> expression)
        {
            var fieldName = TableInfo.GetColumnName(expression.Body.GetMemberExpression());
            Builder.OrderByDescending(TableInfo.GetTableName<T>(), fieldName);
        }

        internal void OrderByDescending<T>(string idColumnName)
        {
            Builder.OrderByDescending(TableInfo.GetTableName<T>(), idColumnName);
        }

        internal void QueryByIsIn<T>(Expression<Func<T, object>> expression, IEnumerable<object> values)
        {
            var fieldName = TableInfo.GetColumnName(expression);
            Builder.QueryByIsIn(TableInfo.GetTableName<T>(), fieldName, values);
        }

        internal void QueryByNotIn<T>(Expression<Func<T, object>> expression, IEnumerable<object> values)
        {
            var fieldName = TableInfo.GetColumnName(expression);
            Builder.Not();
            Builder.QueryByIsIn(TableInfo.GetTableName<T>(), fieldName, values);
        }

        internal void QueryByPrimaryKey<T>(Expression<Func<T, bool>> whereExpression)
        {
            var expressionTree = ExpressionResolver.Resolve((dynamic)whereExpression.Body, EntityIdName);
            Builder.BuildSql(expressionTree);
        }

        internal void ResolveQuery<T>(Expression<Func<T, bool>> expression)
        {
            var expressionTree = ExpressionResolver.Resolve((dynamic) expression.Body);
            Builder.BuildSql(expressionTree);
        }

        internal void Select<T>(Expression<Func<T, object>> expression)
        {
            Select<T>(expression.Body);
        }

        internal void SelectTop(int take)
        {
            Builder.SelectTop(take);
        }

        internal void SelectCount()
        {
            Builder.SelectCount();
        }

        internal void SelectWithFunction<T, TResult>(
            Expression<Func<T, TResult>> expression,
            SelectFunction selectFunction)
        {
            SelectWithFunction<T>(expression.Body, selectFunction);
        }

        private void GroupBy<T>(Expression expression)
        {
            var fieldName = TableInfo.GetColumnName(expression.GetMemberExpression());
            Builder.GroupBy(TableInfo.GetTableName<T>(), fieldName);
        }

        private void Select<T>(Expression expression)
        {
            switch (expression.NodeType)
            {
                case ExpressionType.Parameter:
                    Builder.Select(TableInfo.GetTableName(expression.Type));
                    break;

                case ExpressionType.Convert:
                case ExpressionType.MemberAccess:
                    Select<T>(expression.GetMemberExpression());
                    break;

                case ExpressionType.New:
                    var newExpression = expression as NewExpression;
                    if (newExpression != null)
                        foreach (var expr in newExpression.Arguments)
                        {
                            var memberExp = (MemberExpression) expr;
                            Select<T>(memberExp);
                        }
                    break;

                default:
                    throw new ArgumentException("Invalid expression");
            }
        }

        private void Select<T>(MemberExpression expression)
        {
            if (expression.Type.IsClass && expression.Type != typeof(string))
                Builder.Select(TableInfo.GetTableName(expression.Type));
            else
                Builder.Select(TableInfo.GetTableName<T>(), TableInfo.GetColumnName(expression));
        }

        private void SelectWithFunction<T>(Expression expression, SelectFunction selectFunction)
        {
            var fieldName = TableInfo.GetColumnName(expression.GetMemberExpression());
            Builder.Select(TableInfo.GetTableName<T>(), fieldName, selectFunction);
        }

        internal void Join<T1, T2>(Expression<Func<T1, T2, bool>> expression, JoinType joinType)
        {
            var joinExpression = expression.Body.GetBinaryExpression();
            var leftExpression = joinExpression.Left.GetMemberExpression();
            var rightExpression = joinExpression.Right.GetMemberExpression();

            Join<T1, T2>(leftExpression, rightExpression, joinType);
        }

        private void Join<T1, T2>(Expression leftExpression, Expression rightExpression, JoinType joinType)
        {
            Builder.Join(
                TableInfo.GetTableName<T1>(),
                TableInfo.GetTableName<T2>(),
                TableInfo.GetColumnName(leftExpression),
                TableInfo.GetColumnName(rightExpression),
                joinType);
        }

        private static IEnumerable<IMemberAccessor> GetTableColumns(TEntity entity)    
        {
            return entity.TypeAccessor.GetDeclaredOnlyProperties()
                .Where(x => x.MemberType.IsPrimitiveExtended())
                .ToArray();
        }

        internal void SetInsertColumns(TEntity entity)
        {
            foreach (var column in GetTableColumns(entity))
            {
                if (entity.IdAccessor.Name.IsNullOrEmpty() ||
                    entity.IdAccessor.Name == column.Name)
                    continue;

                Builder.Insert(column.Name, column.GetValue(entity));
            }
        }

        internal void SetUpdateColumns(TEntity entity)
        {
            foreach (var column in GetTableColumns(entity))
            {
                if (entity.IdAccessor.Name.IsNullOrEmpty() ||
                    entity.IdAccessor.Name == column.Name)
                    continue;

                Builder.Update(column.Name, column.GetValue(entity));
            }
        }
    }
}