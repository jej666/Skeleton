using System;
using System.Data;
using Skeleton.Common;

namespace Skeleton.Infrastructure.Data
{
    public interface IDatabaseTransaction :
        IHideObjectMethods,
        IDisposable
    {
        void Begin();

        void Begin(IsolationLevel isolationLevel);

        void Commit();
    }
}