using System;
using Skeleton.Common;
using Skeleton.Common.Extensions;
using Skeleton.Core.Repository;
using Skeleton.Core.Service;

namespace Skeleton.Infrastructure.Service
{
    public abstract class Service :
        DisposableBase,
        IAggregateService
    {
        private readonly ILogger _logger;
        private readonly IUnitOfWork _unitOfWork;

        protected Service(ILogger logger, IUnitOfWork unitOfWork)
        {
            logger.ThrowIfNull(() => logger);
            unitOfWork.ThrowIfNull(() => unitOfWork);

            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IUnitOfWork UnitOfWork
        {
            get { return _unitOfWork; }
        }

        protected T HandleException<T>(Func<T> handler)
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

        protected override void DisposeManagedResources()
        {
            _unitOfWork.ResolveAll().ForEach(
                repository => repository.Dispose());
        }
    }
}