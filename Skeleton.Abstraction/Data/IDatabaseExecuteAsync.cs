using System.Collections.Generic;
using System.Threading.Tasks;

namespace Skeleton.Abstraction.Data
{
    public interface IDatabaseExecuteAsync
    {
        Task<int> ExecuteAsync(
            string query,
            IDictionary<string, object> parameters);

        Task<TValue> ExecuteScalarAsync<TValue>(
            string query,
            IDictionary<string, object> parameters);

        Task<int> ExecuteStoredProcedureAsync(
            string procedureName,
            IDictionary<string, object> parameters);
    }
}