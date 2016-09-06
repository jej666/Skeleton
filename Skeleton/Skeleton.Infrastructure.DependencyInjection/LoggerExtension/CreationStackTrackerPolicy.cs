using System;

namespace Skeleton.Infrastructure.DependencyInjection.LoggerExtension
{
    public class CreationStackTrackerPolicy : ICreationStackTrackerPolicy
    {
        public PeekableCollection<Type> TypeStack { get; } = new PeekableCollection<Type>();
    }
}