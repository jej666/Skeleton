using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;
using Skeleton.Common.Extensions;

namespace Skeleton.Common.Reflection
{
    [DebuggerDisplay("Type = {Type}")]
    public sealed class TypeAccessor : ITypeAccessor
    {
        private const BindingFlags DeclaredOnlyFlags =
            BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance;

        private const BindingFlags DefaultFlags =
            BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static;

        private readonly Lazy<ConcurrentDictionary<string, IMemberAccessor>> _fieldCache =
            new Lazy<ConcurrentDictionary<string, IMemberAccessor>>(() =>
                new ConcurrentDictionary<string, IMemberAccessor>());

        private readonly Lazy<ConcurrentDictionary<int, IMethodAccessor>> _methodCache =
            new Lazy<ConcurrentDictionary<int, IMethodAccessor>>(() =>
                new ConcurrentDictionary<int, IMethodAccessor>());

        private readonly Lazy<ConcurrentDictionary<string, IMemberAccessor>> _propertyCache =
            new Lazy<ConcurrentDictionary<string, IMemberAccessor>>(() =>
                new ConcurrentDictionary<string, IMemberAccessor>());

        private ConstructorDelegate _constructor;

        public TypeAccessor(Type type)
        {
            type.ThrowIfNull(() => type);

            Type = type;
        }

        public int FieldsCount
        {
            get { return _fieldCache.Value.Count; }
        }

        public int MethodsCount
        {
            get { return _methodCache.Value.Count; }
        }

        public int PropertiesCount
        {
            get { return _propertyCache.Value.Count; }
        }

        public Type Type { get; private set; }

        public object CreateInstance()
        {
            return CreateInstance(new object[] {});
        }

        public T CreateInstance<T>()
        {
            return (T) CreateInstance();
        }

        public object CreateInstance(object[] parameters)
        {
            CreateConstructorDelegate(parameters.ToTypeArray());

            return _constructor(parameters);
        }

        public T CreateInstance<T>(object[] parameters)
        {
            return (T) CreateInstance(parameters);
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
            name.ThrowIfNullOrEmpty(() => name);

            IMemberAccessor accessor;
            if (_fieldCache.Value.TryGetValue(name, out accessor))
                return accessor;

            var fieldInfo = Type.GetField(name);

            return fieldInfo == null ? null : FieldAccessor.Create(fieldInfo);
        }

        public IEnumerable<IMemberAccessor> GetFields()
        {
            return GetFields(DefaultFlags);
        }

        public IMethodAccessor GetMethod(string name, params object[] parameters)
        {
            return GetMethod(name, parameters.ToTypeArray());
        }

        public IMethodAccessor GetMethod(string name, Type[] parameterTypes)
        {
            var key = MethodAccessor.GetKey(name, parameterTypes);

            return _methodCache.Value.GetOrAdd(
                key, MethodAccessor.Create(Type, name, parameterTypes));
        }

        public IEnumerable<IMemberAccessor> GetProperties()
        {
            return GetProperties(DefaultFlags);
        }

        public IMemberAccessor GetProperty(string name)
        {
            name.ThrowIfNullOrEmpty(() => name);

            IMemberAccessor accessor;
            if (_propertyCache.Value.TryGetValue(name, out accessor))
                return accessor;

            var propertyInfo = Type.GetProperty(name);

            return propertyInfo == null ? null : PropertyAccessor.Create(propertyInfo);
        }

        public IMemberAccessor GetProperty<T>(Expression<Func<T, object>> expression)
        {
            expression.ThrowIfNull(() => expression);

            var propertyInfo = expression.GetPropertyAccess();

            if (propertyInfo == null)
                return null;

            IMemberAccessor accessor;
            if (_propertyCache.Value.TryGetValue(propertyInfo.Name, out accessor))
                return accessor;

            accessor = PropertyAccessor.Create(propertyInfo);
            _propertyCache.Value.TryAdd(accessor.Name, accessor);

            return accessor;
        }

        private void CreateConstructorDelegate(Type[] parameterTypes)
        {
            if (_constructor == null)
                _constructor = DelegateFactory.CreateConstructor(Type, parameterTypes);

            if (_constructor == null)
                throw new InvalidOperationException(
                    "Could not find constructor for '{0}'.".FormatWith(Type.Name));
        }

        private IEnumerable<IMemberAccessor> GetFields(BindingFlags bindingAttr)
        {
            if (FieldsCount == 0)
                Type.GetFields(bindingAttr).ForEach(f =>
                    _fieldCache.Value.TryAdd(f.Name, FieldAccessor.Create(f)));

            return _fieldCache.Value.Values;
        }

        private IEnumerable<IMemberAccessor> GetProperties(BindingFlags bindingAttr)
        {
            if (PropertiesCount == 0)
                Type.GetProperties(bindingAttr).ForEach(p =>
                    _propertyCache.Value.TryAdd(p.Name, PropertyAccessor.Create(p)));

            return _propertyCache.Value.Values;
        }
    }
}