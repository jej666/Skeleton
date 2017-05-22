using System.Collections.Generic;

namespace Skeleton.Abstraction.Data
{
    public interface ISqlCommand
    {
        string SqlQuery { get; }

        IDictionary<string, object> Parameters { get; }
    }
}