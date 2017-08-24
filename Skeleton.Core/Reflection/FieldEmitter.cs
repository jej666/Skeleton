using System;
using System.Reflection;
using System.Reflection.Emit;

namespace Skeleton.Core.Reflection
{
    internal sealed class FieldEmitter : EmitterBase
    {
        private readonly FieldInfo _fieldInfo;

        internal FieldEmitter(FieldInfo fieldInfo)
            : base(fieldInfo.DeclaringType)
        {
            _fieldInfo = fieldInfo;
        }

        internal Func<object, object> CreateGetter()
        {
            var dynamicMethod = CreateDynamicMethod(
                "Get" + _fieldInfo.Name,
                typeof(object),
                new[] { typeof(object) });

            var generator = dynamicMethod.GetILGenerator();

            if (_fieldInfo.IsStatic)
                generator.Emit(OpCodes.Ldsfld, _fieldInfo);
            else
                generator.PushInstance(Owner);

            generator.Emit(OpCodes.Ldfld, _fieldInfo);
            generator.BoxIfNeeded(_fieldInfo.FieldType);
            generator.Return();

            return (Func<object, object>)dynamicMethod.CreateDelegate(typeof(Func<object, object>));
        }

        internal Action<object, object> CreateSetter()
        {
            var dynamicMethod = CreateDynamicMethod(
                "Set" + _fieldInfo.Name,
                null,
                new[] { typeof(object), typeof(object) });

            var generator = dynamicMethod.GetILGenerator();

            if (_fieldInfo.IsStatic)
                generator.Emit(OpCodes.Ldsfld, _fieldInfo);
            else
                generator.PushInstance(_fieldInfo.DeclaringType);

            generator.Emit(OpCodes.Ldarg_1);
            generator.UnboxIfNeeded(_fieldInfo.FieldType);
            generator.Emit(OpCodes.Stfld, _fieldInfo);
            generator.Return();

            return (Action<object, object>)dynamicMethod.CreateDelegate(typeof(Action<object, object>));
        }
    }
}