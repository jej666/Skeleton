namespace Skeleton.Web.Client
{
    public class Query
    {
        public string Fields { get; set; }

        public string OrderBy { get; set; }

        public int? PageNumber { get; set; }

        public int? PageSize { get; set; }
    }
}