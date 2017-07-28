using Skeleton.Abstraction.Reflection;
using Skeleton.Core.Reflection.Emitter;
using System;
using System.Diagnostics;
using System.Reflection;

namespace Skeleton.Core.Reflection
{
    [DebuggerDisplay("Name: {Name}")]
    public sealed class FieldAccessor : MemberAccessorBase
    {
        private readonly FieldInfo _fieldInfo;
        private readonly Func<object, object> _getDelegate;
        private readonly Action<object, object> _setDelegate;

        public FieldAccessor(FieldInfo fieldInfo)
        {
            fieldInfo.ThrowIfNull(nameof(fieldInfo));

            _fieldInfo = fieldInfo;
            Name = fieldInfo.Name;
            MemberType = fieldInfo.FieldType;
            HasSetter = !fieldInfo.IsInitOnly && !fieldInfo.IsLiteral;

            var emitter = new FieldEmitter(fieldInfo);
            _getDelegate = emitter.CreateGetter();
            _setDelegate = emitter.CreateSetter();
        }

        public override Func<object, object> Getter => _getDelegate;

        public override Action<object, object> Setter => _setDelegate;

        public override bool HasGetter => true;

        public override bool HasSetter { get; }

        public override MemberInfo MemberInfo => _fieldInfo;

        public override Type MemberType { get; }

        public override string Name { get; }

        internal static IMemberAccessor Create(FieldInfo fieldInfo)
        {
            return fieldInfo == null ? null : new FieldAccessor(fieldInfo);
        }
    }
}