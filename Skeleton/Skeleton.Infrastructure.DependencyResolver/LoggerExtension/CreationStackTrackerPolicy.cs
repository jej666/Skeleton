using System;

namespace Skeleton.Infrastructure.DependencyResolver.LoggerExtension
{
    public class CreationStackTrackerPolicy : ICreationStackTrackerPolicy
    {
        public PeekableCollection<Type> TypeStack { get; } = new PeekableCollection<Type>();
    }
}