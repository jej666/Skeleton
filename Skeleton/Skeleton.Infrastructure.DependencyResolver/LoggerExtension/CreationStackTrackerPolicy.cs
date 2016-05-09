namespace Skeleton.Infrastructure.DependencyResolver.LoggerExtension
{
    using System;

    public class CreationStackTrackerPolicy : ICreationStackTrackerPolicy
    {
        private readonly PeekableCollection<Type> typeStack = new PeekableCollection<Type>();

        public PeekableCollection<Type> TypeStack
        {
            get { return typeStack; }
        }
    }
}