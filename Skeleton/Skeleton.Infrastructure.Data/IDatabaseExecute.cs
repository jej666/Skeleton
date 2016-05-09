namespace Skeleton.Infrastructure.Data
{
    using System.Collections.Generic;

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