using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Skeleton.Abstraction.Data;
using Skeleton.Common;

namespace Skeleton.Infrastructure.Repository
{
    public abstract class AsyncEntityDatabase : DisposableBase
    {
        protected AsyncEntityDatabase(IDatabaseAsync database)
        {
            Database = database;
        }

        protected IDatabaseAsync Database { get; }

        protected override void DisposeManagedResources()
        {
            Database.Dispose();
        }
    }
}
