namespace Skeleton.Common
{
    using System;

    [Serializable]
    public abstract class DisposableBase :
        HideObjectMethods,
        IDisposable
    {
        private bool _disposed;

        ~DisposableBase()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void DisposeManagedResources()
        {
        }

        protected virtual void DisposeUnmanagedResources()
        {
        }

        private void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
                DisposeManagedResources();

            DisposeUnmanagedResources();
            _disposed = true;
        }
    }
}