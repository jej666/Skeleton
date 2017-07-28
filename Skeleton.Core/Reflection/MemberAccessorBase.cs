using Skeleton.Abstraction.Reflection;
using System;
using System.Diagnostics;
using System.Reflection;

namespace Skeleton.Core.Reflection
{
    [DebuggerDisplay("Name: {Name}")]
    public abstract class MemberAccessorBase :
        IMemberAccessor,
        IEquatable<IMemberAccessor>
    {
        public bool Equals(IMemberAccessor other)
        {
            if (ReferenceEquals(null, other))
                return false;

            return ReferenceEquals(this, other) ||
                   Equals(other.MemberInfo, MemberInfo);
        }

        public abstract Func<object, object> Getter { get; }

        public abstract Action<object, object> Setter { get; }

        public abstract bool HasGetter { get; }
        public abstract bool HasSetter { get; }
        public abstract MemberInfo MemberInfo { get; }
        public abstract Type MemberType { get; }
        public abstract string Name { get; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;

            return (obj.GetType() == typeof(MemberAccessorBase)) &&
                   Equals((MemberAccessorBase)obj);
        }

        public override int GetHashCode()
        {
            return MemberInfo.GetHashCode();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Valider les arguments de méthodes publiques", MessageId = "0")]
        public static bool operator ==(MemberAccessorBase accessorBase, MemberAccessorBase other)
        {
            var isAccessorNull = ReferenceEquals(accessorBase, null);
            var isOtherNull = ReferenceEquals(other, null);

            if (isAccessorNull && isOtherNull)
                return true;

            if (isAccessorNull)
                return false;

            return accessorBase.Equals(other);
        }

        public static bool operator !=(MemberAccessorBase accessorBase, MemberAccessorBase other)
        {
            return !(accessorBase == other);
        }
    }
}