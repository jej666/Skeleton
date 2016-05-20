using System;

namespace Skeleton.Infrastructure.DependencyResolver.LoggerExtension
{
    public class CreationStackTrackerPolicy : ICreationStackTrackerPolicy
    {
        private readonly PeekableCollection<Type> _typeStack = new PeekableCollection<Type>();

        public PeekableCollection<Type> TypeStack
        {
            get { return _typeStack; }
        }
    }
}