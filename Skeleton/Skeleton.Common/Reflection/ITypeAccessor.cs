namespace Skeleton.Common.Reflection
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq.Expressions;

    public interface ITypeAccessor
    {
        int FieldsCount { get; }
        int MethodsCount { get; }
        int PropertiesCount { get; }
        Type Type { get; }

        object CreateInstance();

        object CreateInstance(object[] parameters);

        T CreateInstance<T>();

        T CreateInstance<T>(object[] parameters);

        IEnumerable<IMemberAccessor> GetDeclaredOnlyFields();

        IEnumerable<IMemberAccessor> GetDeclaredOnlyProperties();

        IMemberAccessor GetField(string name);

        IEnumerable<IMemberAccessor> GetFields();

        IMethodAccessor GetMethod(string name, params object[] parameters);

        IMethodAccessor GetMethod(string name, Type[] parameterTypes);

        IEnumerable<IMemberAccessor> GetProperties();

        IMemberAccessor GetProperty(string name);

        IMemberAccessor GetProperty<T>(Expression<Func<T, object>> expression);
    }
}