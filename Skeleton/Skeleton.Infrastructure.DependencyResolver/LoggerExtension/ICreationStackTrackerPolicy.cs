namespace Skeleton.Infrastructure.DependencyResolver.LoggerExtension
{
    using Microsoft.Practices.ObjectBuilder2;
    using System;

    public interface ICreationStackTrackerPolicy : IBuilderPolicy
    {
        PeekableCollection<Type> TypeStack { get; }
    }
}