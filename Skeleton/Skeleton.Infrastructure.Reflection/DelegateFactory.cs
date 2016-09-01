using System;
using System.Reflection;
using System.Reflection.Emit;

namespace Skeleton.Infrastructure.Reflection
{
    internal static class DelegateFactory
    {
        internal static ConstructorDelegate CreateConstructor(Type type, Type[] paramTypes)
        {
            type.ThrowIfNull(() => type);

            var dynamicMethod = CreateDynamicMethod(
                "Create" + type.FullName,
                typeof(object),
                new[] {typeof(object[])},
                type);

            var generator = dynamicMethod.GetILGenerator();

            if (type.IsValueType && (paramTypes.Length == 0))
            {
                generator.DeclareLocal(type);
                generator.Emit(OpCodes.Ldloc_0);
                generator.Emit(OpCodes.Box, type);
            }
            else
            {
                var constructorInfo = type.GetConstructor(paramTypes);

                if (constructorInfo == null)
                    return null;

                generator.PushParameters(paramTypes);
                generator.Emit(OpCodes.Newobj, constructorInfo);
            }

            generator.Return();

            return (ConstructorDelegate) dynamicMethod.CreateDelegate(typeof(ConstructorDelegate));
        }

        internal static GetterDelegate CreateGet(PropertyInfo propertyInfo)
        {
            propertyInfo.ThrowIfNull(() => propertyInfo);

            if (!propertyInfo.CanRead)
                return null;

            var methodInfo = propertyInfo.GetGetMethod(true);

            if (methodInfo == null)
                return null;

            var dynamicMethod = CreateDynamicMethod(
                "Get" + propertyInfo.Name,
                typeof(object),
                new[] {typeof(object)},
                propertyInfo.DeclaringType);

            var generator = dynamicMethod.GetILGenerator();

            if (!methodInfo.IsStatic)
                generator.PushInstance(propertyInfo.DeclaringType);

            generator.CallMethod(methodInfo);
            generator.BoxIfNeeded(propertyInfo.PropertyType);
            generator.Return();

            return (GetterDelegate) dynamicMethod.CreateDelegate(typeof(GetterDelegate));
        }

        internal static GetterDelegate CreateGet(FieldInfo fieldInfo)
        {
            fieldInfo.ThrowIfNull(() => fieldInfo);

            var dynamicMethod = CreateDynamicMethod(
                "Get" + fieldInfo.Name,
                typeof(object),
                new[] {typeof(object)},
                fieldInfo.DeclaringType);

            var generator = dynamicMethod.GetILGenerator();

            if (fieldInfo.IsStatic)
                generator.Emit(OpCodes.Ldsfld, fieldInfo);
            else
                generator.PushInstance(fieldInfo.DeclaringType);

            generator.Emit(OpCodes.Ldfld, fieldInfo);
            generator.BoxIfNeeded(fieldInfo.FieldType);
            generator.Return();

            return (GetterDelegate) dynamicMethod.CreateDelegate(typeof(GetterDelegate));
        }

        internal static MethodDelegate CreateMethod(MethodInfo methodInfo)
        {
            methodInfo.ThrowIfNull(() => methodInfo);

            var dynamicMethod = CreateDynamicMethod(
                "Dynamic" + methodInfo.Name,
                typeof(object),
                new[] {typeof(object), typeof(object[])},
                methodInfo.DeclaringType);

            var generator = dynamicMethod.GetILGenerator();
            var ps = methodInfo.GetParameters();

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

            if (!methodInfo.IsStatic)
                generator.Emit(OpCodes.Ldarg_0);

            for (var i = 0; i < paramTypes.Length; i++)
                generator.Emit(ps[i].ParameterType.IsByRef ? OpCodes.Ldloca_S : OpCodes.Ldloc, locals[i]);

            generator.EmitCall(methodInfo.IsStatic ? OpCodes.Call : OpCodes.Callvirt, methodInfo, null);

            if (methodInfo.ReturnType == typeof(void))
                generator.Emit(OpCodes.Ldnull);
            else
                generator.BoxIfNeeded(methodInfo.ReturnType);

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

            return (MethodDelegate) dynamicMethod.CreateDelegate(typeof(MethodDelegate));
        }

        internal static SetterDelegate CreateSet(PropertyInfo propertyInfo)
        {
            propertyInfo.ThrowIfNull(() => propertyInfo);

            if (!propertyInfo.CanWrite)
                return null;

            var methodInfo = propertyInfo.GetSetMethod(true);
            if (methodInfo == null)
                return null;

            var dynamicMethod = CreateDynamicMethod(
                "Set" + propertyInfo.Name,
                null,
                new[] {typeof(object), typeof(object)},
                propertyInfo.DeclaringType);

            var generator = dynamicMethod.GetILGenerator();

            if (!methodInfo.IsStatic)
                generator.PushInstance(propertyInfo.DeclaringType);

            generator.Emit(OpCodes.Ldarg_1);
            generator.UnboxIfNeeded(propertyInfo.PropertyType);
            generator.CallMethod(methodInfo);
            generator.Return();

            return (SetterDelegate) dynamicMethod.CreateDelegate(typeof(SetterDelegate));
        }

        internal static SetterDelegate CreateSet(FieldInfo fieldInfo)
        {
            fieldInfo.ThrowIfNull(() => fieldInfo);

            var dynamicMethod = CreateDynamicMethod(
                "Set" + fieldInfo.Name,
                null,
                new[] {typeof(object), typeof(object)},
                fieldInfo.DeclaringType);

            var generator = dynamicMethod.GetILGenerator();

            if (fieldInfo.IsStatic)
                generator.Emit(OpCodes.Ldsfld, fieldInfo);
            else
                generator.PushInstance(fieldInfo.DeclaringType);

            generator.Emit(OpCodes.Ldarg_1);
            generator.UnboxIfNeeded(fieldInfo.FieldType);
            generator.Emit(OpCodes.Stfld, fieldInfo);
            generator.Return();

            return (SetterDelegate) dynamicMethod.CreateDelegate(typeof(SetterDelegate));
        }

        private static DynamicMethod CreateDynamicMethod(string name, Type returnType, Type[] parameterTypes, Type owner)
        {
            return !owner.IsInterface
                ? new DynamicMethod(name, returnType, parameterTypes, owner, true)
                : new DynamicMethod(name, returnType, parameterTypes, owner.Assembly.ManifestModule, true);
        }
    }
}