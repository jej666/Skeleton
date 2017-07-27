using Skeleton.Abstraction.Reflection;
using Skeleton.Core;
using Skeleton.Core.Reflection.Emitter;
using System;
using System.Diagnostics;
using System.Reflection;

namespace Skeleton.Core.Reflection
{
    [DebuggerDisplay("Name: {Name}")]
    public sealed class PropertyAccessor : MemberAccessorBase
    {
        private readonly PropertyInfo _propertyInfo;
        private readonly Func<object, object> _getDelegate;
        private readonly Action<object, object> _setDelegate;

        public PropertyAccessor(PropertyInfo propertyInfo)
        {
            propertyInfo.ThrowIfNull(nameof(propertyInfo));

            _propertyInfo = propertyInfo;
            Name = propertyInfo.Name;
            MemberType = propertyInfo.PropertyType;
            HasGetter = propertyInfo.CanRead;
            HasSetter = propertyInfo.CanWrite;

            var emitter = new PropertyEmitter(propertyInfo);
            _getDelegate = emitter.CreateGetter();
            _setDelegate = emitter.CreateSetter();
        }

        public override Func<object, object> Getter => _getDelegate;

        public override Action<object, object> Setter => _setDelegate;

        public override bool HasGetter { get; }

        public override bool HasSetter { get; }

        public override MemberInfo MemberInfo => _propertyInfo;

        public override Type MemberType { get; }

        public override string Name { get; }

        internal static IMemberAccessor Create(PropertyInfo propertyInfo)
        {
            return propertyInfo == null
                ? null
                : new PropertyAccessor(propertyInfo);
        }
    }
}