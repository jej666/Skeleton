﻿using Skeleton.Web.Client;

namespace Skeleton.Tests.Infrastructure
{
    public class CustomerHttpClient : CrudHttpClient<CustomerDto, int>
    {
        public CustomerHttpClient()
            : base("http://localhost:8081/", "api/customer/")
        {
        }
    }
}
