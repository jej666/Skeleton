using Skeleton.Abstraction.Data;
using Skeleton.Core;
using System.Data;

namespace Skeleton.Infrastructure.Data
{
    public sealed class DatabaseTransaction :
        DisposableBase,
        IDatabaseTransaction
    {
        private readonly DatabaseContext _database;

        public DatabaseTransaction(DatabaseContext database)
        {
            _database = database;
        }

        public void Begin()
        {
            _database.BeginTransaction(null);
        }

        public void Begin(IsolationLevel isolationLevel)
        {
            _database.BeginTransaction(isolationLevel);
        }

        public void Commit()
        {
            _database.CommitTransaction();
        }

        protected override void DisposeManagedResources()
        {
            _database?.DisposeTransaction();
            base.DisposeManagedResources();
        }
    }
}