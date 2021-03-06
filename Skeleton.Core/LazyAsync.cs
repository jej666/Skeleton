﻿using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Skeleton.Core
{
    /// <summary>
    /// https://blogs.msdn.microsoft.com/pfxteam/2011/01/15/asynclazyt/
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class LazyAsync<T> : Lazy<Task<T>>
    {
        public LazyAsync(Func<T> valueFactory) :
            base(() => Task.Factory.StartNew(valueFactory))
        {
        }

        public LazyAsync(Func<Task<T>> taskFactory) :
            base(() => Task.Factory.StartNew(taskFactory).Unwrap())
        {
        }

        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        public TaskAwaiter<T> GetAwaiter()
        {
            return Value.GetAwaiter();
        }
    }
}