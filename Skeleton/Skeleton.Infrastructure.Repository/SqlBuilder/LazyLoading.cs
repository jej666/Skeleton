using System;

namespace Skeleton.Common
{
    public sealed class LazyLoading<T> 
    {
        private Func<T> _initializer;
        private T _value;

        public LazyLoading(Func<T> initializer)
        {
            initializer.ThrowIfNull(() => initializer);

            _initializer = initializer;
        }

        public bool HasValue
        {
            get { return _initializer == null; }
        }

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