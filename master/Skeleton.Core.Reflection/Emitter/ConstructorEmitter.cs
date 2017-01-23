using System;
using System.Reflection.Emit;

namespace Skeleton.Core.Reflection.Emitter
{
    internal sealed class ConstructorEmitter : EmitterBase
    {
        private readonly Type[] _parameterTypes;

        internal ConstructorEmitter(Type type, Type[] parameterTypes)
            : base(type)
        {
            _parameterTypes = parameterTypes;
        }

        internal Func<object[], object> CreateDelegate()
        {
            var dynamicMethod = CreateDynamicMethod(
                "Create" + Owner.FullName,
                typeof(object),
                new[] { typeof(object[]) });

            var generator = dynamicMethod.GetILGenerator();

            if (Owner.IsValueType && (_parameterTypes.Length == 0))
            {
                generator.DeclareLocal(Owner);
                generator.Emit(OpCodes.Ldloc_0);
                generator.Emit(OpCodes.Box, Owner);
            }
            else
            {
                var constructorInfo = Owner.GetConstructor(_parameterTypes);

                if (constructorInfo == null)
                    return null;

                generator.PushParameters(_parameterTypes);
                generator.Emit(OpCodes.Newobj, constructorInfo);
            }

            generator.Return();

            return (Func<object[], object>)dynamicMethod.CreateDelegate(typeof(Func<object[], object>));
        }
    }
}
