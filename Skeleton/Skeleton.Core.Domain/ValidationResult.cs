using System.Collections.Generic;
using System.Linq;
using Skeleton.Common.Extensions;

namespace Skeleton.Core.Domain
{
    // make return type a rule
    public class ValidationResult : IValidationResult
    {
        private readonly IEnumerable<string> _brokenRules;

        public ValidationResult(IEnumerable<string> brokenRules)
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