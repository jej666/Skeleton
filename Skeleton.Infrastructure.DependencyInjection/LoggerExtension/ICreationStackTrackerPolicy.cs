using System;
using Microsoft.Practices.ObjectBuilder2;

namespace Skeleton.Infrastructure.DependencyInjection.LoggerExtension
{
    public interface ICreationStackTrackerPolicy : IBuilderPolicy
    {
        PeekableCollection<Type> TypeStack { get; }
    }
}