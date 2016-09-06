using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Skeleton.Common.Extensions;
using Skeleton.Common.Reflection;
using Skeleton.Core.Domain;
using Skeleton.Core.Repository;

namespace Skeleton.Infrastructure.Repository.SqlBuilder
{
    public sealed class SqlExecuteBuilder<TEntity, TIdentity> :
        SqlBuilderBase,
        IExecuteBuilder<TEntity, TIdentity>,
        IExecuteWhereBuilder<TEntity, TIdentity>
        where TEntity : class, IEntity<TEntity, TIdentity>
    {
        private readonly Func<IMemberAccessor, bool> _simplePropertiesCondition =
            x => x.MemberType.IsPrimitive ||
                 x.MemberType == typeof(decimal) ||
                 x.MemberType == typeof(string);

        private readonly ITypeAccessor _accessor;
        private TEntity _entity;

        public SqlExecuteBuilder(ITypeAccessorCache accessorCache)
            : base(TableInfo.GetTableName<TEntity>())
        {
            accessorCache.ThrowIfNull(() => accessorCache);

            _accessor = accessorCache.Get<TEntity>();
        }

        private IEnumerable<IMemberAccessor> TableColumns
        {
            get
            {
                return _accessor.GetDeclaredOnlyProperties()
                    .Where(_simplePropertiesCondition)
                    .ToArray();
            }
        }

        public ISqlExecute AsSql()
        {
            return this;
        }

        public IExecuteWhereBuilder<TEntity, TIdentity> Delete(TEntity entity)
        {
            entity.ThrowIfNull(() => entity);
            _entity = entity;

            return this;
        }

        public IExecuteBuilder<TEntity, TIdentity> Insert(TEntity entity)
        {
            entity.ThrowIfNull(() => entity);
            _entity = entity;
            TableColumns.ForEach(SetInsertByColumn);

            return this;
        }

        public IExecuteWhereBuilder<TEntity, TIdentity> Update(TEntity entity)
        {
            entity.ThrowIfNull(() => entity);
            _entity = entity;
            TableColumns.ForEach(SetUpdateByColumn);

            return this;
        }

        public IExecuteBuilder<TEntity, TIdentity> Where(Expression<Func<TEntity, bool>> expression)
        {
            Builder.And();
            Resolver.ResolveQuery(expression);

            return this;
        }

        public IExecuteBuilder<TEntity, TIdentity> WhereIsIn(
            Expression<Func<TEntity, object>> expression,
            IEnumerable<object> values)
        {
            Builder.And();
            Resolver.QueryByIsIn(expression, values);

            return this;
        }

        public IExecuteBuilder<TEntity, TIdentity> WhereNotIn(
            Expression<Func<TEntity, object>> expression,
            IEnumerable<object> values)
        {
            Builder.And();
            Resolver.QueryByNotIn(expression, values);

            return this;
        }

        public IExecuteBuilder<TEntity, TIdentity> WherePrimaryKey(
            Expression<Func<TEntity, bool>> whereExpression)
        {
            Builder.And();
            Resolver.QueryByPrimaryKey(_entity.IdAccessor.Name, whereExpression);

            return this;
        }

        private void SetInsertByColumn(IMemberAccessor column)
        {
            if (_entity.IdAccessor.Name.IsNullOrEmpty() ||
                _entity.IdAccessor.Name == column.Name)
                return;

            Builder.AddInsert(column.Name, column.GetValue(_entity));
        }

        private void SetUpdateByColumn(IMemberAccessor column)
        {
            if (_entity.IdAccessor.Name.IsNullOrEmpty() ||
                _entity.IdAccessor.Name == column.Name)
                return;

            Builder.AddUpdate(column.Name, column.GetValue(_entity));
        }
    }
}