using System;
using System.Collections.Generic;

namespace Skeleton.Abstraction.Orm
{
    public interface IStoredProcedureExecutor :
            IDisposable,
            IHideObjectMethods
    {
        int Execute(string storedProcedureName, IDictionary<string, object> parameters);
    }
}