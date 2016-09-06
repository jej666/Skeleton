using System.Collections.Generic;

namespace Skeleton.Core.Domain
{
    public interface IValidationResult
    {
        IEnumerable<string> BrokenRules { get; }
        bool IsValid { get; }
    }
}