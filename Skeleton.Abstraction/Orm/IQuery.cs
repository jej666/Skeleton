namespace Skeleton.Abstraction.Orm
{
    //Commit forced
    public interface IQuery
    {
        int? PageNumber { get; set; }

        int? PageSize { get; set; }

        string OrderBy { get; set; }

        string Fields { get; set; }
    }
}
