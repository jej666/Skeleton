using System.Collections.Generic;

namespace Skeleton.Shared.Abstraction
{
    public interface IValidationResult : IHideObjectMethods
    {
        IEnumerable<string> BrokenRules { get; }

        bool IsValid { get; }
    }
}