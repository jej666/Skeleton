using System;

namespace Skeleton.Infrastructure.Data
{
    [Serializable]
    public abstract class DataDisposableBase :
        DataHideObjectMethods,
        IDisposable
    {
        private bool _disposed;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~DataDisposableBase()
        {
            Dispose(false);
        }

        protected virtual void DisposeManagedResources()
        {
        }

        private void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
                DisposeManagedResources();

            _disposed = true;
        }
    }
}