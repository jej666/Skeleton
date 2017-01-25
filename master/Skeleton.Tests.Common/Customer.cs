using Skeleton.Core.Domain;

namespace Skeleton.Tests.Common
{
    public class Customer : Entity<Customer>
    {
        // Need an empty ctor
        public Customer()
            : base(e => e.CustomerId)
        {
        }

        public int CustomerId { get; set; }

        public int CustomerCategoryId { get; set; }

        public string Name { get; set; }
    }
}