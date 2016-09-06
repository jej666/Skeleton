using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Skeleton.Shared.Abstraction.Reflection
{
    public interface IPropertyMetadata
    {
        int PropertiesCount { get; }
        IEnumerable<IMemberAccessor> GetDeclaredOnlyProperties();

        IEnumerable<IMemberAccessor> GetProperties();

        IMemberAccessor GetProperty(string name);

        IMemberAccessor GetProperty<T>(Expression<Func<T, object>> expression);
    }
}