using System;

namespace Skeleton.Abstraction.Data
{
    public interface IDatabase :
        IHideObjectMethods,
        IDisposable,
        IDatabaseExecute,
        IDatabaseQuery
    {
        IDatabaseTransaction Transaction { get; }
    }
}