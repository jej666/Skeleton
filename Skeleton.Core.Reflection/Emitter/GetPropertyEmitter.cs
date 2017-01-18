using System;
using System.Reflection;

namespace Skeleton.Core.Reflection.Emitter
{
    internal class GetPropertyEmitter: EmitterBase
    {
        private readonly PropertyInfo _propertyInfo;

        internal GetPropertyEmitter(PropertyInfo propertyInfo)
            :base(propertyInfo.DeclaringType)
        {
            _propertyInfo = propertyInfo;
        }

        internal override Delegate CreateDelegate()
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

            return dynamicMethod.CreateDelegate(typeof(GetterDelegate));
        }
    }
}
