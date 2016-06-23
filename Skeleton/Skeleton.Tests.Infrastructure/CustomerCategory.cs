using Skeleton.Core.Domain;

namespace Skeleton.Tests.Infrastructure
{
    public class CustomerCategory : Entity<CustomerCategory,int>
    {
        // Need an empty ctor
        public CustomerCategory()
            : base(key => key.CustomerCategoryId)
        {
        }

        public int CustomerCategoryId { get; set; }

        public string Name { get; set; }
    }
}