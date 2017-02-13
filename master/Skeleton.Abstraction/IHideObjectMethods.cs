using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace Skeleton.Abstraction
{
    public interface IHideObjectMethods
    {
        [SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "obj")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        bool Equals(object obj);

        [EditorBrowsable(EditorBrowsableState.Never)]
        int GetHashCode();

        [SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "GetType")]
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        Type GetType();

        [EditorBrowsable(EditorBrowsableState.Never)]
        string ToString();
    }
}