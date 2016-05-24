﻿using Skeleton.Common.Extensions;
using Skeleton.Core.Domain;
using Skeleton.Core.Repository;

namespace Skeleton.Infrastructure.Repository
{
    internal sealed class CacheKeyGenerator<TEntity, TIdentity>
        where TEntity : class, IEntity<TEntity, TIdentity>
    {
        internal string ForFind(ISqlQuery query)
        {
            return "{0}.Find.{1}".FormatWith(
                typeof(TEntity).FullName,
                query.Query);
        }

        internal string ForFirstOrDefault(TIdentity id)
        {
            return "{0}.FirstOrDefault.{1}".FormatWith(
                typeof(TEntity).FullName,
                id.ToString());
        }

        internal string ForFirstOrDefault(ISqlQuery query)
        {
            return "{0}.FirstOrDefault.{1}".FormatWith(
                typeof(TEntity).FullName,
                query.Query);
        }

        internal string ForGetAll()
        {
            return "{0}.GetAll".FormatWith(
                typeof(TEntity).FullName);
        }

        internal string ForPageAll(int pageSize, int pageNumber)
        {
            return "{0}.PagedAll.Size{1}.Page{2}".FormatWith(
                typeof(TEntity).FullName,
                pageSize,
                pageNumber);
        }
    }
}