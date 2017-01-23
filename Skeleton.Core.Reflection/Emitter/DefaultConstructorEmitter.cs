using System;
using System.Reflection.Emit;

namespace Skeleton.Core.Reflection.Emitter
{
    internal sealed class DefaultConstructorEmitter : EmitterBase
    {
        internal DefaultConstructorEmitter(Type type)
            : base(type)
        {
        }

        internal Func<object> CreateDelegate()
        {
            var dynamicMethod = CreateDynamicMethod(
                "Create" + Owner.FullName,
                typeof(object),
                new[] { typeof(object[]) });

            var generator = dynamicMethod.GetILGenerator();
            var constructorInfo = Owner.GetConstructor(Type.EmptyTypes);

            if (constructorInfo == null)
                return null;

            generator.Emit(OpCodes.Newobj, constructorInfo);
            generator.Return();

            return (Func<object>)dynamicMethod.CreateDelegate(typeof(Func<object>));
        }
    }
}
