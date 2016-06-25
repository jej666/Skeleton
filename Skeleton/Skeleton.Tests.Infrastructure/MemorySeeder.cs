using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skeleton.Tests.Infrastructure
{
    public static class MemorySeeder
    {
        public static IEnumerable<Customer> SeedCustomers(int iterations)
        {
            for (var i = 0; i < iterations; ++i)
                yield return new Customer { Name = "Customer" + i };
        }

        public static IEnumerable<CustomerDto> SeedCustomerDtos(int iterations)
        {
            for (var i = 0; i < iterations; ++i)
                yield return new CustomerDto { Name = "Customer" + i };
        }
    }
}
