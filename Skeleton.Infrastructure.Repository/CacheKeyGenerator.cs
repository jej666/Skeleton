using Skeleton.Abstraction;
using Skeleton.Abstraction.Domain;
using Skeleton.Common;

namespace Skeleton.Infrastructure.Repository
{
    public class CacheKeyGenerator<TEntity>
        where TEntity : class, IEntity<TEntity>
    {
        public CacheKeyGenerator()
        {
            Prefix = string.Empty;
        }

        protected string Prefix { get; set; }

        public string ForFind(string query)
        {
            query.ThrowIfNullOrEmpty(() => query);

            return "{0}{1}[Find]-{2}".FormatWith(
                Prefix,
                typeof(TEntity).FullName,
                query);
        }

        public string ForFirstOrDefault(object id)
        {
            id.ThrowIfNull(() => id);

            return "{0}{1}[FirstOrDefault]-{2}".FormatWith(
                Prefix,
                typeof(TEntity).FullName,
                id.ToString());
        }

        public string ForFirstOrDefault(string query)
        {
            query.ThrowIfNullOrEmpty(() => query);

            return "{0}{1}[FirstOrDefault]-{2}".FormatWith(
                Prefix,
                typeof(TEntity).FullName,
                query);
        }

        public string ForPage(int pageSize, int pageNumber)
        {
            return "{0}{1}[Page]-Size{2}-Page{3}".FormatWith(
                Prefix,
                typeof(TEntity).FullName,
                pageSize,
                pageNumber);
        }
    }
}