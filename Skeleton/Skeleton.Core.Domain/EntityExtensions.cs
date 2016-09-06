using System.Text;
using System;
using Skeleton.Abstraction;

namespace Skeleton.Core.Domain
{
    public static class EntityExtensions
    {
        public static string Stringify<TEntity, TIdentity>(
            this IEntity<TEntity, TIdentity> entity,
            ITypeAccessor typeAccessor)
            where TEntity : class, IEntity<TEntity, TIdentity>
        {
            var builder = new StringBuilder();

            foreach (var property in typeAccessor.GetDeclaredOnlyProperties())
                builder.AppendLine("{0} : {1}".FormatWith(property.Name, property.GetValue(entity)));

            return builder.ToString();
        }
    }
}