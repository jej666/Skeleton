﻿using Skeleton.Tests.Common;
using Skeleton.Web.Client;

namespace Skeleton.Tests.Web.Mock
{
    public class CachedCustomersHttpClient : CrudHttpClient<CustomerDto>
    {
        public CachedCustomersHttpClient()
            : base("http://localhost:8081/", "api/cachedcustomers")
        {
        }
    }
}