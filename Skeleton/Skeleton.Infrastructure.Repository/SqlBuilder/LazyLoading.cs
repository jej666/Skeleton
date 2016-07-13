using System;

namespace Skeleton.Infrastructure.Repository.SqlBuilder
{
    internal sealed class LazyLoading<T>
    {
        private Func<T> _initializer;
        private T _value;

        internal LazyLoading(Func<T> initializer)
        {
            initializer.ThrowIfNull(() => initializer);

            _initializer = initializer;
        }

        internal bool HasValue
        {
            get { return _initializer == null; }
        }

        internal T Value
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