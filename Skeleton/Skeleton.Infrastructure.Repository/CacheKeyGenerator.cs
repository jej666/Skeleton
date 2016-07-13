using System;
using Skeleton.Shared.Abstraction;

namespace Skeleton.Infrastructure.Repository
{
    public sealed class CacheKeyGenerator<TEntity, TIdentity>
        where TEntity : class, IEntity<TEntity, TIdentity>
    {
        private readonly string _prefix;

        public CacheKeyGenerator(bool isAsync)
        {
            _prefix = isAsync ? "async_" : string.Empty;
        }

        public string ForFind(string query)
        {
            return "{0}{1}[Find]-{2}".FormatWith(
                _prefix,
                typeof(TEntity).FullName,
                query);
        }

        public string ForFirstOrDefault(TIdentity id)
        {
            return "{0}{1}[FirstOrDefault]-{2}".FormatWith(
                _prefix,
                typeof(TEntity).FullName,
                id.ToString());
        }

        public string ForFirstOrDefault(string query)
        {
            return "{0}{1}[FirstOrDefault]-{2}".FormatWith(
                _prefix,
                typeof(TEntity).FullName,
                query);
        }

        public string ForGetAll()
        {
            return "{0}{1}[GetAll]".FormatWith(
                _prefix,
                typeof(TEntity).FullName);
        }

        public string ForPage(int pageSize, int pageNumber)
        {
            return "{0}{1}[Page]-Size{2}-Page{3}".FormatWith(
                _prefix,
                typeof(TEntity).FullName,
                pageSize,
                pageNumber);
        }
    }
}