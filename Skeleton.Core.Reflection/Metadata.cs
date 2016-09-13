using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Skeleton.Abstraction;
using Skeleton.Common;

namespace Skeleton.Core.Reflection
{
    [DebuggerDisplay("Type = {Type}")]
    public sealed class Metadata : HideObjectMethods, IMetadata
    {
        private const BindingFlags DeclaredOnlyFlags =
            BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance;

        private const BindingFlags DefaultFlags =
            BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static;

        private const BindingFlags AllFlags =
            BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic;

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

        public Metadata(Type type)
        {
            type.ThrowIfNull(() => type);

            Type = type;
        }

        public int FieldsCount => _fieldCache.Value.Count;

        public int MethodsCount => _methodCache.Value.Count;

        public int PropertiesCount => _propertyCache.Value.Count;

        public Type Type { get; }

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
            return GetField(name, DefaultFlags);
        }
        
        public IMemberAccessor GetPrivateField(string name)
        {
            return GetField(name, AllFlags);
        }

        private IMemberAccessor GetField(string name, BindingFlags bindings)
        {
            name.ThrowIfNullOrEmpty(() => name);

            IMemberAccessor accessor;
            if (_fieldCache.Value.TryGetValue(name, out accessor))
                return accessor;

            var fieldInfo = Type.GetField(name, bindings);

            return fieldInfo == null ? null : MemberAccessorFactory.Create(fieldInfo);
        }

        public IEnumerable<IMemberAccessor> GetAllFields()
        {
            return GetFields(AllFlags);
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
            var key = GetMethodKey(name, parameterTypes);

            return _methodCache.Value.GetOrAdd(
                key, MemberAccessorFactory.Create(Type, name, parameterTypes));
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

            return propertyInfo == null ? null : MemberAccessorFactory.Create(propertyInfo);
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

            return MemberAccessorFactory.Create(propertyInfo);
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
                        _fieldCache.Value.TryAdd(f.Name, MemberAccessorFactory.Create(f)));

            return _fieldCache.Value.Values;
        }

        private IEnumerable<IMemberAccessor> GetProperties(BindingFlags bindingAttr)
        {
            if (PropertiesCount == 0)
                Type.GetProperties(bindingAttr).ForEach(p =>
                        _propertyCache.Value.TryAdd(p.Name, MemberAccessorFactory.Create(p)));

            return _propertyCache.Value.Values;
        }

        private static int GetMethodKey(string name, IEnumerable<Type> parameterTypes)
        {
            unchecked
            {
                var result = name?.GetHashCode() ?? 0;
                result = parameterTypes.Aggregate(result,
                    (r, p) => (r*397) ^ (p != null ? p.GetHashCode() : 0));

                return result;
            }
        }
    }
}