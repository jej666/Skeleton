using Skeleton.Core.Domain;
using System;
using System.Runtime.Serialization;

namespace Skeleton.Tests.Infrastructure
{
    public class Customer : Entity<Customer, int>
    {
        // Need an empty ctor
        public Customer()
            : base(pk => pk.CustomerId)
        {
        }

        public int CustomerId { get; set; }
       
        public int CustomerCategoryId { get; set; }
       
        public string Name { get; set; }
    }

    public class CustomerDto
    { 
        public int CustomerId { get; set; }

        public int CustomerCategoryId { get; set; }

        public string Name { get; set; }
    }
}