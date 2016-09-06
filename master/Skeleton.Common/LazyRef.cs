﻿using System;

namespace Skeleton.Common
{
    public sealed class LazyRef<T> : HideObjectMethods
    {
        private Func<T> _initializer;
        private T _value;

        public LazyRef(Func<T> initializer)
        {
            initializer.ThrowIfNull(() => initializer);

            _initializer = initializer;
        }

        public bool HasValue => _initializer == null;

        public T Value
        {
            get
            {
                if (_initializer == null)
                    return _value;

                _value = _initializer();
                _initializer = null;

                return _value;
            }
        }
    }
}