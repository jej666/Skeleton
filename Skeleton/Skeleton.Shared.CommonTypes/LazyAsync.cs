using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Skeleton.Shared.CommonTypes
{
    public class LazyAsync<T> : Lazy<Task<T>>
    {
        public LazyAsync(Func<T> valueFactory) :
            base(() => Task.Factory.StartNew(valueFactory))
        {
        }

        public LazyAsync(Func<Task<T>> taskFactory) :
            base(() => Task.Factory.StartNew(taskFactory).Unwrap())
        {
        }

        public TaskAwaiter<T> GetAwaiter()
        {
            return Value.GetAwaiter();
        }
    }
}