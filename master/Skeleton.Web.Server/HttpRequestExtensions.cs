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
            var totalPages = (int) Math.Ceiling((double) totalCount/pageSize);
            var urlHelper = new UrlHelper(request);
            var prevLink = pageNumber > 0
                ? urlHelper.Link("DefaultApiWithId", new {page = pageNumber - 1, pageSize})
                : "";
            var nextLink = pageNumber < totalPages - 1
                ? urlHelper.Link("DefaultApiWithId", new {page = pageNumber + 1, pageSize})
                : "";

            return new
            {
                TotalCount = totalCount,
                TotalPages = totalPages,
                PrevPageLink = prevLink,
                NextPageLink = nextLink,
                Results = pagedData
            };
        }
    }
}