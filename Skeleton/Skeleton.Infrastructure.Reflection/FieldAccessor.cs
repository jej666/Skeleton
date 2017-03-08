﻿using System;
using System.Diagnostics;
using System.Reflection;
using Skeleton.Shared.CommonTypes;

namespace Skeleton.Core.Reflection
{
    [DebuggerDisplay("Name: {Name}")]
    public class FieldAccessor : MemberAccessorBase
    {
        private readonly FieldInfo _fieldInfo;
        private readonly LazyRef<GetterDelegate> _getDelegate;
        private readonly LazyRef<SetterDelegate> _setDelegate;

        public FieldAccessor(FieldInfo fieldInfo)
        {
            fieldInfo.ThrowIfNull(() => fieldInfo);

            _fieldInfo = fieldInfo;
            Name = fieldInfo.Name;
            MemberType = fieldInfo.FieldType;
            HasSetter = !fieldInfo.IsInitOnly && !fieldInfo.IsLiteral;

            _getDelegate = new LazyRef<GetterDelegate>(() =>
                    DelegateFactory.CreateGet(fieldInfo));
            _setDelegate = new LazyRef<SetterDelegate>(() =>
                    DelegateFactory.CreateSet(fieldInfo));
        }

        public override bool HasGetter
        {
            get { return true; }
        }

        public override bool HasSetter { get; }

        public override MemberInfo MemberInfo
        {
            get { return _fieldInfo; }
        }

        public override Type MemberType { get; }

        public override string Name { get; }

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
            if ((value == null) || (instance == null))
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