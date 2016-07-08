using System.Collections.Generic;

namespace Skeleton.Abstraction
{
    public interface IValidationResult : IHideObjectMethods
    {
        IEnumerable<string> BrokenRules { get; }

        bool IsValid { get; }
    }
}