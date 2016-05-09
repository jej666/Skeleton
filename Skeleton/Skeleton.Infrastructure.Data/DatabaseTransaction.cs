namespace Skeleton.Infrastructure.Data
{
    using Skeleton.Common;
    using System.Data;

    public sealed class DatabaseTransaction :
        DisposableBase,
        IDatabaseTransaction
    {
        private readonly DatabaseBase _database;

        public DatabaseTransaction(DatabaseBase database)
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