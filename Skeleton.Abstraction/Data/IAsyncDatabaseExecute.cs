using System.Threading.Tasks;

namespace Skeleton.Abstraction.Data
{
    public interface IAsyncDatabaseExecute
    {
        Task<int> ExecuteAsync(ISqlCommand command);

        Task<object> ExecuteScalarAsync(ISqlCommand command);

        Task<TValue> ExecuteScalarAsync<TValue>(ISqlCommand command);

        Task<int> ExecuteStoredProcedureAsync(ISqlCommand command);
    }
}