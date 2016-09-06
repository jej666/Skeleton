﻿using System;

namespace Skeleton.Abstraction
{
    public interface IEntityAuditable
    {
        string CreatedBy { get; set; }
        DateTime CreatedDateTime { get; set; }
        string LastModifiedBy { get; set; }
        DateTime? LastModifiedDateTime { get; set; }
    }
}