using Skeleton.Abstraction;

namespace Skeleton.Core
{
    public class Query : IQuery
    {
        public string Fields { get; set; }

        public string OrderBy { get; set; }

        public int? PageNumber { get; set; }

        public int? PageSize { get; set; }
    }
}