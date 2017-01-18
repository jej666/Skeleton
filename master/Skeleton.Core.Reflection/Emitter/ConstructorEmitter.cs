using System;
using System.Reflection.Emit;

namespace Skeleton.Core.Reflection.Emitter
{
    internal class ConstructorEmitter : EmitterBase
    {
        private readonly Type[] _paramTypes;

        internal ConstructorEmitter(Type type, Type[] paramTypes)
            : base(type)
        {
            _paramTypes = paramTypes;
        }

        internal override Delegate CreateDelegate()
        {
            var dynamicMethod = CreateDynamicMethod(
                "Create" + Owner.FullName,
                typeof(object),
                new[] { typeof(object[]) });

            var generator = dynamicMethod.GetILGenerator();

            if (Owner.IsValueType && (_paramTypes.Length == 0))
            {
                generator.DeclareLocal(Owner);
                generator.Emit(OpCodes.Ldloc_0);
                generator.Emit(OpCodes.Box, Owner);
            }
            else
            {
                var constructorInfo = Owner.GetConstructor(_paramTypes);

                if (constructorInfo == null)
                    return null;

                generator.PushParameters(_paramTypes);
                generator.Emit(OpCodes.Newobj, constructorInfo);
            }

            generator.Return();

            return dynamicMethod.CreateDelegate(typeof(ConstructorDelegate));
        }
    }
}
