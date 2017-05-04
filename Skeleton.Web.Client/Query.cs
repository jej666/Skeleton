namespace Skeleton.Web.Client
{
    public class Query 
    {
        public virtual string Fields { get; set; }

        public virtual string OrderBy { get; set; }

        public virtual int? PageNumber { get; set; }

        public virtual int? PageSize { get; set; }
    }
}
