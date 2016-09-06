using System;
using Microsoft.Practices.ObjectBuilder2;

namespace Skeleton.Infrastructure.DependencyResolver.LoggerExtension
{
    public interface ICreationStackTrackerPolicy : IBuilderPolicy
    {
        PeekableCollection<Type> TypeStack { get; }
    }
}