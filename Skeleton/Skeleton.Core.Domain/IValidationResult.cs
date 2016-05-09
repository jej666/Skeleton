namespace Skeleton.Core.Domain
{
    using System.Collections.Generic;

    public interface IValidationResult
    {
        IEnumerable<string> BrokenRules { get; }
        bool IsValid { get; }
    }
}