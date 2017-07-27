using Skeleton.Abstraction.Data;
using Skeleton.Abstraction.Orm;
using Skeleton.Core;
using Skeleton.Infrastructure.Orm.SqlBuilder;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Skeleton.Infrastructure.Orm
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
            storedProcedureName.ThrowIfNullOrEmpty(nameof(storedProcedureName));
            parameters.ThrowIfNull(nameof(parameters));

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