using System;
using System.Diagnostics;
using System.Reflection;
using Skeleton.Abstraction;

namespace Skeleton.Common.Reflection
{
    [DebuggerDisplay("Name: {Name}")]
    public class FieldAccessor : MemberAccessorBase
    {
        private readonly FieldInfo _fieldInfo;
        private readonly LazyRef<GetterDelegate> _getDelegate;
        private readonly bool _hasSetter;
        private readonly Type _memberType;
        private readonly string _name;
        private readonly LazyRef<SetterDelegate> _setDelegate;

        private FieldAccessor(FieldInfo fieldInfo)
        {
            fieldInfo.ThrowIfNull(() => fieldInfo);

            _fieldInfo = fieldInfo;
            _name = fieldInfo.Name;
            _memberType = fieldInfo.FieldType;
            _hasSetter = !fieldInfo.IsInitOnly && !fieldInfo.IsLiteral;

            _getDelegate = new LazyRef<GetterDelegate>(() =>
                DelegateFactory.CreateGet(fieldInfo));
            _setDelegate = new LazyRef<SetterDelegate>(() =>
                DelegateFactory.CreateSet(fieldInfo));
        }

        public override bool HasGetter
        {
            get { return true; }
        }

        public override bool HasSetter
        {
            get { return _hasSetter; }
        }

        public override MemberInfo MemberInfo
        {
            get { return _fieldInfo; }
        }

        public override Type MemberType
        {
            get { return _memberType; }
        }

        public override string Name
        {
            get { return _name; }
        }

        public static IMemberAccessor Create(FieldInfo fieldInfo)
        {
            return fieldInfo == null ? null : new FieldAccessor(fieldInfo);
        }

        public override object GetValue(object instance)
        {
            if (instance == null)
                return null;

            return _getDelegate.Value == null
                ? null
                : _getDelegate.Value(instance);
        }

        public override void SetValue(object instance, object value)
        {
            if (value == null || instance == null)
                return;

            if (!HasSetter)
                throw new InvalidOperationException(
                    "Field '{0}' does not have a setter.".FormatWith(Name));

            if (_setDelegate.Value == null)
                return;

            _setDelegate.Value(instance, value);
        }
    }
}