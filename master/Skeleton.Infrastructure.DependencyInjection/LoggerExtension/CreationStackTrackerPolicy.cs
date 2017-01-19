using System;

namespace Skeleton.Infrastructure.DependencyInjection.LoggerExtension
{
    public sealed class CreationStackTrackerPolicy : ICreationStackTrackerPolicy
    {
        public PeekableCollection<Type> TypeStack { get; } = new PeekableCollection<Type>();
    }
}