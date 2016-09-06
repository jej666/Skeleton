using System;

namespace Skeleton.Abstraction.Data
{
    public interface IDatabaseAsync :
        IHideObjectMethods,
        IDisposable,
        IDatabaseExecuteAsync,
        IDatabaseQueryAsync
    {
        IDatabaseConfiguration Configuration { get; }

        IDatabaseTransaction Transaction { get; }
    }
}