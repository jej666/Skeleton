using Skeleton.Abstraction.Domain;

namespace Skeleton.Core.Domain
{
    public class ValidationRule : IValidationRule
    {
        public ValidationRule(string ruleDescription)
        {
            RuleDescription = ruleDescription;
        }

        public string RuleDescription { get; private set; }
    }
}