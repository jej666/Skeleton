using System;
using System.Collections.Generic;

namespace Skeleton.Tests.Infrastructure
{
    public static class MemorySeeder
    {
        public static IEnumerable<Customer> SeedCustomers(int iterations)
        {
            for (var i = 1; i <= iterations; ++i)
                yield return SeedCustomer(i);
        }

        public static IEnumerable<CustomerDto> SeedCustomerDtos(int iterations)
        {
            for (var i = 1; i <= iterations; ++i)
                yield return SeedCustomerDto(i);
        }

        public static Customer SeedCustomer(int index = 0)
        {
            return new Customer
            {
                Name = $"Customer{(index > 0 ? index.ToString() : "")}",
                CustomerCategoryId = new Random().Next(1, 10)
            };
        }


        public static CustomerDto SeedCustomerDto(int index = 0)
        {
            return new CustomerDto
            {
                Name = $"Customer{(index > 0 ? index.ToString() : "")}",
                CustomerCategoryId = new Random().Next(1, 10)
            };
        }
    }
}