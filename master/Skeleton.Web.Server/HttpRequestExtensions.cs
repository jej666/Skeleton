using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http.Routing;

namespace Skeleton.Web.Server
{
    public static class HttpRequestExtensions
    {
        public static object SetPagedResult<TDto>(
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