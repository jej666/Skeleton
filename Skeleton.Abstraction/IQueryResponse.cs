using System.Collections.Generic;

namespace Skeleton.Abstraction
{
    //Move links to generic metadata dictionary
    public interface IQueryResponse<T>
    {
        string NextPageLink { get; set; }

        string PrevPageLink { get; set; }

        IEnumerable<T> Results { get; set; }

        int TotalCount { get; set; }

        int TotalPages { get; set; }
    }
}