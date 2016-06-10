using System;
using Skeleton.Abstraction;
using Skeleton.Infrastructure.Data.Configuration;

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