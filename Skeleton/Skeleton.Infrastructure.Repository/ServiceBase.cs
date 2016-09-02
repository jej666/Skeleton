using System;
using Skeleton.Core;
using Skeleton.Shared.CommonTypes;

namespace Skeleton.Infrastructure.Repository
{
    public abstract class ServiceBase :DisposableBase
    {
        private readonly ILogger _logger;
       
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
    }
}