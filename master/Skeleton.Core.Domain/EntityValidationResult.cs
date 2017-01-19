﻿using Skeleton.Abstraction.Domain;
using Skeleton.Common;
using System.Collections.Generic;
using System.Linq;

namespace Skeleton.Core.Domain
{
    public sealed class EntityValidationResult : IEntityValidationResult
    {
        public EntityValidationResult(IEnumerable<string> brokenRules)
        {
            var enumerable = brokenRules as IList<string> ?? brokenRules.ToList();
            enumerable.ThrowIfNull(() => enumerable);

            BrokenRules = enumerable;
        }

        public IEnumerable<string> BrokenRules { get; }

        public bool IsValid => BrokenRules.IsNullOrEmpty();
    }
}