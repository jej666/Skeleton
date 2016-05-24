using System;
using Skeleton.Common.Reflection;
using Skeleton.Infrastructure.Data;
using Skeleton.Infrastructure.Data.Configuration;
using Skeleton.Infrastructure.Repository;

namespace Skeleton.Tests.Infrastructure
{
    public class CustomerCategoryRepository : RepositoryBase<CustomerCategory, int>
    {
        public CustomerCategoryRepository(
            ITypeAccessorCache typeAccessorCache,
            IDatabase database)
            : base(typeAccessorCache, database)
        {
        }
    }
}