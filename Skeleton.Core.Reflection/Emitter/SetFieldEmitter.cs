using System;
using System.Reflection;
using System.Reflection.Emit;

namespace Skeleton.Core.Reflection.Emitter
{
    internal sealed class SetFieldEmitter : EmitterBase
    {
        private readonly FieldInfo _fieldInfo;

        internal SetFieldEmitter(FieldInfo fieldInfo)
            : base(fieldInfo.DeclaringType)
        {
            _fieldInfo = fieldInfo;
        }

        internal override Delegate CreateDelegate()
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

            return dynamicMethod.CreateDelegate(typeof(SetterDelegate));
        }
    }
}