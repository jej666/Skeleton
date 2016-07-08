using System;
using Skeleton.Core.Repository;
using Skeleton.Shared.Abstraction;

namespace Skeleton.Infrastructure.Repository
{
    public sealed class CacheKeyGenerator<TEntity, TIdentity> :
        ICacheKeyGenerator<TEntity, TIdentity>
        where TEntity : class, IEntity<TEntity, TIdentity>
    {
        public string ForFind(ISqlQuery query)
        {
            return "{0}.Find.{1}".FormatWith(
                typeof(TEntity).FullName,
                query.Query);
        }

        public string ForFirstOrDefault(TIdentity id)
        {
            return "{0}.FirstOrDefault.{1}".FormatWith(
                typeof(TEntity).FullName,
                id.ToString());
        }

        public string ForFirstOrDefault(ISqlQuery query)
        {
            return "{0}.FirstOrDefault.{1}".FormatWith(
                typeof(TEntity).FullName,
                query.Query);
        }

        public string ForGetAll()
        {
            return "{0}.GetAll".FormatWith(
                typeof(TEntity).FullName);
        }

        public string ForPage(int pageSize, int pageNumber)
        {
            return "{0}.Page.Size{1}.Page{2}".FormatWith(
                typeof(TEntity).FullName,
                pageSize,
                pageNumber);
        }
    }
}