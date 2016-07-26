using System;
using Skeleton.Infrastructure.Data.Configuration;
using Skeleton.Shared.Abstraction;

namespace Skeleton.Infrastructure.Data
{
    public interface IDatabase :
        IHideObjectMethods,
        IDisposable,
        IDatabaseExecute,
        IDatabaseQuery
    {
        IDatabaseConfiguration Configuration { get; }

        IDatabaseTransaction Transaction { get; }
    }
}