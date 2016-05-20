using System.Collections.Generic;

namespace Skeleton.Tests.Infrastructure
{
    public class CustomerSeeder
    {
        private readonly IList<Customer> _customers = new List<Customer>();

        public IEnumerable<Customer> Seed(int iteration)
        {
            for (var i = 0; i <= iteration; ++i)
                _customers.Add(new Customer {Name = "Name" + i});

            return _customers;
        }
    }
}