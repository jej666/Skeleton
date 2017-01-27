using System;

namespace Skeleton.Abstraction.Data
{
    public interface IAsyncDatabase :
        IHideObjectMethods,
        IDisposable,
        IAsyncDatabaseExecute,
        IAsyncDatabaseQuery
    {
        IDatabaseConfiguration Configuration { get; }

        IDatabaseTransaction Transaction { get; }
    }
}