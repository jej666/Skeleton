using Skeleton.Core.Domain;

namespace Skeleton.Tests.Infrastructure
{
    public class CustomerCategory : EntityBase<CustomerCategory, int>
    {
        // Need an empty ctor
        public CustomerCategory()
            : base(pk => pk.CustomerCategoryId)
        {
        }

        public int CustomerCategoryId { get; set; }

        public string Name { get; set; }
    }
}