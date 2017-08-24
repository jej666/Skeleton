using Skeleton.Abstraction.Domain;
using Skeleton.Core;

namespace Skeleton.Infrastructure.Orm
{
    internal class CacheKeyGenerator<TEntity>
        where TEntity : class, IEntity<TEntity>, new()
    {
        public CacheKeyGenerator()
        {
            Prefix = string.Empty;
        }

        protected internal string Prefix { get; set; }

        internal string ForFind(string query)
        {
            query.ThrowIfNullOrEmpty();

            return Template.Find.FormatWith(
                Prefix,
                typeof(TEntity).FullName,
                query);
        }

        internal string ForFirstOrDefault(object id)
        {
            id.ThrowIfNull();

            return Template.FirstOrDefault.FormatWith(
                Prefix,
                typeof(TEntity).FullName,
                id.ToString());
        }

        internal string ForFirstOrDefault(string query)
        {
            query.ThrowIfNullOrEmpty();

            return Template.FirstOrDefault.FormatWith(
                Prefix,
                typeof(TEntity).FullName,
                query);
        }

        private static class Template
        {
            internal const string Find = "{0}{1}[Find]-{2}";
            internal const string FirstOrDefault = "{0}{1}[FirstOrDefault]-{2}";
        }
    }
}