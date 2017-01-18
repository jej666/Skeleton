using System;
using System.Diagnostics;
using System.Reflection;
using Skeleton.Common;
using Skeleton.Abstraction.Reflection;
using Skeleton.Core.Reflection.Emitter;

namespace Skeleton.Core.Reflection
{
    [DebuggerDisplay("Name: {Name}")]
    public sealed class FieldAccessor : MemberAccessorBase
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

            var getEmitter = new GetFieldEmitter(fieldInfo);
            _getDelegate = new LazyRef<GetterDelegate>(() =>
                   (GetterDelegate)getEmitter.CreateDelegate());

            var setEmitter = new SetFieldEmitter(fieldInfo);
            _setDelegate = new LazyRef<SetterDelegate>(() =>
                    (SetterDelegate)setEmitter.CreateDelegate());
        }

        public override bool HasGetter => true;

        public override bool HasSetter { get; }

        public override MemberInfo MemberInfo => _fieldInfo;

        public override Type MemberType { get; }

        public override string Name { get; }

        public override object GetValue(object instance)
        {
            instance.ThrowIfNull(() => instance);

            return _getDelegate.Value?.Invoke(instance);
        }

        public override void SetValue(object instance, object value)
        {
            instance.ThrowIfNull(() => instance);
            value.ThrowIfNull(() => value);

            if (!HasSetter)
                throw new InvalidOperationException(
                    "Field '{0}' does not have a setter.".FormatWith(Name));

            _setDelegate.Value?.Invoke(instance, value);
        }

        public static IMemberAccessor Create(FieldInfo fieldInfo)
        {
            return fieldInfo == null ? null : new FieldAccessor(fieldInfo);
        }
    }
}