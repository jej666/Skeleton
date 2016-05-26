﻿using System;
using Skeleton.Common.Reflection;
using Skeleton.Core.Repository;
using Skeleton.Infrastructure.Data;
using Skeleton.Infrastructure.Data.Configuration;
using Skeleton.Infrastructure.Repository;

namespace Skeleton.Tests.Infrastructure
{
    public class CustomerRepository : Repository<Customer, int>
    {
        public CustomerRepository(
            ITypeAccessorCache typeAccessorCache,
            IDatabase database)
            : base(typeAccessorCache, database)
        {
        }
    }
}