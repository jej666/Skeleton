using Skeleton.Core.Domain;

namespace Skeleton.Tests.Common
{
    public class CustomerCategory : EntityBase<CustomerCategory>
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