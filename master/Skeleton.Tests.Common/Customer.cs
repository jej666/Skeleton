using Skeleton.Core.Domain;

namespace Skeleton.Tests.Common
{
    public class Customer : EntityBase<Customer>
    {
        // Need an empty ctor and an Id definition
        public Customer()
            : base(e => e.CustomerId)
        {
        }

        public int CustomerId { get; set; }

        public int CustomerCategoryId { get; set; }

        public string Name { get; set; }
    }
}