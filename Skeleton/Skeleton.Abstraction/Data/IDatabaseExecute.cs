using System.Collections.Generic;

namespace Skeleton.Abstraction.Data
{
    public interface IDatabaseExecute
    {
        int Execute(
            string query,
            IDictionary<string, object> parameters);

        TValue ExecuteScalar<TValue>(
            string query,
            IDictionary<string, object> parameters);

        int ExecuteStoredProcedure(
            string procedureName,
            IDictionary<string, object> parameters);
    }
}