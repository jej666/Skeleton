using System.Collections.Generic;

namespace Skeleton.Abstraction
{
    public interface IValidationResult
    {
        IEnumerable<string> BrokenRules { get; }
        bool IsValid { get; }
    }
}