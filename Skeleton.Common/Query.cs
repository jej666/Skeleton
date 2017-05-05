using Skeleton.Abstraction.Orm;

namespace Skeleton.Common
{
    public class Query : IQuery
    {
        public virtual string Fields { get; set; }
      
        public virtual string OrderBy { get; set; }

        public virtual int? PageNumber { get; set; }

        public virtual int? PageSize { get; set; }
    }
}