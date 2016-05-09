namespace Skeleton.Core.Repository
{
    using System.Collections.Generic;

    public interface ISqlQuery
    {
        IDictionary<string, object> Parameters { get; }
        string Query { get; }

        string PagedQuery(int pageSize, int pageNumber);
    }
}