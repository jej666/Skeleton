using Skeleton.Abstraction;
using Skeleton.Common;
using Skeleton.Core.Service;
using System;

namespace Skeleton.Infrastructure.Service
{
    public abstract class EntityService<TEntity, TIdentity> :
        DisposableBase,
        IEntityService<TEntity, TIdentity>
        where TEntity : class, IEntity<TEntity, TIdentity>
    {
        private readonly ILogger _logger;

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
    }
}