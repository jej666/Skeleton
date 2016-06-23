﻿using Skeleton.Abstraction;
using Skeleton.Core.Service;
using System;
using System.ComponentModel;

namespace Skeleton.Infrastructure.Service
{
    public abstract class EntityService<TEntity, TIdentity> :
        IEntityService<TEntity, TIdentity>
        where TEntity : class, IEntity<TEntity, TIdentity>
    {
        private readonly ILogger _logger;
        private bool _disposed;

        protected EntityService(ILogger logger)
        {
            logger.ThrowIfNull(() => logger);

            _logger = logger;
        }

        public ILogger Logger
        {
            get { return _logger; }
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

        ~EntityService()
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

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public new Type GetType()
        {
            return base.GetType();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override string ToString()
        {
            return base.ToString();
        }
    }
}