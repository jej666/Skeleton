using System;
using System.Reflection;
using System.Reflection.Emit;
using Skeleton.Common;

namespace Skeleton.Core.Reflection.Emitter
{
    internal static class EmitExtensions
    {
        internal static void BoxIfNeeded(this ILGenerator generator, Type type)
        {
            generator.ThrowIfNull(() => generator);
            type.ThrowIfNull(() => type);

            generator.Emit(type.IsValueType ? OpCodes.Box : OpCodes.Castclass, type);
        }

        internal static void CallMethod(this ILGenerator generator, MethodInfo methodInfo)
        {
            generator.ThrowIfNull(() => generator);
            methodInfo.ThrowIfNull(() => methodInfo);

            if (methodInfo.IsFinal || !methodInfo.IsVirtual)
                generator.Emit(OpCodes.Call, methodInfo);
            else
                generator.Emit(OpCodes.Callvirt, methodInfo);
        }

        internal static void FastInt(this ILGenerator generator, int value)
        {
            generator.ThrowIfNull(() => generator);

            switch (value)
            {
                case -1:
                    generator.Emit(OpCodes.Ldc_I4_M1);
                    return;

                case 0:
                    generator.Emit(OpCodes.Ldc_I4_0);
                    return;

                case 1:
                    generator.Emit(OpCodes.Ldc_I4_1);
                    return;

                case 2:
                    generator.Emit(OpCodes.Ldc_I4_2);
                    return;

                case 3:
                    generator.Emit(OpCodes.Ldc_I4_3);
                    return;

                case 4:
                    generator.Emit(OpCodes.Ldc_I4_4);
                    return;

                case 5:
                    generator.Emit(OpCodes.Ldc_I4_5);
                    return;

                case 6:
                    generator.Emit(OpCodes.Ldc_I4_6);
                    return;

                case 7:
                    generator.Emit(OpCodes.Ldc_I4_7);
                    return;

                case 8:
                    generator.Emit(OpCodes.Ldc_I4_8);
                    return;
            }

            if ((value > -129) && (value < 128))
                generator.Emit(OpCodes.Ldc_I4_S, (sbyte) value);
            else
                generator.Emit(OpCodes.Ldc_I4, value);
        }

        internal static void PushInstance(this ILGenerator generator, Type type)
        {
            generator.ThrowIfNull(() => generator);
            type.ThrowIfNull(() => type);

            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(type.IsValueType ? OpCodes.Unbox : OpCodes.Castclass, type);
        }

        internal static void PushParameters(this ILGenerator generator, Type[] parameterTypes)
        {
            generator.ThrowIfNull(() => generator);
            parameterTypes.ThrowIfNull(() => parameterTypes);

            for (int i = 0; i < parameterTypes.Length; i++)
            {
                generator.Emit(OpCodes.Ldarg_0); // push args array
                generator.FastInt(i); // push index
                generator.Emit(OpCodes.Ldelem_Ref); // push array[index]
                generator.UnboxIfNeeded(parameterTypes[i]); // cast
            }
        }

        internal static void Return(this ILGenerator generator)
        {
            generator.ThrowIfNull(() => generator);

            generator.Emit(OpCodes.Ret);
        }

        internal static void UnboxIfNeeded(this ILGenerator generator, Type type)
        {
            generator.ThrowIfNull(() => generator);
            type.ThrowIfNull(() => type);

            generator.Emit(type.IsValueType ? OpCodes.Unbox_Any : OpCodes.Castclass, type);
        }
    }
}