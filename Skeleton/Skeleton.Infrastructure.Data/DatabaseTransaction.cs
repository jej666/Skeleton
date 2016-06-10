using System.Data;

namespace Skeleton.Infrastructure.Data
{
    public sealed class DatabaseTransaction :
        DataDisposableBase,
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
            if (_database != null)
                _database.DisposeTransaction();
        }
    }
}