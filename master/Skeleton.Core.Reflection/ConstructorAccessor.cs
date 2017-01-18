using Skeleton.Abstraction.Reflection;
using Skeleton.Common;
using Skeleton.Core.Reflection.Emitter;
using System;

namespace Skeleton.Core.Reflection
{
    public sealed class ConstructorAccessor : IConstructorAccessor
    {
        private readonly LazyRef<ConstructorDelegate> _constructorDelegate;

        public ConstructorAccessor(Type type, Type[] paramTypes)
        {
            type.ThrowIfNull(() => type);
            paramTypes.ThrowIfNull(() => paramTypes);

            var emitter = new ConstructorEmitter(type, paramTypes);
            _constructorDelegate = new LazyRef<ConstructorDelegate>(
                ()=>(ConstructorDelegate)emitter.CreateDelegate());
        }

        public object Invoke(params object[] arguments)
        {
            arguments.ThrowIfNull(() => arguments);

            return _constructorDelegate.Value?.Invoke(arguments);
        }

        public static IConstructorAccessor Create(Type type, Type[] parameterTypes)
        {
            return new ConstructorAccessor(type, parameterTypes);
        }
    }
}