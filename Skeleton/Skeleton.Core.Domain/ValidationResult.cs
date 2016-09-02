using System;
using System.Collections.Generic;
using System.Linq;

namespace Skeleton.Core.Domain
{
    // make return type a rule
    public class EntityValidationResult : IEntityValidationResult
    {
        private readonly IEnumerable<string> _brokenRules;

        public EntityValidationResult(IEnumerable<string> brokenRules)
        {
            var enumerable = brokenRules as IList<string> ?? brokenRules.ToList();
            enumerable.ThrowIfNull(() => enumerable);

            _brokenRules = enumerable;
        }

        public IEnumerable<string> BrokenRules
        {
            get { return _brokenRules; }
        }

        public bool IsValid
        {
            get { return BrokenRules.IsNullOrEmpty(); }
        }
    }
}