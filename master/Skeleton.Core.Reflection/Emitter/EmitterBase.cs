﻿using System;
using System.Reflection.Emit;

namespace Skeleton.Core.Reflection.Emitter
{
    internal abstract class EmitterBase
    {
        protected internal EmitterBase(Type type)
        {
            Owner = type;
        }

        protected internal Type Owner
        {
            get;
        }

        internal abstract Delegate CreateDelegate();

        internal DynamicMethod CreateDynamicMethod(string name, Type returnType, Type[] parameterTypes)
        {
            return !Owner.IsInterface
                ? new DynamicMethod(name, returnType, parameterTypes, Owner, true)
                : new DynamicMethod(name, returnType, parameterTypes, Owner.Assembly.ManifestModule, true);
        }
    }
}
