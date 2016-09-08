using Skeleton.Abstraction;
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
            return "{0}{1}[Find]-{2}".FormatWith(
                Prefix,
                typeof(TEntity).FullName,
                query);
        }

        public string ForFirstOrDefault(object id)
        {
            return "{0}{1}[FirstOrDefault]-{2}".FormatWith(
                Prefix,
                typeof(TEntity).FullName,
                id.ToString());
        }

        public string ForFirstOrDefault(string query)
        {
            return "{0}{1}[FirstOrDefault]-{2}".FormatWith(
                Prefix,
                typeof(TEntity).FullName,
                query);
        }

        public string ForGetAll()
        {
            return "{0}{1}[GetAll]".FormatWith(
                Prefix,
                typeof(TEntity).FullName);
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