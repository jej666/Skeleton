using System;
using System.Data;

namespace Skeleton.Abstraction.Data
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