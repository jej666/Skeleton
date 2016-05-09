namespace Skeleton.Infrastructure.Data
{
    using Common;
    using Configuration;
    using System;

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