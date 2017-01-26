﻿using System.Diagnostics;
using System.Reflection;
using Skeleton.Abstraction.Reflection;
using Skeleton.Common;
using Skeleton.Core.Reflection.Emitter;
using System;
using System.Globalization;

namespace Skeleton.Core.Reflection
{
    [DebuggerDisplay("Name: {Name}")]
    public sealed class MethodAccessor : IMethodAccessor
    {
        private readonly Func<object, object[], object> _methodDelegate;

        public MethodAccessor(MethodInfo methodInfo)
        {
            methodInfo.ThrowIfNull(() => methodInfo);

            MethodInfo = methodInfo;
            Name = methodInfo.Name;

            var emitter = new MethodEmitter(methodInfo);
            _methodDelegate = emitter.CreateDelegate();
        }

        public MethodInfo MethodInfo { get; }

        public string Name { get; }

        public Func<object, object[], object> Invoker => _methodDelegate;

        internal static IMethodAccessor Create(Type type, string name, Type[] parameterTypes)
        {
            type.ThrowIfNull(() => type);
            name.ThrowIfNullOrEmpty(() => name);
           
            const BindingFlags flags = BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance;

            var methodInfo = type.GetMethod(name,
                                 flags,
                                 null,
                                 CallingConventions.Any,
                                 parameterTypes,
                                 null) ?? type.GetMethod(name);

            if (methodInfo == null)
                throw new InvalidOperationException(
                    string.Format(CultureInfo.CurrentCulture, "Method '{0}' was not found in type {1}.", name, type));

            return new MethodAccessor(methodInfo);
        }
    }
}