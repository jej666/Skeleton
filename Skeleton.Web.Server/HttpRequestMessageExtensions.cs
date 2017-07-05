using Microsoft.Owin;
using Skeleton.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http.Routing;

namespace Skeleton.Web.Server
{
    public static class HttpRequestMessageExtensions
    {
        public static object EnrichQueryResult<TDto>(
            this HttpRequestMessage request,
            IEnumerable<TDto> items,
            IQuery query) where TDto : class
        {
            if (request == null)
                return null;

            if (items == null || !items.Any())
                return null;

            var totalCount = items.Count();

            if (!query.PageSize.HasValue)
            {
                return new
                {
                    Items = items,
                    TotalCount = totalCount
                };
            }

            var urlHelper = new UrlHelper(request);
            var totalPages = (int)Math.Ceiling((double)totalCount / query.PageSize.Value);
            var prevPageLink = urlHelper.GetPrevLink(query.PageNumber.Value, query.PageSize.Value);
            var nextPageLink = urlHelper.GetNextLink(query.PageNumber.Value, query.PageSize.Value, totalPages);

            return new
            {
                Items = items,
                TotalCount = totalCount,
                TotalPages = totalPages,
                PrevPageLink = prevPageLink,
                NextPageLink = nextPageLink
            };
        }

        public static string GetClientIp(this HttpRequestMessage request)
        {
            if (request == null)
                return null;

            if (request.Properties.ContainsKey(Constants.OwinContext))
            {
                return ((OwinContext)request.Properties[Constants.OwinContext]).Request.RemoteIpAddress;
            }
            return null;
        }

        public static IDictionary<string, string> GetQueryStrings(this HttpRequestMessage request)
        {
            if (request == null)
                return null;

            return request.GetQueryNameValuePairs()
                          .ToDictionary(kv => kv.Key, kv => kv.Value, StringComparer.OrdinalIgnoreCase);
        }

        public static string GetQueryString(this HttpRequestMessage request, string key)
        {
            if (request == null)
                return null;

            var queryStrings = request.GetQueryNameValuePairs();
            if (queryStrings == null)
                return null;

            var match = queryStrings.FirstOrDefault(kv => string.Compare(kv.Key, key, StringComparison.OrdinalIgnoreCase) == 0);
            if (string.IsNullOrEmpty(match.Value))
                return null;

            return match.Value;
        }

        public static string GetHeader(this HttpRequestMessage request, string key)
        {
            if (request == null)
                return null;

            IEnumerable<string> keys = null;
            if (!request.Headers.TryGetValues(key, out keys))
                return null;

            return keys.First();
        }

        private static string GetNextLink(this UrlHelper urlHelper, int pageNumber, int pageSize, int totalPages)
        {
            return pageNumber < totalPages - 1
                ? urlHelper.Link(Constants.DefaultHttpRoute, new { page = pageNumber + 1, pageSize })
                : string.Empty;
        }

        private static string GetPrevLink(this UrlHelper urlHelper, int pageNumber, int pageSize)
        {
            return pageNumber > 0
                ? urlHelper.Link(Constants.DefaultHttpRoute, new { page = pageNumber - 1, pageSize })
                : string.Empty;
        }
    }
}