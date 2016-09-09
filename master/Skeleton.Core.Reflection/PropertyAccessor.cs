using System;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using Skeleton.Common;

namespace Skeleton.Core.Reflection
{
    [DebuggerDisplay("Name: {Name}")]
    public sealed class PropertyAccessor : MemberAccessorBase
    {
        private readonly LazyRef<GetterDelegate> _getDelegate;
        private readonly PropertyInfo _propertyInfo;
        private readonly LazyRef<SetterDelegate> _setDelegate;

        public PropertyAccessor(PropertyInfo propertyInfo)
        {
            propertyInfo.ThrowIfNull(() => propertyInfo);

            _propertyInfo = propertyInfo;
            Name = propertyInfo.Name;
            MemberType = propertyInfo.PropertyType;
            HasGetter = propertyInfo.CanRead;
            HasSetter = propertyInfo.CanWrite;

            _getDelegate = new LazyRef<GetterDelegate>(
                () => DelegateFactory.CreateGet(propertyInfo));

            _setDelegate = new LazyRef<SetterDelegate>(
                () => DelegateFactory.CreateSet(propertyInfo));
        }

        public override bool HasGetter { get; }

        public override bool HasSetter { get; }

        public override MemberInfo MemberInfo => _propertyInfo;

        public override Type MemberType { get; }

        public override string Name { get; }

        public override object GetValue(object instance)
        {
            if (instance == null)
                return null;

            if (!HasGetter)
                throw new InvalidOperationException(
                    string.Format(CultureInfo.CurrentCulture, "Property '{0}' does not have a getter.", Name));

            return _getDelegate.Value?.Invoke(instance);
        }

        public override void SetValue(object instance, object value)
        {
            if ((value == null) || (instance == null))
                return;

            if (!HasSetter)
                throw new InvalidOperationException(
                    string.Format(CultureInfo.CurrentCulture, "Property '{0}' does not have a setter.", Name));

            _setDelegate.Value?.Invoke(instance, value);
        }
    }
}