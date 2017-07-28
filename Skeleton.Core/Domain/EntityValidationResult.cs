using Skeleton.Abstraction.Domain;
using System.Collections.Generic;

namespace Skeleton.Core.Domain
{
    public sealed class EntityValidationResult : IEntityValidationResult
    {
        public EntityValidationResult(IEnumerable<IValidationRule> brokenRules)
        {
            var enumerable = brokenRules.AsList();
            enumerable.ThrowIfNull(nameof(enumerable));

            BrokenRules = enumerable;
        }

        public IEnumerable<IValidationRule> BrokenRules { get; }

        public bool IsValid => BrokenRules.IsNullOrEmpty();
    }
}