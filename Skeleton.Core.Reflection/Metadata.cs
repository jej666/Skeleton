using Skeleton.Abstraction.Reflection;
using Skeleton.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Skeleton.Core.Reflection
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1724:TypeNamesShouldNotMatchNamespaces")]
    [DebuggerDisplay("Type = {Type}")]
    public sealed class Metadata : HideObjectMethodsBase, IMetadata
    {
        private const BindingFlags DeclaredOnlyFlags =
            BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance;

        private const BindingFlags DefaultFlags =
            BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static;

        private readonly LazyRef<Dictionary<string, IMemberAccessor>> _fieldCache =
            new LazyRef<Dictionary<string, IMemberAccessor>>(() =>
                    new Dictionary<string, IMemberAccessor>());

        private readonly LazyRef<Dictionary<int, IMethodAccessor>> _methodCache =
            new LazyRef<Dictionary<int, IMethodAccessor>>(() =>
                    new Dictionary<int, IMethodAccessor>());

        private readonly LazyRef<Dictionary<string, IMemberAccessor>> _propertyCache =
            new LazyRef<Dictionary<string, IMemberAccessor>>(() =>
                    new Dictionary<string, IMemberAccessor>());

        private readonly LazyRef<Dictionary<int, IInstanceAccessor>> _constructorCache =
            new LazyRef<Dictionary<int, IInstanceAccessor>>(() =>
                    new Dictionary<int, IInstanceAccessor>());

        public Metadata(Type type)
        {
            type.ThrowIfNull(nameof(type));

            Type = type;
        }

        public Type Type { get; }

        public IInstanceAccessor GetConstructor()
        {
            return GetConstructor(Type.EmptyTypes);
        }

        public IInstanceAccessor GetConstructor(Type[] parameterTypes)
        {
            var key = GetConstructorKey(parameterTypes);

            return _constructorCache.Value.GetOrAdd(
                key, () => ConstructorAccessor.Create(Type, parameterTypes));
        }

        public IEnumerable<IMemberAccessor> GetDeclaredOnlyFields()
        {
            return GetFields(DeclaredOnlyFlags);
        }

        public IEnumerable<IMemberAccessor> GetDeclaredOnlyProperties()
        {
            return GetProperties(DeclaredOnlyFlags);
        }

        public IMemberAccessor GetField(string name)
        {
            return GetField(name, DefaultFlags);
        }

        public IEnumerable<IMemberAccessor> GetFields()
        {
            return GetFields(DefaultFlags);
        }

        public IMethodAccessor GetMethod(string name)
        {
            return GetMethod(name, Type.EmptyTypes);
        }

        public IMethodAccessor GetMethod(string name, Type[] parameterTypes)
        {
            var key = GetMethodKey(name, parameterTypes);

            return _methodCache.Value.GetOrAdd(
                key, () => MethodAccessor.Create(Type, name, parameterTypes));
        }

        public IEnumerable<IMemberAccessor> GetProperties()
        {
            return GetProperties(DefaultFlags);
        }

        public IMemberAccessor GetProperty(string name)
        {
            name.ThrowIfNullOrEmpty(nameof(name));

            IMemberAccessor accessor;
            if (_propertyCache.Value.TryGetValue(name, out accessor))
                return accessor;

            var propertyInfo = Type.GetProperty(name);
            var value = PropertyAccessor.Create(propertyInfo);

            _propertyCache.Value.Add(name, value);

            return value;
        }

        public IMemberAccessor GetProperty<T>(Expression<Func<T, object>> expression)
        {
            expression.ThrowIfNull(nameof(expression));

            var propertyInfo = expression.GetPropertyAccess();

            IMemberAccessor accessor;
            if (_propertyCache.Value.TryGetValue(propertyInfo.Name, out accessor))
                return accessor;

            var value = PropertyAccessor.Create(propertyInfo);

            _propertyCache.Value.Add(propertyInfo.Name, value);

            return value;
        }

        private IMemberAccessor GetField(string name, BindingFlags bindings)
        {
            name.ThrowIfNullOrEmpty(nameof(name));

            IMemberAccessor accessor;
            if (_fieldCache.Value.TryGetValue(name, out accessor))
                return accessor;

            var fieldInfo = Type.GetField(name, bindings);
            var value = FieldAccessor.Create(fieldInfo);

            _fieldCache.Value.Add(name, value);

            return value;
        }

        private IEnumerable<IMemberAccessor> GetFields(BindingFlags bindings)
        {
            var fields = Type.GetFields(bindings);
            foreach (var fieldInfo in fields)
                yield return _fieldCache.Value.GetOrAdd(
                    fieldInfo.Name,
                    () => FieldAccessor.Create(fieldInfo));
        }

        private IEnumerable<IMemberAccessor> GetProperties(BindingFlags bindings)
        {
            var properties = Type.GetProperties(bindings);
            foreach (var propertyInfo in properties)
                yield return _propertyCache.Value.GetOrAdd(
                    propertyInfo.Name,
                    () => PropertyAccessor.Create(propertyInfo));
        }

        private static int GetMethodKey(string name, IEnumerable<Type> parameterTypes)
        {
            unchecked
            {
                var result = name?.GetHashCode() ?? 0;
                result = parameterTypes.Aggregate(result,
                    (r, p) => (r * 397) ^ (p != null ? p.GetHashCode() : 0));

                return result;
            }
        }

        private int GetConstructorKey(IEnumerable<Type> parameterTypes)
        {
            unchecked
            {
                var result = Type.GetHashCode();
                result = parameterTypes.Aggregate(result,
                    (r, p) => (r * 397) ^ (p != null ? p.GetHashCode() : 0));

                return result;
            }
        }
    }
}