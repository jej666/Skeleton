using System.Collections.Generic;
using System.Linq;
using System;
using Skeleton.Shared.Abstraction;
using Skeleton.Shared.Abstraction.Reflection;

namespace Skeleton.Infrastructure.Repository.SqlBuilder
{
    internal static class TableColumns
    {
        private static IEnumerable<IMemberAccessor> GetTableColumns<TEntity, TIdentity>(
            this TEntity entity)
            where TEntity : class, IEntity<TEntity, TIdentity>
        {
            return entity.TypeAccessor.GetDeclaredOnlyProperties()
                .Where(x => x.MemberType.IsPrimitiveExtended())
                .ToArray();
        }

        internal static void SetInsertColumns<TEntity, TIdentity>(
            this SqlBuilderManager builder,
            TEntity entity)
            where TEntity : class, IEntity<TEntity, TIdentity>
        {
            foreach (var column in entity.GetTableColumns<TEntity, TIdentity>())
            {
                if (entity.IdAccessor.Name.IsNullOrEmpty() ||
                    entity.IdAccessor.Name == column.Name)
                    continue;

                builder.Insert(column.Name, column.GetValue(entity));
            }
        }

        internal static void SetUpdateColumns<TEntity, TIdentity>(
            this SqlBuilderManager builder,
            TEntity entity)
            where TEntity : class, IEntity<TEntity, TIdentity>
        {
            foreach (var column in entity.GetTableColumns<TEntity, TIdentity>())
            {
                if (entity.IdAccessor.Name.IsNullOrEmpty() ||
                    entity.IdAccessor.Name == column.Name)
                    continue;

                builder.Update(column.Name, column.GetValue(entity));
            }
        }
    }
}