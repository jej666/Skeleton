using System.Collections.Generic;
using Skeleton.Common.Extensions;
using Skeleton.Core.Repository;

namespace Skeleton.Tests.Infrastructure
{
    internal static class CustomerCacheKey
    {
        internal static string ForFind(ISqlQuery query)
        {
            return "{0}.Find.{1}".FormatWith(
                typeof(Customer).FullName,
                query.Query);
        }

        internal static string ForFirstOrDefault(int id)
        {
            return "{0}.FirstOrDefault.{1}".FormatWith(
                typeof(Customer).FullName,
                id.ToString());
        }

        internal static string ForFirstOrDefault(ISqlQuery query)
        {
            return "{0}.FirstOrDefault.{1}".FormatWith(
                typeof(Customer).FullName,
                query.Query);
        }

        internal static string ForGetAll()
        {
            return "{0}.GetAll".FormatWith(
                typeof(Customer).FullName);
        }

        internal static string ForPaged(int pageSize, int pageNumber, IDictionary<string, object> parameters)
        {
            return "{0}.Paged.Size{1}.Page{2}.{3}".FormatWith(
                typeof(Customer).FullName,
                pageSize,
                pageNumber,
                parameters.ToString("=", "|"));
        }

        internal static string ForPagedAll(int pageSize, int pageNumber)
        {
            return "{0}.PagedAll.Size{1}.Page{2}".FormatWith(
                typeof(Customer).FullName,
                pageSize,
                pageNumber);
        }
    }
}