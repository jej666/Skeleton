using Skeleton.Abstraction;
using System.Collections.Generic;

namespace Skeleton.Common
{
    public class QueryResponse<T> : IQueryResponse<T>
    {
        public int TotalCount { get; set; }

        public int TotalPages { get; set; }

        public string PrevPageLink { get; set; }

        public string NextPageLink { get; set; }

        public IEnumerable<T> Results { get; set; }
    }
}