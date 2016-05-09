namespace Skeleton.Core.Domain
{
    using Common.Extensions;
    using System.Collections.Generic;

    // make return type a rule
    public class ValidationResult : IValidationResult
    {
        private readonly IEnumerable<string> _brokenRules;

        public ValidationResult(IEnumerable<string> brokenRules)
        {
            brokenRules.ThrowIfNull(() => brokenRules);

            _brokenRules = brokenRules;
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