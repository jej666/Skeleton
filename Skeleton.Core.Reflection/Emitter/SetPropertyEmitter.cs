using System;
using System.Reflection;
using System.Reflection.Emit;

namespace Skeleton.Core.Reflection.Emitter
{
    internal class SetPropertyEmitter : EmitterBase
    {
        private readonly PropertyInfo _propertyInfo;

        internal SetPropertyEmitter(PropertyInfo propertyInfo)
            : base(propertyInfo.DeclaringType)
        {
            _propertyInfo = propertyInfo;
        }

        internal override Delegate CreateDelegate()
        {
            if (!_propertyInfo.CanWrite)
                return null;

            var methodInfo = _propertyInfo.GetSetMethod(true);
            if (methodInfo == null)
                return null;

            var dynamicMethod = CreateDynamicMethod(
                "Set" + _propertyInfo.Name,
                null,
                new[] { typeof(object), typeof(object) });

            var generator = dynamicMethod.GetILGenerator();

            if (!methodInfo.IsStatic)
                generator.PushInstance(_propertyInfo.DeclaringType);

            generator.Emit(OpCodes.Ldarg_1);
            generator.UnboxIfNeeded(_propertyInfo.PropertyType);
            generator.CallMethod(methodInfo);
            generator.Return();

            return dynamicMethod.CreateDelegate(typeof(SetterDelegate));
        }
    }
}
