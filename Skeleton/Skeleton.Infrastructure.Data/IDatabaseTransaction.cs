namespace Skeleton.Infrastructure.Data
{
    using Common;
    using System;
    using System.Data;

    public interface IDatabaseTransaction :
        IHideObjectMethods,
        IDisposable
    {
        void Begin();

        void Begin(IsolationLevel isolationLevel);

        void Commit();
    }
}