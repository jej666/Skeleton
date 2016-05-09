namespace Skeleton.Infrastructure.Service
{
    using Common;
    using Common.Extensions;
    using Core.Service;
    using System;

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