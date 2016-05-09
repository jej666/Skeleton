namespace Skeleton.Core.Domain
{
    using System;

    public interface IAuditable
    {
        string CreatedBy { get; }
        DateTime CreatedDateTime { get; }
        string LastModifiedBy { get; }
        DateTime? LastModifiedDateTime { get; }
    }
}