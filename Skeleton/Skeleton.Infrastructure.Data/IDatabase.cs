namespace Skeleton.Infrastructure.Data
{
    using Common;
    using Configuration;
    using System;

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