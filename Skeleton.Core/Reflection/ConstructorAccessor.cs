﻿using Skeleton.Abstraction.Reflection;
using System;

namespace Skeleton.Core.Reflection
{
    public sealed class ConstructorAccessor : IInstanceAccessor
    {
        private readonly Func<object[], object> _constructorDelegate;

        public ConstructorAccessor(Type type, Type[] paramTypes)
        {
            type.ThrowIfNull();
            paramTypes.ThrowIfNull();

            var emitter = new ConstructorEmitter(type, paramTypes);
            _constructorDelegate = emitter.CreateDelegate();
        }

        public Func<object[], object> InstanceCreator => _constructorDelegate;

        internal static IInstanceAccessor Create(Type type, Type[] parameterTypes)
        {
            return new ConstructorAccessor(type, parameterTypes);
        }
    }
}