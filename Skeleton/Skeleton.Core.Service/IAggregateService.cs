﻿using System;
using Skeleton.Common;
using Skeleton.Core.Repository;

namespace Skeleton.Core.Service
{
    public interface IAggregateService :
        IDisposable,
        IHideObjectMethods
    {
        IRepositories Repositories { get; }
    }
}