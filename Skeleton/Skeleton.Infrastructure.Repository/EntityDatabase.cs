using Skeleton.Abstraction.Data;
using Skeleton.Common;

namespace Skeleton.Infrastructure.Repository
{
    public abstract class EntityDatabase : DisposableBase
    {
        protected EntityDatabase(IDatabase database)
        {
            Database = database;
        }

        protected IDatabase Database { get; }

        protected override void DisposeManagedResources()
        {
            Database.Dispose();
        }
    }
}