using System;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using Skeleton.Common;
using Skeleton.Abstraction.Reflection;
using Skeleton.Core.Reflection.Emitter;

namespace Skeleton.Core.Reflection
{
    [DebuggerDisplay("Name: {Name}")]
    public sealed class PropertyAccessor : MemberAccessorBase
    {
        private readonly PropertyInfo _propertyInfo;
        private readonly LazyRef<GetterDelegate> _getDelegate;
        private readonly LazyRef<SetterDelegate> _setDelegate;

        public PropertyAccessor(PropertyInfo propertyInfo)
        {
            propertyInfo.ThrowIfNull(() => propertyInfo);

            _propertyInfo = propertyInfo;
            Name = propertyInfo.Name;
            MemberType = propertyInfo.PropertyType;
            HasGetter = propertyInfo.CanRead;
            HasSetter = propertyInfo.CanWrite;

            var getEmitter = new GetPropertyEmitter(propertyInfo);
            _getDelegate = new LazyRef<GetterDelegate>(
                () => (GetterDelegate)getEmitter.CreateDelegate());

            var setEmitter = new SetPropertyEmitter(propertyInfo);
            _setDelegate = new LazyRef<SetterDelegate>(
                () => (SetterDelegate)setEmitter.CreateDelegate());
        }

        public override bool HasGetter { get; }

        public override bool HasSetter { get; }

        public override MemberInfo MemberInfo => _propertyInfo;

        public override Type MemberType { get; }

        public override string Name { get; }

        public override object GetValue(object instance)
        {
            instance.ThrowIfNull(() => instance);

            if (!HasGetter)
                throw new InvalidOperationException(
                    string.Format(CultureInfo.CurrentCulture, "Property '{0}' does not have a getter.", Name));

            return _getDelegate.Value?.Invoke(instance);
        }

        public override void SetValue(object instance, object value)
        {
            instance.ThrowIfNull(() => instance);
            value.ThrowIfNull(() => value);

            if (!HasSetter)
                throw new InvalidOperationException(
                    string.Format(CultureInfo.CurrentCulture, "Property '{0}' does not have a setter.", Name));

            _setDelegate.Value?.Invoke(instance, value);
        }

        public static IMemberAccessor Create(PropertyInfo propertyInfo)
        {
            return propertyInfo == null
                ? null
                : new PropertyAccessor(propertyInfo);
        }
    }
}