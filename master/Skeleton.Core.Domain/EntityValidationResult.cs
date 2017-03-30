using Skeleton.Abstraction.Domain;
using Skeleton.Common;
using System.Collections.Generic;

namespace Skeleton.Core.Domain
{
    public sealed class EntityValidationResult : IEntityValidationResult
    {
        public EntityValidationResult(IEnumerable<string> brokenRules)
        {
            var enumerable = brokenRules.AsList();
            enumerable.ThrowIfNull(nameof(enumerable));

            BrokenRules = enumerable;
        }

        public IEnumerable<string> BrokenRules { get; }

        public bool IsValid => BrokenRules.IsNullOrEmpty();
    }
}