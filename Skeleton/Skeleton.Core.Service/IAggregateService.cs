using System;
using Skeleton.Abstraction;
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