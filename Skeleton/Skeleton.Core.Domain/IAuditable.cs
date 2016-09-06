using System;

namespace Skeleton.Core.Domain
{
    public interface IAuditable
    {
        string CreatedBy { get; set; }
        DateTime CreatedDateTime { get; set; }
        string LastModifiedBy { get; set; }
        DateTime? LastModifiedDateTime { get; set; }
    }
}