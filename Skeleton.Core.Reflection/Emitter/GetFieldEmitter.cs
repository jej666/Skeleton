using System;
using System.Reflection;
using System.Reflection.Emit;

namespace Skeleton.Core.Reflection.Emitter
{
    internal sealed class GetFieldEmitter: EmitterBase
    {
        private readonly FieldInfo _fieldInfo;

        internal GetFieldEmitter(FieldInfo fieldInfo)
            :base(fieldInfo.DeclaringType)
        {
            _fieldInfo = fieldInfo;
        }

        internal override Delegate CreateDelegate()
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

            return dynamicMethod.CreateDelegate(typeof(GetterDelegate));
        }
    }
}
