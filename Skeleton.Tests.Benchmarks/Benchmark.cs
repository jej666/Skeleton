using System;
using System.Diagnostics;

namespace Skeleton.Tests.Benchmarks
{
    public class Benchmark
    {
        public Action Executor { get; private set; }

        public string Name { get; private set; }

        public Stopwatch Watch { get; set; }

        public static Benchmark Create(Action executor, string name)
        {
            return new Benchmark {Executor = executor, Name = name};
        }
    }
}