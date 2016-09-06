using System.Collections.Generic;

namespace Skeleton.Core.Repository
{
    public interface ISqlQuery
    {
        IDictionary<string, object> Parameters { get; }
        string Query { get; }

        string PagedQuery(int pageSize, int pageNumber);
    }
}