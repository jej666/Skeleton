using Skeleton.Abstraction.Data;
using Skeleton.Abstraction.Repository;
using Skeleton.Common;
using Skeleton.Infrastructure.Repository.SqlBuilder;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Skeleton.Infrastructure.Repository
{
    public sealed class AsyncStoredProcedureExecutor :
        DisposableBase,
        IAsyncStoredProcedureExecutor
    {
        private readonly IAsyncDatabase _database;

        public AsyncStoredProcedureExecutor(IAsyncDatabase database)
        {
            _database = database;
        }

        public async Task<int> ExecuteAsync(string storedProcedureName, IDictionary<string, object> parameters)
        {
            storedProcedureName.ThrowIfNullOrEmpty(() => storedProcedureName);
            parameters.ThrowIfNull(() => parameters);

            var command = new SqlCommand(storedProcedureName, parameters);

            return await _database.ExecuteStoredProcedureAsync(command);
        }

        protected override void DisposeManagedResources()
        {
            base.DisposeManagedResources();
            _database.Dispose();
        }
    }
}