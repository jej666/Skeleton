using System;

namespace Skeleton.Abstraction.Data
{
    public interface IAsyncDatabase :
        IHideObjectMethods,
        IDisposable,
        IAsyncDatabaseExecute,
        IAsyncDatabaseQuery
    {
        IDatabaseTransaction Transaction { get; }
    }
}