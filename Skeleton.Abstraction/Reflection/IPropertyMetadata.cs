using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace Skeleton.Abstraction.Reflection
{
    public interface IPropertyMetadata
    {
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        IEnumerable<IMemberAccessor> GetDeclaredOnlyProperties();

        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        IEnumerable<IMemberAccessor> GetProperties();

        IMemberAccessor GetProperty(string name);

        IMemberAccessor GetProperty<T>(Expression<Func<T, object>> expression);
    }
}