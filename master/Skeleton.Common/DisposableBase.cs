﻿using System;

namespace Skeleton.Common
{
    [Serializable]
    public abstract class DisposableBase :
        HideObjectMethodsBase,
        IDisposable
    {
        private bool _disposed;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~DisposableBase()
        {
            Dispose(false);
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