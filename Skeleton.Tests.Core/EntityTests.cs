using NUnit.Framework;
using Skeleton.Core.Domain;
using Skeleton.Tests.Common;
using System.Linq;

namespace Skeleton.Tests.Core
{
    [TestFixture]
    public class EntityTests
    {
        [Test]
        public void Entity_Can_Validate()
        {
            var customer = new Customer { Name = string.Empty };
            var validationResult = customer.Validate(new CustomerValidator());

            Assert.IsFalse(validationResult.IsValid);
            Assert.IsNotNull(validationResult.BrokenRules);
            Assert.IsTrue(validationResult.BrokenRules.First().RuleDescription.Equals("A customer must have a name"));
        }

        [Test]
        public void Entity_NotEquals()
        {
            var customer1 = new Customer { CustomerId = 1 };
            var customer2 = new Customer { CustomerId = 2 };

            EqualityTests.TestUnequalObjects(customer1 as EntityBase<Customer>, customer2 as EntityBase<Customer>);
        }

        [Test]
        public void Entity_Equals()
        {
            var customer1 = new Customer { CustomerId = 1 };
            var customer2 = new Customer { CustomerId = 1 };

            EqualityTests.TestEqualObjects(customer1 as EntityBase<Customer>, customer2 as EntityBase<Customer>);
        }

        [Test]
        public void Entity_AgainstNull()
        {
            var customer1 = new Customer { CustomerId = 1 };

            EqualityTests.TestAgainstNull(customer1 as EntityBase<Customer>);
        }

        [Test]
        public void Entity_SelfDescribed_Id()
        {
            var customer = new Customer { CustomerId = 1 };

            Assert.IsTrue(customer.IdName.Equals(customer.IdAccessor.Name));
        }
    }
}