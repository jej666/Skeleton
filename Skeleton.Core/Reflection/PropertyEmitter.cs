using System;
using System.Reflection;
using System.Reflection.Emit;

namespace Skeleton.Core.Reflection
{
    internal sealed class PropertyEmitter : EmitterBase
    {
        private readonly PropertyInfo _propertyInfo;

        internal PropertyEmitter(PropertyInfo propertyInfo)
            : base(propertyInfo.DeclaringType)
        {
            _propertyInfo = propertyInfo;
        }

        internal Func<object, object> CreateGetter()
        {
            if (!_propertyInfo.CanRead)
                return null;

            var methodInfo = _propertyInfo.GetGetMethod(true);

            if (methodInfo == null)
                return null;

            var dynamicMethod = CreateDynamicMethod(
                "Get" + _propertyInfo.Name,
                typeof(object),
                new[] { typeof(object) });

            var generator = dynamicMethod.GetILGenerator();

            if (!methodInfo.IsStatic)
                generator.PushInstance(Owner);

            generator.CallMethod(methodInfo);
            generator.BoxIfNeeded(_propertyInfo.PropertyType);
            generator.Return();

            return (Func<object, object>)dynamicMethod.CreateDelegate(typeof(Func<object, object>));
        }

        internal Action<object, object> CreateSetter()
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

            return (Action<object, object>)dynamicMethod.CreateDelegate(typeof(Action<object, object>));
        }
    }
}