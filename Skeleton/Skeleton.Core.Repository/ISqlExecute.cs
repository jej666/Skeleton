namespace Skeleton.Core.Repository
{
    using System.Collections.Generic;

    public interface ISqlExecute
    {
        string DeleteQuery { get; }
        string InsertQuery { get; }
        IDictionary<string, object> Parameters { get; }
        string UpdateQuery { get; }
    }
}