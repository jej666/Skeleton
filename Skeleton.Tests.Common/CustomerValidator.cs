using Skeleton.Core;
using Skeleton.Core.Domain;

namespace Skeleton.Tests.Common
{
    public class CustomerValidator : EntityValidatorBase<Customer>
    {
        protected override void Validate(Customer entity)
        {
            if (entity == null)
                AddBrokenRule(new ValidationRule("Entity must not be null"));

            if (entity.Name.IsNullOrEmpty())
                AddBrokenRule(new ValidationRule("A customer must have a name"));
        }
    }
}