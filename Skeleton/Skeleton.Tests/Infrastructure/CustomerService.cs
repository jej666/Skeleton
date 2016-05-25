using Skeleton.Common;
using Skeleton.Core.Repository;
using Skeleton.Infrastructure.Service;

namespace Skeleton.Tests.Infrastructure
{
    public sealed class CustomerService : Service
    {
        public CustomerService(ILogger logger, IUnitOfWork unitOfWork) : base(logger, unitOfWork)
        {
      
        }
    }
}