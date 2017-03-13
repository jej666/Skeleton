using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http.Routing;

namespace Skeleton.Web.Server
{
    public static class HttpRequestMessageExtensions
    {
        public static object ToPagedResult<TDto>(
            this HttpRequestMessage request,
            int totalCount,
            int pageNumber,
            int pageSize,
            IEnumerable<TDto> pagedData)
        {
            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
            var urlHelper = new UrlHelper(request);
            var prevLink = urlHelper.GetPrevLink(pageNumber, pageSize);
            var nextLink = urlHelper.GetNextLink(pageNumber, pageSize, totalPages);

            return new
            {
                TotalCount = totalCount,
                TotalPages = totalPages,
                PrevPageLink = prevLink,
                NextPageLink = nextLink,
                Results = pagedData
            };
        }

        public static IDictionary<string, string> GetQueryStrings(this HttpRequestMessage request)
        {
            return request.GetQueryNameValuePairs()
                          .ToDictionary(kv => kv.Key, kv => kv.Value, StringComparer.OrdinalIgnoreCase);
        }

        public static string GetQueryString(this HttpRequestMessage request, string key)
        {
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