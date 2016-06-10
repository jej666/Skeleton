using Skeleton.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Skeleton.Infrastructure.Repository.SqlBuilder
{
    internal static class TableColumns
    {
        private static readonly Func<IMemberAccessor, bool> SimplePropertiesCondition =
            x => x.MemberType.IsPrimitive ||
                 x.MemberType == typeof(decimal) ||
                 x.MemberType == typeof(string);

        internal static IEnumerable<IMemberAccessor> GetTableColumns(
            this ITypeAccessor typeAccessor)
        {
            return typeAccessor.GetDeclaredOnlyProperties()
                .Where(SimplePropertiesCondition)
                .ToArray();
        }

        internal static void SetInsertColumns<TEntity, TIdentity>(
            this SqlBuilderManager builder,
            IEnumerable<IMemberAccessor> columns,
            TEntity entity)
            where TEntity : class, IEntity<TEntity, TIdentity>
        {
            foreach (var column in columns)
            {
                if (entity.IdAccessor.Name.IsNullOrEmpty() ||
                    entity.IdAccessor.Name == column.Name)
                    continue;

                builder.Insert(column.Name, column.GetValue(entity));
            }
        }

        internal static void SetUpdateColumns<TEntity, TIdentity>(
            this SqlBuilderManager builder,
            IEnumerable<IMemberAccessor> columns,
            TEntity entity)
            where TEntity : class, IEntity<TEntity, TIdentity>
        {
            foreach (var column in columns)
            {
                if (entity.IdAccessor.Name.IsNullOrEmpty() ||
                    entity.IdAccessor.Name == column.Name)
                    continue;

                builder.Update(column.Name, column.GetValue(entity));
            }
        }
    }
}