using Skeleton.Abstraction.Data;
using Skeleton.Abstraction.Repository;
using Skeleton.Common;
using Skeleton.Infrastructure.Repository.SqlBuilder;
using System.Collections.Generic;

namespace Skeleton.Infrastructure.Repository
{
    public sealed class StoredProcedureExecutor :
        DisposableBase,
        IStoredProcedureExecutor
    {
        private readonly IDatabase _database;

        public StoredProcedureExecutor(IDatabase database)
        {
            _database = database;
        }

        public int Execute(string storedProcedureName, IDictionary<string, object> parameters)
        {
            var command = new SqlCommand(storedProcedureName, parameters);

            return _database.ExecuteStoredProcedure(command);
        }

        protected override void DisposeManagedResources()
        {
            base.DisposeManagedResources();
            _database.Dispose();
        }
    }
}