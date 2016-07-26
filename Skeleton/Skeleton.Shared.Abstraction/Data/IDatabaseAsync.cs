using System;
using Skeleton.Infrastructure.Data.Configuration;
using Skeleton.Shared.Abstraction;

namespace Skeleton.Infrastructure.Data
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