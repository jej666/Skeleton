using System;

namespace Skeleton.Core.Domain
{
    public interface IAuditable
    {
        string CreatedBy { get; }
        DateTime CreatedDateTime { get; }
        string LastModifiedBy { get; }
        DateTime? LastModifiedDateTime { get; }
    }
}