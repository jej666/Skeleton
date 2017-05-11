using NUnit.Framework;
using Skeleton.Tests.Common;
using System.Linq;

namespace Skeleton.Tests.Core
{
    [TestFixture]
       public class ValidationTests
    {
        [Test]
        public void ValidateCustomer()
        {
            var customer = new Customer { Name = string.Empty };
            var validationResult = customer.Validate(new CustomerValidator());

            Assert.IsFalse(validationResult.IsValid);
            Assert.IsNotNull(validationResult.BrokenRules);
            Assert.IsTrue(validationResult.BrokenRules.First().RuleDescription.Equals("A customer must have a name"));
        }
    }
}