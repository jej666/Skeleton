using System;
using Skeleton.Common;
using Skeleton.Common.Extensions;
using Skeleton.Core.Domain;
using Skeleton.Core.Service;

namespace Skeleton.Infrastructure.Service
{
    public abstract class EntityService<TEntity, TIdentity> :
       DisposableBase,
       IEntityService<TEntity,TIdentity> 
       where TEntity : class, IEntity<TEntity, TIdentity>
    {
        private readonly ILogger _logger;

        protected EntityService(ILogger logger)
        {
            logger.ThrowIfNull(() => logger);

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
    }
}