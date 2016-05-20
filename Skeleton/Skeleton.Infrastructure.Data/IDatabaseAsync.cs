using System;
using Skeleton.Common;
using Skeleton.Infrastructure.Data.Configuration;

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