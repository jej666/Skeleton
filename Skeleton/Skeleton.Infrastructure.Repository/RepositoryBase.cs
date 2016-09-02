using System;
using Skeleton.Core;
using Skeleton.Shared;

namespace Skeleton.Infrastructure.Repository
{
    public abstract class RepositoryBase : DisposableBase
    {
        private readonly ILogger _logger;

        protected RepositoryBase(ILogger logger)
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