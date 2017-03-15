using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Skeleton.Abstraction.Orm
{
    public interface IAsyncStoredProcedureExecutor :
            IDisposable,
            IHideObjectMethods
    {
        Task<int> ExecuteAsync(string storedProcedureName, IDictionary<string, object> parameters);
    }
}