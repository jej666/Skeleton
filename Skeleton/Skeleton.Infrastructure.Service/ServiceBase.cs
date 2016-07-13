using System;
using Skeleton.Shared.Abstraction;

namespace Skeleton.Infrastructure.Service
{
    public abstract class ServiceBase :
        HideObjectMethods,
        IDisposable
    {
        private readonly ILogger _logger;
        private bool _disposed;

        protected ServiceBase(ILogger logger)
        {
            _logger = logger;
        }

        protected T HandleException<T>(Func<T> handler)
        {
            try
            {
                return handler();
            }
            catch (Exception e)
            {
                _logger.Error(e.Message);
                throw;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~ServiceBase()
        {
            Dispose(false);
        }

        protected abstract void DisposeManagedResources();

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