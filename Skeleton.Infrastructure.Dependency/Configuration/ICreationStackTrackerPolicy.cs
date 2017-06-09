using Microsoft.Practices.ObjectBuilder2;
using System;

namespace Skeleton.Infrastructure.Dependency.Configuration
{
    public interface ICreationStackTrackerPolicy : IBuilderPolicy
    {
        PeekableCollection<Type> TypeStack { get; }
    }
}