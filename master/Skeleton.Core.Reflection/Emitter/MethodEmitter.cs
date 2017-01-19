using System;
using System.Reflection;
using System.Reflection.Emit;

namespace Skeleton.Core.Reflection.Emitter
{
    internal sealed class MethodEmitter : EmitterBase
    {
        private readonly MethodInfo _methodInfo;

        internal MethodEmitter(MethodInfo methodInfo)
            : base(methodInfo.DeclaringType)
        {
            _methodInfo = methodInfo;
        }

        internal override Delegate CreateDelegate()
        {
            var dynamicMethod = CreateDynamicMethod(
                "Dynamic" + _methodInfo.Name,
                typeof(object),
                new[] { typeof(object), typeof(object[]) });

            var generator = dynamicMethod.GetILGenerator();
            var ps = _methodInfo.GetParameters();

            var paramTypes = new Type[ps.Length];
            for (var i = 0; i < paramTypes.Length; i++)
                if (ps[i].ParameterType.IsByRef)
                    paramTypes[i] = ps[i].ParameterType.GetElementType();
                else
                    paramTypes[i] = ps[i].ParameterType;

            var locals = new LocalBuilder[paramTypes.Length];
            for (var i = 0; i < paramTypes.Length; i++)
                locals[i] = generator.DeclareLocal(paramTypes[i], true);

            for (var i = 0; i < paramTypes.Length; i++)
            {
                generator.Emit(OpCodes.Ldarg_1);
                generator.FastInt(i);
                generator.Emit(OpCodes.Ldelem_Ref);
                generator.UnboxIfNeeded(paramTypes[i]);
                generator.Emit(OpCodes.Stloc, locals[i]);
            }

            if (!_methodInfo.IsStatic)
                generator.Emit(OpCodes.Ldarg_0);

            for (var i = 0; i < paramTypes.Length; i++)
                generator.Emit(ps[i].ParameterType.IsByRef ? OpCodes.Ldloca_S : OpCodes.Ldloc, locals[i]);

            generator.EmitCall(_methodInfo.IsStatic ? OpCodes.Call : OpCodes.Callvirt, _methodInfo, null);

            if (_methodInfo.ReturnType == typeof(void))
                generator.Emit(OpCodes.Ldnull);
            else
                generator.BoxIfNeeded(_methodInfo.ReturnType);

            for (var i = 0; i < paramTypes.Length; i++)
            {
                if (!ps[i].ParameterType.IsByRef)
                    continue;

                generator.Emit(OpCodes.Ldarg_1);
                generator.FastInt(i);
                generator.Emit(OpCodes.Ldloc, locals[i]);
                if (locals[i].LocalType.IsValueType)
                    generator.Emit(OpCodes.Box, locals[i].LocalType);
                generator.Emit(OpCodes.Stelem_Ref);
            }

            generator.Emit(OpCodes.Ret);

            return dynamicMethod.CreateDelegate(typeof(MethodDelegate));
        }
    }
}
