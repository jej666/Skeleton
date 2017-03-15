using Skeleton.Abstraction.Domain;
using Skeleton.Common;

namespace Skeleton.Infrastructure.Orm
{
    internal class CacheKeyGenerator<TEntity>
        where TEntity : class, IEntity<TEntity>
    {
        public CacheKeyGenerator()
        {
            Prefix = string.Empty;
        }

        protected internal string Prefix { get; set; }

        internal string ForFind(string query)
        {
            query.ThrowIfNullOrEmpty(() => query);

            return Template.Find.FormatWith(
                Prefix,
                typeof(TEntity).FullName,
                query);
        }

        internal string ForFirstOrDefault(object id)
        {
            id.ThrowIfNull(() => id);

            return Template.FirstOrDefault.FormatWith(
                Prefix,
                typeof(TEntity).FullName,
                id.ToString());
        }

        internal string ForFirstOrDefault(string query)
        {
            query.ThrowIfNullOrEmpty(() => query);

            return Template.FirstOrDefault.FormatWith(
                Prefix,
                typeof(TEntity).FullName,
                query);
        }

        internal string ForPage(int pageSize, int pageNumber)
        {
            return Template.Page.FormatWith(
                Prefix,
                typeof(TEntity).FullName,
                pageSize,
                pageNumber);
        }

        private static class Template
        {
            internal const string Find = "{0}{1}[Find]-{2}";
            internal const string FirstOrDefault = "{0}{1}[FirstOrDefault]-{2}";
            internal const string Page = "{0}{1}[Page]-Size{2}-Page{3}";
        } 
    }
}