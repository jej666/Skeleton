using System.Collections.Generic;

namespace Skeleton.Core.Repository
{
    public interface ISqlExecute
    {
        string DeleteQuery { get; }
        string InsertQuery { get; }
        IDictionary<string, object> Parameters { get; }
        string UpdateQuery { get; }
    }
}