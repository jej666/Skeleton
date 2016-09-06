using System.Collections.Generic;

namespace Skeleton.Abstraction
{
    public interface IEntityValidationResult : IHideObjectMethods
    {
        IEnumerable<string> BrokenRules { get; }

        bool IsValid { get; }
    }
}