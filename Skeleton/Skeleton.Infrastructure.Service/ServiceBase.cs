using System;
using Skeleton.Common;
using Skeleton.Common.Extensions;
using Skeleton.Core.Service;

namespace Skeleton.Infrastructure.Service
{
    public abstract class ServiceBase :
        DisposableBase,
        IService
    {
        private readonly ILogger _logger;

        protected ServiceBase(ILogger logger)
        {
            logger.ThrowIfNull(() => logger);

            _logger = logger;
        }

        protected virtual T HandleException<T>(Func<T> handler)
        {
            handler.ThrowIfNull(() => handler);

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