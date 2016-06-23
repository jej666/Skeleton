using System.Collections.Generic;

namespace Skeleton.Web.Client
{
    public sealed class PagedResult<TDto> where TDto : class
    {
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
        public string PrevPageLink { get; set; }
        public string NextPageLink { get; set; }
        public IEnumerable<TDto> Results { get; set; }
    }
}
