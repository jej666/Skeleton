namespace Skeleton.Common.Reflection
{
    using Extensions;
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.Reflection;

    [DebuggerDisplay("Name: {Name}")]
    public class PropertyAccessor : MemberAccessorBase
    {
        private readonly LazyRef<GetterDelegate> _getDelegate;
        private readonly bool _hasGetter;
        private readonly bool _hasSetter;
        private readonly string _name;
        private readonly PropertyInfo _propertyInfo;
        private readonly Type _propertyType;
        private readonly LazyRef<SetterDelegate> _setDelegate;

        public PropertyAccessor(PropertyInfo propertyInfo)
        {
            propertyInfo.ThrowIfNull(() => propertyInfo);

            _propertyInfo = propertyInfo;
            _name = propertyInfo.Name;
            _propertyType = propertyInfo.PropertyType;
            _hasGetter = propertyInfo.CanRead;
            _hasSetter = propertyInfo.CanWrite;

            _getDelegate = new LazyRef<GetterDelegate>(
                () => DelegateFactory.CreateGet(propertyInfo));

            _setDelegate = new LazyRef<SetterDelegate>(
                () => DelegateFactory.CreateSet(propertyInfo));
        }

        public override bool HasGetter
        {
            get { return _hasGetter; }
        }

        public override bool HasSetter
        {
            get { return _hasSetter; }
        }

        public override MemberInfo MemberInfo
        {
            get { return _propertyInfo; }
        }

        public override Type MemberType
        {
            get { return _propertyType; }
        }

        public override string Name
        {
            get { return _name; }
        }

        public static IMemberAccessor Create(PropertyInfo propertyInfo)
        {
            return propertyInfo == null ?
                null :
                new PropertyAccessor(propertyInfo);
        }

        public override object GetValue(object instance)
        {
            if (instance == null)
                return null;

            if (!HasGetter)
                throw new InvalidOperationException(
                    string.Format(CultureInfo.CurrentCulture, "Property '{0}' does not have a getter.", Name));

            return _getDelegate.Value == null ?
                null :
                _getDelegate.Value(instance);
        }

        public override void SetValue(object instance, object value)
        {
            if (value == null || instance == null)
                return;

            if (!HasSetter)
                throw new InvalidOperationException(
                    string.Format(CultureInfo.CurrentCulture, "Property '{0}' does not have a setter.", Name));

            if (_setDelegate.Value == null)
                return;

            _setDelegate.Value(instance, value);
        }
    }
}