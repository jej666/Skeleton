using System;

namespace Skeleton.Infrastructure.Dependency.Configuration
{
    public sealed class CreationStackTrackerPolicy : ICreationStackTrackerPolicy
    {
        public PeekableCollection<Type> TypeStack { get; } = new PeekableCollection<Type>();
    }
}